using System.Collections.Generic;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x05, Name = "UnknownPacket05", SubTypes = true)]
  public class Packet05 : IGamePacketTemplate {
    public uint NetworkAvatarId;
    public ushort Subtype;
    public uint Unknown1;
    public uint Unknown2;
    public Float4 Unknown3;
    public uint Unknown4;
    public uint Unknown5;
    public uint Size;
    [GamePacketField(DynamicSizeReference = "Size", PolymorphicReference = "Subtype")]
    public IGamePacketTemplate Data;
  }
}
