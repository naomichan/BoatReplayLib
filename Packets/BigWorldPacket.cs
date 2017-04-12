using System;
using System.IO;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  class BigWorldPacket : IGamePacketTemplate, IDisposable {
    public uint Size;
    public uint Type;
    public float Time;
    [GamePacketField(DynamicSizeReference = "Size", PolymorphicReference = "Type", Fallback = typeof(MemoryPacket))]
    public IGamePacketTemplate Data = null;
    
    public void Dispose() {
      if(Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
        ((IDisposable) Data).Dispose();
      }
    }
  }
}
