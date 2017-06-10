using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets;
using BoatReplayLib.Packets.Generic;

namespace ReplayXML {
    public class XMLWriter {
		private static Type STREAM = typeof(MemoryPacket);
        private static Type GPT = typeof(IGamePacketTemplate);
		private static Type REPRES = typeof(IRepresentative);
		private static Type AVATAR = typeof(IAvatarInfo);
		private static Type BW = typeof(BigWorldPacket);

		public static void CreateXMLInner(XElement root, object obj, FieldInfo field, Type fieldType) {
			XElement element = new XElement(field.Name);
			element.SetAttributeValue("Type", fieldType.Name);
            if (fieldType.IsArray) {
                CreateXMLInnerArray(element, (Array)obj, field, fieldType);
            } else {
                if (GPT.IsAssignableFrom(fieldType)) {
                    CreateXML(element, obj as IGamePacketTemplate);
                } else {
                    element.SetValue(obj);
                }
            }
            root.Add(element);
		}

		public static void CreateXMLInnerArray(XElement root, Array obj, FieldInfo field, Type fieldType) {
            for (int i = 0; i < obj.Length; ++i) {
                CreateXMLInner(root, obj.GetValue(i), field, fieldType.GetElementType());
            }
		}

		public static XElement CreateXML(IGamePacketTemplate packet) {
			Type t = packet.GetType();
            GamePacketAttribute attr = GamePacketTemplateFactory.GetInstance().GetGamePacketAttribute(t);
            XElement root = null;
            if (attr == null) {
                if (BW.IsAssignableFrom(t)) {
                    root = new XElement("Packet");
                } else {
                    return null;
                }
            } else {
                root = new XElement(attr.Name);
            }
            CreateXML(root, packet);
            if(root.IsEmpty) {
                return null;
            }
            return root;
        }

        public static void CreateXML(XElement root, IGamePacketTemplate packet) {
            Type t = packet.GetType();
            if (REPRES.IsAssignableFrom(t)) {
                IRepresentative r = packet as IRepresentative;
                if (r.Represents() == null || STREAM.IsAssignableFrom(r.Represents())) {
                    return;
                }
            }
            if (STREAM.IsAssignableFrom(t)) {
                return;
            }
            
            if(BW.IsAssignableFrom(t)) {
                BigWorldPacket bw = packet as BigWorldPacket;
                root.SetAttributeValue("Time", bw.Time);
                XElement element = CreateXML(bw.Data);
                if(element == null) {
                    return;
                }
                root.Add(element);
            } else if(AVATAR.IsAssignableFrom(t)) {
                root.Add(AvatarInfoXMLWriter.CreateXML(packet as IAvatarInfo));
            } else {
				FieldInfo[] fields = t.GetFields();
				foreach (FieldInfo field in fields) {
					if (!field.IsPublic) {
						continue;
					}
					CreateXMLInner(root, field.GetValue(packet), field, field.FieldType);
				}
            }
        }
    }
}
