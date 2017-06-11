using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace BoatReplayLib.Packets.Generic {
    public class BigWorldPacket : IGamePacketTemplate, IRepresentative, IDisposable {
        private static Type IREPRESENTATITIVE = typeof(IRepresentative);
        public uint Size;
        public uint Type;
        public float Time;
        [GamePacketField(DynamicSizeReference = "Size", PolymorphicReference = "Type")]
        public IGamePacketTemplate Data = null;

        public void Dispose() {
            if (Data != null && typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
                ((IDisposable)Data).Dispose();
            }
        }

        public bool HasSubtypes() {
            return IREPRESENTATITIVE.IsAssignableFrom(Data.GetType());
        }

        public Type GetSubtype() {
            if (!this.HasSubtypes()) {
                return null;
            }

            return (Data as IRepresentative).Represents();
        }

        public Type Represents() {
            return GamePacketTemplateFactory.GetInstance().GetRepresentative(this, "Data");
        }

        public IGamePacketTemplate GetInnerData() => Data;
    }
}
