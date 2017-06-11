using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace ReplayXML {
    public class AvatarInfoXMLWriter {
        private static Type T_INT = typeof(int);
        private static Type T_LONG = typeof(long);

        public static XElement CreateXML(IGamePacketTemplate packet) {
            if (packet == null) {
                return null;
            }

            Type t = typeof(IAvatarInfo);
            Type r = typeof(IRepresentative);
            while (r.IsAssignableFrom(packet.GetType())) {
                packet = (packet as IRepresentative).GetInnerData();
                if (t.IsAssignableFrom(packet.GetType())) {
                    XElement root = new XElement("AvatarInfo");
                    return CreateXML(root, packet as IAvatarInfo);
                }
            }
            return null;
        }

        private static void DictToXML(XElement root, Dictionary<object, object> dict) {
            foreach (KeyValuePair<object, object> pair in dict) {
                string key = pair.Key as string;
                XElement element = new XElement(key, new XAttribute("Type", pair.Value.GetType().Name.Split('`')[0]));
                if (pair.Value.GetType().Name == "Dictionary`2") {
                    DictToXML(element, pair.Value as Dictionary<object, object>);
                } else if (pair.Value.GetType().Name == "List`1") {
                    ListToXML(element, pair.Value as List<object>);
                } else {
                    element.SetValue(pair.Value);
                }
                root.Add(element);
            }
        }

        private static void ListToXML(XElement root, List<object> list) {
            foreach (object entry in list) {
                XElement element = new XElement("value", new XAttribute("Type", entry.GetType().Name.Split('`')[0]));
                if (entry.GetType().Name == "Dictionary`2") {
                    DictToXML(element, entry as Dictionary<object, object>);
                } else if (entry.GetType().Name == "List`1") {
                    ListToXML(element, entry as List<object>);
                } else {
                    element.SetValue(entry);
                }
                root.Add(element);
            }
        }

        public static XElement CreateXML(XElement root, IAvatarInfo info) {
            if (info == null) {
                return null;
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
                    if (pair.Value.GetType().Name == "Dictionary`2") {
                        DictToXML(element, pair.Value as Dictionary<object, object>);
                    } else if (pair.Value.GetType().Name == "List`1") {
                        ListToXML(element, pair.Value as List<object>);
                    } else {
                        element.SetValue(pair.Value);
                    }
                    player.Add(element);
                }
            }

            return root;
        }
    }
}
