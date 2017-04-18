using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0xF, Name = "UnknownPacket0F")]
  public class Packet0F : IGamePacketTemplate {
    public uint Unknown1;
    public float Unknown2;
  }
}
