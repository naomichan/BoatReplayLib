using System;
using System.IO;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  class BigWorldPacket : IDisposable {
    public uint Size = 0;
    public uint Type = 0;
    public float Time = 0;
    public IGamePacketTemplate Data = null;
    public MemoryStream RawData = null;

    public void Mutate(Type ns) {
      if(RawData == null || Size == 0) {
        return;
      }
      Type template = GamePacketTemplateFactory.INSTANCE.GetTemplate(ns, Type);
      if(template == null) {
        return;
      }
      Data = GamePacketTemplateFactory.INSTANCE.Read(RawData, template);
    }

    public static BigWorldPacket Read(Stream input, bool autoMutate = true, bool keepOpen = true) {
      // TODO: Implement this.
      return null;
    }

    public void Dispose() {
      RawData.Dispose();
      if(typeof(IDisposable).IsAssignableFrom(Data.GetType())) {
        ((IDisposable) Data).Dispose();
      }
    }
  }
}
