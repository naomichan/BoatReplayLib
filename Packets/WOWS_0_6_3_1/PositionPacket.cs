using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x0A, Name = "Position")]
  public class Packet0A : IGamePacketTemplate, IPosition {
    public uint NetworkAvatarId;
    public uint Unknown1;
    public uint Unknown2;
    public Float3 Position;
    public uint Unknown3;
    public uint Unknown4;
    public uint Unknown5;
    public Float3 Rotation;

    public Float3 GetPosition() => Position;
    public Float3 GetRotation() => Rotation;
  }
}
