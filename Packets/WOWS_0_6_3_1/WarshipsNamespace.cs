using System;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  public class WarshipsNamespace : IGamePacketNamespace {
    public string GameVersion() {
      return "0, 6, 3, 1";
    }

    private static Type[] templates = new Type[] {
      typeof(GameVersionPacket)
    };

    public Type[] GetPacketTemplates() {
      return templates;
    }
  }
}
