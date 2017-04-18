using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.UnknownPacket05Subtypes {
  [GamePacket(Name = "UnknownSubtype04", Type = 0x04)]
  public class UnknownSubtype04 : IGamePacketTemplate {
    public ushort Unknown1;
    public uint Unknown2;
    public uint Unknown3;
    public ushort Unknown4;
    public uint Unknown5;
    public uint Unknown6;
    public Float2 Position;
  }
}
