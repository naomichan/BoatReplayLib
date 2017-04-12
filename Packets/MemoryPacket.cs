using System;
using System.IO;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  public class MemoryPacket : IGamePacketTemplate, IDisposable {
    public MemoryStream Data;

    public void Dispose() {
      Data.Dispose();
    }
  }
}
