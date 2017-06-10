using System;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace BoatReplayLib.Packets.Version066Scenario {
    [GamePacket(Name = "GameLogic", Type = 0x08, SubTypes = true)]
    public class GameLogic : IGamePacketTemplate, IDisposable, IRepresentative {
        public uint NetworkAvatarId;
        public uint Subtype;
        public uint Length;
        [GamePacketField(DynamicSizeReference = "Length", PolymorphicReference = "Subtype")]
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
