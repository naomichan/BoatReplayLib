using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
    [GamePacket(Type = 0x05, Name = "UnknownPacket05", SubTypes = true)]
    public class Packet05 : IGamePacketTemplate, IDisposable, IRepresentative {
        public uint NetworkAvatarId;
        public ushort Subtype;
        public uint Unknown1;
        public uint Unknown2;
        public Float4 Unknown3;
        public uint Unknown4;
        public uint Unknown5;
        public uint Size;
        [GamePacketField(DynamicSizeReference = "Size", PolymorphicReference = "Subtype")]
        public IGamePacketTemplate Data;


        public void Dispose() {
            if (Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
                ((IDisposable)Data).Dispose();
            }
        }

        public Type Represents() => GamePacketTemplateFactory.GetInstance().GetRepresentative(this, "Data");

        public IGamePacketTemplate GetInnerData() => Data;
    }
}
