using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes
{
	[GamePacket(Name = "Spotted", Type = 0x5)]
	public class Spotted : IGamePacketTemplate, ISpotted
	{
		public GameSpotting Value;

		public GameSpotting GetSpottingType() => Value;
	}
}
