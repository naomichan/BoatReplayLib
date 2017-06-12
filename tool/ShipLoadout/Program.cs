using BoatReplayLib;
using System;
using System.IO;
using Ionic.Zlib;
using System.Linq;

namespace ShipLoadout {
    class MainClass {
        public static void Main(string[] args) {
            if (args.Length == 0) {
                Console.Error.WriteLine("Usage: ShipLoadout GameParams.data");
                return;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File {args[0]} does not exist");
                return;
            }

            byte[] bytes = File.ReadAllBytes(args[0]).Reverse().ToArray();
            byte[] data = ZlibStream.UncompressBuffer(bytes);

            object[] GameParams = Unpickler.load(data);
        }
    }
}
