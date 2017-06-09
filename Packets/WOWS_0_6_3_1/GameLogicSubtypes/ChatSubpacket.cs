using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets;

namespace Packets.WOWS_0_6_3_1.GameLogicSubtypes {
    [GamePacket(Name = "ChatMessage", Type = 0x44)]
    public class ChatSubpacket : IGamePacketTemplate, IChatMessage {
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
