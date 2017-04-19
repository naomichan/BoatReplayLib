using System.Collections.Generic;

namespace BoatReplayLib.Interfaces.SuperTemplates {
  public interface IAvatarInfo : IBlankSuperTemplate {
    IReadOnlyDictionary<string, object>[] GetAvatarInfo();
  }
}
