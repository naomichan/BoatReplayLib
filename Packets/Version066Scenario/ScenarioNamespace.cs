using System;
using BoatReplayLib.Interfaces;
namespace BoatReplayLib.Packets.Version066Scenario
{
    public class ScenarioNamespace : IGamePacketNamespace
    {

        public short[] GameVersion() => new short[3] { 0, 6, 6 };

        public Type Inherits() => null;
    }
}
