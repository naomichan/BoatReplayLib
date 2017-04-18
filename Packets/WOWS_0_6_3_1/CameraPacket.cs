using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x24, Name = "CameraPacket")]
  public class CameraPacket : IGamePacketTemplate {
    [GamePacketField(Size = 14)]
    public float[] Matrix;
  }
}
