using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0xFFFFFFFF, Name = "End")]
  public class EndPacket : IGamePacketTemplate {
    public uint Unknown1;
    public uint Unknown2;
    public uint Unknown3;
    public uint Unknown4;
  }
}
