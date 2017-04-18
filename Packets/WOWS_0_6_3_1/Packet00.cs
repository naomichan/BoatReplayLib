using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x00, Name = "UnknownPacket00")]
  public class Packet00 : IGamePacketTemplate {
    public uint NetworkAvatarId;
    public ushort Unknown1;
    public uint Unknown2;
    public uint Unknown3;
  }
}
