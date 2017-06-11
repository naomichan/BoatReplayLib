using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.Version066Scenario {
    [GamePacket(Name = "Disconnect", Type = 0x4)]
    public class Disconnect : IGamePacketTemplate {
        public uint WorldAvatarId;
    }
}
