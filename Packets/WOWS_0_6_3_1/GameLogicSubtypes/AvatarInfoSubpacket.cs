using System.Collections.Generic;
using System.IO;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.GameLogicSubtypes {
  [GamePacket(Name = "AvatarInfo", Type = 0x55)]
  public class AvatarInfoSubpacket : IGamePacketTemplate {
    public uint Unknown1;
    public uint Unknown2;
    public uint Unknown3;
    public ushort Size;
    public ushort Unknown4;
    [GamePacketField(DynamicSizeReference = "Size")]
    public byte[] PickleData;
    public uint Unknown5;
    public ushort Unknown6;

    private static string[] KVTranslation = new string[] {
      "UserId",
      "NetworkAvatarId",
      "ClanTag",
      null,
      "Id",
      "IsAlly",
      null,
      "IsEnemy",
      null,
      null,
      null,
      null,
      "Name",
      "DivisionId",
      null,
      "SquadId",
      null,
      "Loadout",
      "WorldAvatarId",
      "ShipId",
      null,
      null,
      null
    };

    private Dictionary<string, object>[] map = null;
    public Dictionary<string, object>[] ParsePickle() {
      using(MemoryStream ms = new MemoryStream(PickleData)) {
        dynamic data = Depickler.load(ms);
        System.Diagnostics.Debugger.Break();
      }
      return null;
    }
  }
}
