using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.UnknownPacket05Subtypes {
    [GamePacket(Name = "WeatherSubtype", Type = 0x0B)]
    public class WeatherSubtype : IGamePacketTemplate {
        public uint Unknown1;
        public ushort WeatherTransitions;
        public ushort Unknown3;

        [GamePacketField(DynamicSizeReference = "WeatherTransitions")]
        public Weather[] Weathers;

        public byte Size;
        [GamePacketField(DynamicSizeReference = "Size")]
        public string Config;
    }
}
