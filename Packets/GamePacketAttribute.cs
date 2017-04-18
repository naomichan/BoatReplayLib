using System;
using System.Reflection;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
  public class GamePacketAttribute : Attribute {
    public uint Type;
    public bool SubTypes = false;
    public string Name;

    public GamePacketAttribute() {
    }

    public uint FindFirstSubtype(IGamePacketTemplate template) {
      Type T = template.GetType();
      if(!T.IsClass || T.GetCustomAttributes(typeof(GamePacketAttribute), false).Length == 0) {
        return 0xFFFFFFFF;
      }

      foreach(FieldInfo info in T.GetFields()) {
        if(!info.IsPublic) {
          continue;
        }
        GamePacketFieldAttribute attrib = info.GetCustomAttribute<GamePacketFieldAttribute>();
        if(attrib == null) {
          continue;
        }

        if(attrib.PolymorphicReference == null) {
          continue;
        }

        FieldInfo field = T.GetField(attrib.PolymorphicReference);
        if(field == null) {
          throw new FieldNotFoundException(attrib.PolymorphicReference, T);
        }
        return (uint)Convert.ChangeType(field.GetValue(template), typeof(uint));
      }
      return 0xFFFFFFFF;
    }
  }
}
