using System;
using System.IO;
using System.Xml.Linq;
using BoatReplayLib.Packets;
using BoatReplayLib;
using BoatReplayLib.Interfaces.SuperTemplates;

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
                    replay.Data.Position = 0;
                    Type ns = factory.GetClosestNamespace(replay.GameVersion[0], replay.GameVersion[1], replay.gameVersion[2]);
                    BigWorldPacketCollection packets = factory.ReadAll(replay.Data, ns, BigWorldPacketCollection.COLLECT_ALL);
                    XElement avatarInfo = AvatarInfoXMLWriter.CreateXML(packets.GetFirst<IAvatarInfo>());
                    if (avatarInfo != null) {
                        Console.Out.WriteLine(avatarInfo);
                    }
                }
            }
        }
    }
}
