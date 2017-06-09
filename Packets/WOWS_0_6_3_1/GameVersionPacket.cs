using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x16, Name = "GameVersion")]
    public class GameVersionPacket : IGamePacketTemplate {
        public uint Length;
        [GamePacketField(DynamicSizeReference = "Length")]
        public string Version;
    }
}
