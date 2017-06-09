using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x25, Name = "UnknownPacket25")]
    public class Packet25 : IGamePacketTemplate {
        public uint NetworkAvatarId;
        public uint Unknown1;
        public ushort Unknown2;
    }
}
