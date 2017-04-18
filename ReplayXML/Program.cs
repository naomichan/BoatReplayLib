using System;
using System.IO;
using BoatReplayLib.Packets;

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
        BigWorldPacketCollection packets = factory.ReadAll(file, factory.GetNamespace("0,6,3,1"), BigWorldPacketCollection.CollectionMode.SubType);
        System.Diagnostics.Debugger.Break();
      }
    }
  }
}
