using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x20, Name = "UnknownPacket20")]
  public class Packet20 : IGamePacketTemplate {
    public uint WorldAvatarId;
  }
}
