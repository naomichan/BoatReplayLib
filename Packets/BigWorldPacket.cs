using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  public class BigWorldPacket : IGamePacketTemplate, IDisposable {
    public uint Size;
    public uint Type;
    public float Time;
    [GamePacketField(DynamicSizeReference = "Size", PolymorphicReference = "Type")]
    public IGamePacketTemplate Data = null;
    
    public void Dispose() {
      if(Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
        ((IDisposable) Data).Dispose();
      }
    }

    public Dictionary<string, object> SpecialValues() {
      return null;
    }
  }
}
