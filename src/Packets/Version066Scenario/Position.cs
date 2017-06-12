using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.Version066Scenario {
    [GamePacket(Name = "Position", Type = 0x0A)]
    public class Position : IGamePacketTemplate, IPosition {
        public uint WorldAvatarId;
        public uint Unknown1;
        public uint Unknown2;
        public Float3 Location;
        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public Float3 Rotation;
        public byte Unknown6;

        public Float3 GetPosition() => Location;
        public Float3 GetRotation() => Rotation;
    }
}
