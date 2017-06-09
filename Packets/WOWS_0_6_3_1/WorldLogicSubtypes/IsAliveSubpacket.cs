using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.WorldLogicSubtypes {
    [GamePacket(Name = "IsAlive", Type = 0x4)]
    public class IsAliveSubpacket : IGamePacketTemplate {
        public bool State;
    }
}
