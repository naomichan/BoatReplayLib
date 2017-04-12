using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.WOWS_0_6_3_1.GameLogicSubtypes;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x8, Name = "GameLogic", SubTypes = new Type[] { typeof(AvatarInfoSubpacket) })]
  public class GameLogicPacket : IGamePacketTemplate, IDisposable {
    public uint AvatarId;
    public uint Subtype;
    public uint Length;
    [GamePacketField(DynamicSizeReference = "Length", PolymorphicReference = "Subtype", Fallback = typeof(MemoryPacket))]
    public IGamePacketTemplate Data;

    public void Dispose() {
      if(Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
        ((IDisposable) Data).Dispose();
      }
    }
  }
}
