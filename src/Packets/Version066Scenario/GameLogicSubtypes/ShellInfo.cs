using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Interfaces.SuperTemplates;

namespace BoatReplayLib.Packets.Version066Scenario.GameLogicSubtypes {
    [GamePacket(Name = "ShellInfo", Type = 0x29)]
    public class ShellInfo : IGamePacketPostTemplate, IPickled {
        public byte Unknown1;
        public ushort Size;
        public byte Unknown2;
        [GamePacketField(DynamicSizeReference = "Size")]
		public byte[] PickleData;

		private object[] data;

		public object GetPickle() {
			if (PickleData == null) {
				return null;
			}
			if (data == null) {
				ParsePickle();
			}
			return data[0];
		}


		public void ParsePickle() {
            data = Unpickler.load(PickleData);
        }

		public void PostProcessing() {
			ParsePickle();
		}

		public Dictionary<string, object> SpecialValues() {
			if (PickleData == null) {
				return null;
			}
			if (data == null) {
				ParsePickle();
			}
			Dictionary<string, object> d = new Dictionary<string, object>();
			d["Data"] = data;
			return d;
        }
    }
}
