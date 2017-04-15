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

      object value = field.GetValue(ob);

      return (ulong)Convert.ChangeType(value, typeof(ulong));
    }

    public Type Polymorph(IGamePacketTemplate ob, Type ns) {
      if(PolymorphicReference == null) {
        return null;
      }

      Type T = ob.GetType();
      FieldInfo field = T.GetField(PolymorphicReference);
      if(field == null) {
        throw new FieldNotFoundException(PolymorphicReference, T);
      }

      return GamePacketTemplateFactory.GetInstance().GetSubtype(T, (uint) Convert.ChangeType(field.GetValue(ob), typeof(uint)), ns);
    }
  }
}
