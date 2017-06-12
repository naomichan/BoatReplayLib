using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Version066Scenario.WorldLogicSubtypes {
    [GamePacket(Name = "IsAlive", Type = 0x4)]
    public class IsAlive : IGamePacketTemplate {
        public bool State;
    }
}
