using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x22, Name = "UnknownPacket22")]
  public class Packet22 : IGamePacketTemplate {
    public uint NetworkAvatarId;
    public byte Unknown1;
    public uint Length;
    [GamePacketField(DynamicSizeReference = "Length")]
    public byte[] Data;
  }
}
