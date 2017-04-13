using System;
using System.Collections.Generic;
using System.IO;
using BoatReplayLib.Packets;
using BoatReplayLib.Packets.WOWS_0_6_3_1;
using BoatReplayLib.Packets.WOWS_0_6_3_1.GameLogicSubtypes;

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
        List<BigWorldPacket> packets = factory.ReadAll(file, typeof(WarshipsNamespace));
        GameLogicPacket glp = packets[0].Data as GameLogicPacket;
        AvatarInfoSubpacket aisp = glp.Data as AvatarInfoSubpacket;
        aisp.ParsePickle();
        System.Diagnostics.Debugger.Break();
      }
    }
  }
}
