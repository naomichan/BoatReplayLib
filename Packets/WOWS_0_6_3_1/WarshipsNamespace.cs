﻿using System;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  public class WarshipsNamespace : IGamePacketNamespace {
    public string GameVersion() {
      return "0, 6, 3";
    }

    private static Type[] templates = new Type[] {
      typeof(GameVersionPacket),
      typeof(GameLogicPacket)
    };

    public Type[] GetPacketTemplates() {
      return templates;
    }
  }
}
