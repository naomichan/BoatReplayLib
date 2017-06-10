using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes {
    [GamePacket(Name = "Message", Type = 0x44)]
    public class ChatMesssge : IGamePacketTemplate, IChatMessage {
        public uint SenderAvatarId;

        public byte ChannelLength;
        [GamePacketField(DynamicSizeReference = "ChannelLength")]
        public string Channel;

        public byte Length;
        [GamePacketField(DynamicSizeReference = "Length")]
        public string Message;

        public string GetMessage() => Message;
    }
}
