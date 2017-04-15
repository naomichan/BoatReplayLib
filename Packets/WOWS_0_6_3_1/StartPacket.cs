using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1 {
  [GamePacket(Type = 0x02, Name = "Start")]
  public class StartPacket : IGamePacketTemplate {
    public uint AvatarId;
    public byte Unknown1;

    public Dictionary<string, object> SpecialValues() {
      return null;
    }
  }
}
