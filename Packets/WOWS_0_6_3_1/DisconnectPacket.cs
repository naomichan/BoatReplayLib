using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x04, Name = "Disconnect")]
    public class DisconnectPacket : IGamePacketTemplate {
        public uint NetworkAvatarId;
    }
}
