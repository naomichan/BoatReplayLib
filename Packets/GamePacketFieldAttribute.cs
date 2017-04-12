using System;
using System.Reflection;
using System.Text;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class GamePacketFieldAttribute : Attribute {
    public bool Ignore = false;
    public string DynamicSizeReference = null;
    public string PolymorphicReference = null;
    public ulong Size = 0;
    public string Encoding = "ascii";

    public GamePacketFieldAttribute() {
    }

    public Encoding GetEncoding() {
      return System.Text.Encoding.GetEncoding(Encoding);
    }

    public ulong RefSize(IGamePacketTemplate ob) {
      if(DynamicSizeReference == null && Size > 0) {
        return Size;
      }

      Type T = ob.GetType();
      FieldInfo field = T.GetField(DynamicSizeReference);
      if(field == null) {
        if(Size > 0) {
          return Size;
        }
        throw new FieldNotFoundException(DynamicSizeReference, T);
      }

      return (ulong) field.GetValue(ob);
    }

    public Type Polymorph(IGamePacketTemplate ob) {
      if(PolymorphicReference == null) {
        return null;
      }

      Type T = ob.GetType();
      FieldInfo field = T.GetField(PolymorphicReference);
      if(field == null) {
        throw new FieldNotFoundException(PolymorphicReference, T);
      }

      return GamePacketTemplateFactory.INSTANCE.GetSubtype(T, (uint) field.GetValue(ob));
    }
  }
}
