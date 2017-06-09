using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x02, Name = "Start")]
    public class StartPacket : IGamePacketTemplate {
        public uint NetworkAvatarId;
        public byte Unknown1;
    }
}
