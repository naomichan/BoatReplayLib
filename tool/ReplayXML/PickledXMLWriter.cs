using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BoatReplayLib;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace ReplayXML {
    public class PickledXMLWriter {
        private static Type T_INT = typeof(int);
        private static Type T_LONG = typeof(long);

		private static void PythonClassToXML(XElement root, Unpickler.PythonClass cls) {
			root.Add(new XElement("Name", cls.Name));
			root.Add(new XElement("Module", cls.Module));
			XElement args = new XElement("Args");
			Decide(args, cls.Args);
			root.Add(args);
			XElement inst = new XElement("Instance");
			Decide(inst, cls.Dict);
			root.Add(inst);
        }

		private static void Decide(XElement element, object entry) {
            Type t = entry.GetType();
			if (t.Name == "Dictionary`2") {
				DictToXML(element, entry as Dictionary<object, object>);
			} else if (t.Name == "List`1") {
				ListToXML(element, entry as List<object>);
            } else if(t.Name == "PythonClass") {
                PythonClassToXML(element, entry as Unpickler.PythonClass);
            } else if (t.IsArray) {
				ListToXML(element, (entry as object[]).ToList());
			} else {
				element.SetValue(entry);
			}
        }

        private static void DictToXML(XElement root, Dictionary<object, object> dict) {
            foreach (KeyValuePair<object, object> pair in dict) {
                string key = pair.Key.ToString();
                int n;
                if(int.TryParse(key[0].ToString(), out n)) {
                    key = "i" + key;
                }
                XElement element = new XElement(key, new XAttribute("Type", pair.Value.GetType().Name.Split('`')[0]));
                Decide(element, pair.Value);
                root.Add(element);
            }
        }

        private static void ListToXML(XElement root, List<object> list) {
            foreach (object entry in list) {
                XElement element = new XElement("value", new XAttribute("Type", entry.GetType().Name.Split('`')[0]));
                Decide(element, entry);
                root.Add(element);
            }
		}

        public static void CreateXML(XElement root, IPickled info) {
            if (info == null) {
                return;
            }
            object data = info.GetPickle();
            if(data == null) {
                return;
            }

			Decide(root, data);
        }

		public static void CreateXML(XElement root, IAvatarInfo info) {
			if (info == null) {
				return;
			}

            IReadOnlyDictionary<string, object>[] data = info.GetAvatarInfo();

            foreach (IReadOnlyDictionary<string, object> entry in data) {
                XElement player = new XElement("Player");
                string name = entry["Name"] as string;
                int networkAvatarId = (int)Convert.ChangeType(entry["NetworkAvatarId"], T_INT);
                int worldAvatarId = (int)Convert.ChangeType(entry["WorldAvatarId"], T_INT);
                long id = (long)Convert.ChangeType(entry["UserId"], T_LONG);

                player.SetAttributeValue("NetworkAvatarId", networkAvatarId);
                player.SetAttributeValue("WorldAvatarId", worldAvatarId);
                player.SetAttributeValue("UserId", id);
                player.SetAttributeValue("Name", name);

                root.Add(player);

                foreach (KeyValuePair<string, object> pair in entry) {
                    if (player.Attribute(pair.Key) != null) {
                        continue;
                    }
                    XElement element = new XElement(pair.Key, new XAttribute("Type", pair.Value.GetType().Name.Split('`')[0]));
                    Decide(element, pair.Value);
                    player.Add(element);
                }
            }
        }
    }
}
