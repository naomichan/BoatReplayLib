using System.Collections.Generic;

namespace BoatReplayLib.Interfaces {
  public interface IGamePacketTemplate {
    Dictionary<string, object> SpecialValues();
  }
}
