using System;

namespace BoatReplayLib.Interfaces {
  public interface IGamePacketNamespace {
    string GameVersion();

    Type Inherits();
  }
}
