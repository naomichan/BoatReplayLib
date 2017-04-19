using System;
namespace BoatReplayLib.Interfaces.SuperTemplates {
  public interface IRepresentative : IBlankSuperTemplate {
    Type Represents();
    IGamePacketTemplate GetInnerData();
  }
}
