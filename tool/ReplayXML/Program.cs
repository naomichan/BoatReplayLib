using System;
using System.IO;
using System.Xml.Linq;
using BoatReplayLib.Packets;
using BoatReplayLib;
using BoatReplayLib.Packets.Generic;

namespace ReplayXML {
    class Program {
        static void Main(string[] args) {
            GamePacketTemplateFactory factory = GamePacketTemplateFactory.GetInstance();
            if (args.Length == 0) {
                Console.Error.WriteLine("Usage: ReplayXML replay.wowsreplay [dumpfile]");
                return;
            }
            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File {args[0]} does not exist");
                return;
            }
            using (Stream file = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read)) {
                using (Replay replay = new Replay(file)) {
                    if (args.Length > 1) {
                        using (Stream output = File.Open(args[1], FileMode.Create, FileAccess.Write, FileShare.Write)) {
                            replay.Data.CopyTo(output);
                        }
                    }
                    GamePacketTemplateFactory.WHINEY = false;
                    Type ns = factory.GetClosestNamespace(replay.GameVersion[0], replay.GameVersion[1], replay.gameVersion[2]);
                    BigWorldPacketCollection packets = factory.ReadAll(replay.Data, ns, BigWorldPacketCollection.CollectionMode.Packets);
                    XElement root = new XElement("Replay", new XAttribute("Version", replay.ParsedJSON.clientVersionFromExe));
                    foreach (BigWorldPacket packet in packets.Packets) {
                        XElement element = XMLWriter.CreateXML(packet);
                        if (element == null) {
                            continue;
                        }
                        root.Add(element);
                    }

                    Console.Out.WriteLine(root);

                    if (args.Length <= 2) return;

                    using (var sw = new StreamWriter(args[2]))
                    {
                        sw.Write(root);
                    } //end using
                }
            }
        }
    }
}
