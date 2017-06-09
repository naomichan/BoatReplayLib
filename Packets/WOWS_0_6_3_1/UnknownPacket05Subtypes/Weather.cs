using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.UnknownPacket05Subtypes {
    public class Weather : IGamePacketTemplate {
        public WeatherParameter Parameter1;
        public WeatherParameter Parameter2;
        public WeatherParameter Parameter3;
        public WeatherParameter Parameter4;
        public WeatherParameter Parameter5;
    }
}
