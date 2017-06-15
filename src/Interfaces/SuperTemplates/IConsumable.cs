using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Interfaces.SuperTemplates
{
	public interface IConsumable : IBlankSuperTemplate
	{
		GameConsumable GetConsumable();
        float GetConsumableDuration();
	}
}
