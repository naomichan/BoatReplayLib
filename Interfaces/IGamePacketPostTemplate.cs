using System.Collections.Generic;

namespace BoatReplayLib.Interfaces {
  public interface IGamePacketPostTemplate : IGamePacketTemplate {
    void PostProcessing();
    Dictionary<string, object> SpecialValues();
  }
}
