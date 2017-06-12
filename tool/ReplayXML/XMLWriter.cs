using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets;
using BoatReplayLib.Packets.Generic;

namespace ReplayXML {
    public static class XMLWriter {
        private static readonly Type MEMORYPACKET = typeof(MemoryPacket);
        private static readonly Type STREAM = typeof(Stream);
        private static readonly Type GPT = typeof(IGamePacketTemplate);
        private static readonly Type REPRES = typeof(IRepresentative);
        private static readonly Type AVATAR = typeof(IAvatarInfo);
        private static readonly Type PICKLED = typeof(IPickled);
        private static readonly Type BW = typeof(BigWorldPacket);

        public static void CreateXMLInner(XElement root, object obj, FieldInfo field, Type fieldType) {
            XElement element = new XElement(field.Name);
            element.SetAttributeValue("Type", fieldType.Name);
            if (fieldType.IsArray) {
                if (fieldType.GetElementType().Name == "Byte" || fieldType.GetElementType().Name == "SByte") {
                    //element.SetValue(Convert.ToBase64String((byte[])obj));
                    return;
                } else {
                    CreateXMLInnerArray(element, (Array)obj, field, fieldType);
                }
            } else {
                if (GPT.IsAssignableFrom(fieldType)) {
                    CreateXML(element, obj as IGamePacketTemplate);
                    Type t = (obj as IGamePacketTemplate).GetType();
                    GamePacketAttribute attr = GamePacketTemplateFactory.GetInstance().GetGamePacketAttribute(t);
                    if (attr != null) {
                        element.SetAttributeValue("Type", attr.Name);
                    }
                } else {
                    if (STREAM.IsAssignableFrom(fieldType)) {
                        //Stream stream = (Stream)obj;
                        //stream.Position = 0;
                        //byte[] buffer = new byte[stream.Length];
                        //stream.Read(buffer, 0, (int)stream.Length);
                        //element.SetValue(Convert.ToBase64String(buffer));
                        return;
                    } else {
                        element.SetValue(obj);
                    }
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
            if (root.IsEmpty) {
                return null;
            }
            return root;
        }

        public static void CreateXML(XElement root, IGamePacketTemplate packet) {
            if (packet == null) {
                return;
            }
            Type t = packet.GetType();
            if (REPRES.IsAssignableFrom(t)) {
                IRepresentative r = packet as IRepresentative;
                if (r.Represents() == null || MEMORYPACKET.IsAssignableFrom(r.Represents())) {
                    return;
                }
            }
            if (MEMORYPACKET.IsAssignableFrom(t)) {
                return;
            }

            if (BW.IsAssignableFrom(t)) {
                BigWorldPacket bw = packet as BigWorldPacket;
                root.SetAttributeValue("Time", bw.Time);
                XElement element = CreateXML(bw.Data);
                if (element == null) {
                    return;
                }
                root.SetAttributeValue("Type", element.Name);
                root.Add(element.Elements());
            } else if (AVATAR.IsAssignableFrom(t)) {
                PickledXMLWriter.CreateXML(root, packet as IAvatarInfo);
            } else if (PICKLED.IsAssignableFrom(t)) {
                PickledXMLWriter.CreateXML(root, packet as IPickled);
            } else {
                FieldInfo[] fields = t.GetFields();
                foreach (FieldInfo field in fields) {
                    if (!field.IsPublic) {
                        continue;
                    }
                    if (GamePacketTemplateFactory.GetInstance().IsReferenced(t, field)) {
                        continue;
                    }
                    CreateXMLInner(root, field.GetValue(packet), field, field.FieldType);
                }
            }
        }
    }
}
