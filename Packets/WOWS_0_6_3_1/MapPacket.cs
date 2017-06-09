using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x27, Name = "MapPacket")]
    public class MapPacket : IGamePacketTemplate {
        public uint Unknown1;
        public float LoadTime;
        public uint MapId;
        public uint Length;
        [GamePacketField(DynamicSizeReference = "Length")]
        public string MapName;
        public Matrix4 Spawn;
        public byte Unknown2;
    }
}
