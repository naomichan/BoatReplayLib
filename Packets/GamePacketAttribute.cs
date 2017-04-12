﻿using System;

namespace BoatReplayLib.Packets {
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class GamePacketAttribute : Attribute {
    public uint Type;
    public Type[] SubTypes;
    public string Name;

    public GamePacketAttribute() {
    }
  }
}
