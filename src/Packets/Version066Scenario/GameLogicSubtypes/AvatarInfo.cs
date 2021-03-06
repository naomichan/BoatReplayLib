﻿using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes {
    [GamePacket(Name = "AvatarInfo", Type = 0x55)]
    public class AvatarInfo : IGamePacketPostTemplate, IAvatarInfo {
        public uint Unknown1;
        public uint Unknown2;
        public ushort Unknown3;
        public ushort Size;
        public byte Unknown4;
        [GamePacketField(DynamicSizeReference = "Size")]
        public byte[] PickleData;

        private static string[] KVTranslation = new string[] {
            "UserId", // 0
            "NetworkAvatarId", // 1
            "ClanTag", // 2
            null, // 3
            "Id", // 4
            null, // 5
            null, // 6
            null, // 7
            null, // 8
            null, // 9
            null, // 10
            null, // 11
            null, // 12
            null, // 13
            null, // 14
            "Health", // 15
            "Name", // 16
            null, // 17
            null, // 18
            null, // 19
            null, // 20
            "Loadout", // 21
            "WorldAvatarId", // 22
            "ShipId",
        };

        public object GetPickle() {
            if (PickleData == null) {
                return null;
            }
            if (map == null) {
                ParsePickle();
            }
            return map[0];
        }

        private Dictionary<string, object>[] map = null;
        public IReadOnlyDictionary<string, object>[] ParsePickle() {
            if (map != null) {
                return map;
            }
            if (PickleData != null) {
                List<object> pickle = Unpickler.LoadPickle(PickleData) as List<object>;
                Dictionary<string, object>[] ret = new Dictionary<string, object>[pickle.Count];
                for (int i = 0; i < pickle.Count; ++i) {
                    List<object> entry = pickle[i] as List<object>;
                    ret[i] = new Dictionary<string, object>();
                    for (int j = 0; j < entry.Count; ++j) {
                        List<object> pair = entry[j] as List<object>;
                        int index = (int)Convert.ChangeType(pair[0], typeof(int));
                        string key = $"Unknown{index}";
                        if (index < KVTranslation.Length && KVTranslation[index] != null) {
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
            if (PickleData == null) {
                return null;
            }
            if (map == null) {
                ParsePickle();
            }
            Dictionary<string, object> d = new Dictionary<string, object>();
            d["Info"] = map;
            return d;
        }

        public void PostProcessing() {
            ParsePickle();
        }

        public IReadOnlyDictionary<string, object>[] GetAvatarInfo() {
            return ParsePickle();
        }
    }
}
