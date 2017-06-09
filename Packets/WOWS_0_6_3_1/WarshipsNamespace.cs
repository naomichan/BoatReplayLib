using System;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    public class WarshipsNamespace : IGamePacketNamespace {
        public short[] GameVersion() {
            return new short[3] { 0, 6, 3 };
        }

        public Type Inherits() {
            return null;
        }
    }
}
