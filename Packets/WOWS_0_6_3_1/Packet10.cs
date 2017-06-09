using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x10, Name = "UnknownPacket10")]
    public class Packet10 : IGamePacketTemplate {
        public byte Unknown1;
    }
}
