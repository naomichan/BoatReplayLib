using System.Collections.Generic;

namespace BoatReplayLib.Interfaces.SuperTemplates {
    public interface IPickled : IBlankSuperTemplate {
        object GetPickle();
    }
}
