using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes {
    [GamePacket(Name = "Ribbon", Type = 0x6)]
    public class Ribbon : IGamePacketTemplate, IRibbon {
        public GameRibbon Value;

        public GameRibbon GetRibbon() => Value;
    }
}
