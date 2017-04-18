using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x7, Name = "WorldLogic", SubTypes = true)]
  public class WorldLogicPacket : IGamePacketTemplate, IDisposable {
    public uint NetworkAvatarId;
    public uint Subtype;
    public uint Length;
    [GamePacketField(DynamicSizeReference = "Length", PolymorphicReference = "Subtype")]
    public IGamePacketTemplate Data;

    public void Dispose() {
      if(Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
        ((IDisposable)Data).Dispose();
      }
    }
  }
}
