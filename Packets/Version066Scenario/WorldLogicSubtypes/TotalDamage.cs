using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Version066Scenario.WorldLogicSubtypes {
	[GamePacket(Name = "TotalDamage", Type = 0x10)]
	public class TotalDamage : IGamePacketTemplate {
		public float Damage;
	}
}
