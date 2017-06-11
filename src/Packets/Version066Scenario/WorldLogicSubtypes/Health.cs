using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Version066Scenario.WorldLogicSubtypes {
	[GamePacket(Name = "Health", Type = 0xF)]
	public class Health : IGamePacketTemplate {
		public float HP;
	}
}
