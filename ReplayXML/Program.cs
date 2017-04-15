using System;
using System.IO;
using BoatReplayLib.Packets;
using BoatReplayLib.Packets.WOWS_0_6_3_1;

namespace ReplayXML {
  class Program {
    static void Main(string[] args) {
      GamePacketTemplateFactory factory = GamePacketTemplateFactory.GetInstance();
      if(args.Length == 0) {
        Console.Error.WriteLine("Usage: ReplayXML replay.bin");
        return;
      }
      if(!File.Exists(args[0])) {
        Console.Error.WriteLine($"File {args[0]} does not exist");
        return;
      }
      using(Stream file = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read)) {
        BigWorldPacketCollection packets = factory.ReadAll(file, typeof(WarshipsNamespace));
        System.Diagnostics.Debugger.Break();
      }
    }
  }
}
