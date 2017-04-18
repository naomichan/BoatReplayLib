using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x18, Name = "UnknownPacket18")]
  public class Packet18 : IGamePacketTemplate {
    [GamePacketField(Size = 13)]
    public float[] Unknown;
  }
}
