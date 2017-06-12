using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.Version066Scenario {
    [GamePacket(Name = "Map", Type = 0x27)]
    public class Map : IGamePacketTemplate {
        public uint Unknown1;
        public ushort Unknown2;
        public uint Unknown3;
        public ushort Unknown4;
        public uint Length;
        [GamePacketField(DynamicSizeReference = "Length")]
        public string MapName;
        public Matrix4 Matrix;
        public byte Unknown5;
    }
}
