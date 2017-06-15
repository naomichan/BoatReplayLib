using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Interfaces.SuperTemplates
{
	public interface ISpotted : IBlankSuperTemplate
	{
		GameSpotting GetSpottingType();
	}
}
