using System;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Generic {
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

    public bool HasSubtypes() {
      object[] attribs = Data.GetType().GetCustomAttributes(typeof(GamePacketAttribute), false);
      if(attribs.Length == 0) {
        return false;
      }
      GamePacketAttribute attrib = attribs[0] as GamePacketAttribute;
      return attrib.SubTypes;
    }

    public uint GetSubtype() {
      object[] attribs = Data.GetType().GetCustomAttributes(typeof(GamePacketAttribute), false);
      if(attribs.Length == 0) {
        return 0xFFFFFFFF;
      }
      GamePacketAttribute attrib = attribs[0] as GamePacketAttribute;
      if(!attrib.SubTypes) {
        return 0xFFFFFFFF;
      }
      return attrib.FindFirstSubtype(Data);
    }
  }
}
