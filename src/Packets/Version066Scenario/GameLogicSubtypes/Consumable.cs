using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes
{
	[GamePacket(Name = "Consumable", Type = 0xE)]
	public class Consumable : IGamePacketTemplate, IConsumable
	{
		public GameConsumable Value;
		public float Duration;

		public GameConsumable GetConsumable() => Value;
		public float GetConsumableDuration() => Duration;
	}
}
