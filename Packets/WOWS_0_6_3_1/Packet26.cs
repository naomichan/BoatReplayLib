using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x26, Name = "UnknownPacket26")]
    public class Packet26 : IGamePacketTemplate {
        public uint Unknown1;
    }
}
