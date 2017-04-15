using System;
using System.IO;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Generic {
  public class MemoryPacket : IGamePacketTemplate, IDisposable {
    public MemoryStream Data;

    public void Dispose() {
      if(Data != null) {
        Data.Dispose();
      }
    }
  }
}
