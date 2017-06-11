using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Interfaces.SuperTemplates {
    public interface IPosition : IBlankSuperTemplate {
        Float3 GetPosition();
        Float3 GetRotation();
    }
}
