using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.UnknownPacket05Subtypes {
  public class WeatherParameter : IGamePacketTemplate {
    public float Weight;
    public ushort Unknown1;
    public ushort Unknown2;
    public ushort Unknown3;
  }
}
