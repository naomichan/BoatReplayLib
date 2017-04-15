using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets.WOWS_0_6_3_1.GameLogicSubtypes {
  [GamePacket(Name = "AvatarInfo", Type = 0x55)]
  public class AvatarInfoSubpacket : IGamePacketPostTemplate {
    public uint Unknown1;
    public uint Unknown2;
    public ushort Unknown3;
    public ushort Size;
    public ushort Unknown4;
    [GamePacketField(DynamicSizeReference = "Size")]
    public byte[] PickleData;

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
      null,
      "DivisionId",
      null,
      "SquadId",
      "Loadout",
      "WorldAvatarId",
      "ShipId",
      null,
      null,
      null
    };

    private Dictionary<string, object>[] map = null;
    public IReadOnlyDictionary<string, object>[] ParsePickle() {
      if(map != null) {
        return map;
      }
      if(PickleData != null) {
        List<object> pickle = Unpickler.load(PickleData)[0] as List<object>;
        Dictionary<string, object>[] ret = new Dictionary<string, object>[pickle.Count];
        for(int i = 0; i < pickle.Count; ++i) {
          List<object> entry = pickle[i] as List<object>;
          ret[i] = new Dictionary<string, object>();
          for(int j = 0; j < entry.Count; ++j) {
            object[] pair = entry[j] as object[];
            int index = (int) Convert.ChangeType(pair[0], typeof(int));
            string key = $"Unknown{index}";
            if(index < KVTranslation.Length && KVTranslation[index] != null) {
              key = KVTranslation[index];
            }
            ret[i][key] = pair[1];
          }
        }
        map = ret;
        return ret;
      }
      return null;
    }

    public Dictionary<string, object> SpecialValues() {
      if(PickleData == null) {
        return null;
      }
      if(map == null) {
        ParsePickle();
      }
      Dictionary<string, object> d = new Dictionary<string, object>();
      d["Info"] = map;
      return d;
     }

    public void PostProcessing() {
      ParsePickle();
    }
  }
}
