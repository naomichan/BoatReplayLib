using static BoatReplayLib.Packets.Generic.GameTypes;

namespace BoatReplayLib.Interfaces.SuperTemplates {
    public interface IRibbon : IBlankSuperTemplate {
       GameRibbon GetRibbon();
    }
}
