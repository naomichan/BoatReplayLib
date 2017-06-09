using System;

namespace BoatReplayLib.Interfaces {
    public interface IGamePacketNamespace {
        short[] GameVersion();

        Type Inherits();
    }
}
