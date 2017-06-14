using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.Version066Scenario {
    [GamePacket(Name = "Map", Type = 0x27)]
    public class Map : IGamePacketTemplate {
        public uint Unknown1;
        public ulong SessionId;
        public uint Length;
        [GamePacketField(DynamicSizeReference = "Length")]
        public string MapName;
        public Matrix4 Matrix;
        public byte Unknown5;
    }
}
