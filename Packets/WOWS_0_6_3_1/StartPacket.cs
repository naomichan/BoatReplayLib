using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x03, Name = "UnknownPacket03")]
    public class Packet03 : IGamePacketTemplate {
        public uint NetworkAvatarId;
        public uint Unknown1;
        public uint Unknown2;
    }
}
