using System;
using System.Reflection;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GamePacketAttribute : Attribute {
        public uint Type;
        public bool SubTypes = false;
        public string Name;
        public bool IsUnknown = false;

        public GamePacketAttribute() {
        }
    }
}
