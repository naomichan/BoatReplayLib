using System.Collections.Generic;

namespace BoatReplayLib.Interfaces.SuperTemplates {
    public interface IAvatarInfo : IPickled {
        IReadOnlyDictionary<string, object>[] GetAvatarInfo();
    }
}
