using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0xE, Name = "UnknownPacket0E")]
    public class Packet0E : IGamePacketTemplate {
        public uint Unknown1;
        public float Unknown2;
    }
}
