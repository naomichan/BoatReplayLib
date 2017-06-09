using System.Collections.Generic;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x01, Name = "UnknownPacket01")]
    public class Packet01 : IGamePacketTemplate {
        public uint NetworkAvatarId;
        public ushort Unknown1;
        public ushort Unknown2;
        public Float3 Unknown3;
        public Float3 Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint WorldAvatarId;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public ushort UnknownA;
        public uint UnknownB;
        public uint UnknownC;
    }
}
