using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BoatReplayLib.Interfaces;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets {
  public class GamePacketTemplateFactory : IDisposable {
    private static GamePacketTemplateFactory __INSTANCE = null;
    private static Type MEMORYTEMPLATE = typeof(MemoryPacket);
    private static Type BIGWORLDPACKET = typeof(BigWorldPacket);
    private static Type GAMEPACKETTEMPLATE = typeof(IGamePacketTemplate);
    private static Type GAMEPACKETPPTEMPLATE = typeof(IGamePacketPostTemplate);
    private static Type GAMEPACKETNS = typeof(IGamePacketNamespace);
    private static Type GAMEPACKETATTRIB = typeof(GamePacketAttribute);
    private static Type GAMEPACKETFIELDATTRIB = typeof(GamePacketFieldAttribute);

    private static Dictionary<string, Type[]> reflectTemplateCache = new Dictionary<string, Type[]>();

    public GamePacketTemplateFactory() {
      Type T = GAMEPACKETNS;
      
      foreach(Type Tns in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => T.IsAssignableFrom(p) && !p.IsInterface)) {
        LoadTemplates(Tns);
      }
    }

    public static Type[] GetTemplates(string ns) {
      if(reflectTemplateCache.ContainsKey(ns)) {
        return reflectTemplateCache[ns];
      }

      List<Type> templates = new List<Type>();
      foreach(Type T in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => GAMEPACKETTEMPLATE.IsAssignableFrom(p) && p.Namespace == ns && !p.IsInterface)) {
        templates.Add(T);
      }

      reflectTemplateCache[ns] = templates.ToArray();

      return reflectTemplateCache[ns];
    }

    public BigWorldPacketCollection ReadAll(Stream data, Type ns) {
      BigWorldPacketCollection packets = new BigWorldPacketCollection();
      while(data.Position < data.Length) {
        packets.Add(Read(data, ns) as BigWorldPacket);
      }
      packets.Freeze();
      return packets;
    }
    
    public IGamePacketTemplate Read(Stream data, Type ns) {
      return Read(data, ns, BIGWORLDPACKET);
    }

    public IGamePacketTemplate Read(Stream data, Type ns, Type template) {
      if(!GAMEPACKETTEMPLATE.IsAssignableFrom(template) || !GAMEPACKETNS.IsAssignableFrom(ns)) {
        return null;
      }
      IGamePacketTemplate instance = Activator.CreateInstance(template) as IGamePacketTemplate;
      using(BinaryReader reader = new BinaryReader(data, Encoding.Default, true)) {
        FieldInfo[] fields = template.GetFields();
        foreach(FieldInfo field in fields) {
          if(!field.IsPublic) {
            continue;
          }
          object value = null;
          GamePacketFieldAttribute attrib = GetGamePacketFieldAttribute(field);
          bool isArray = field.FieldType.IsArray;
          if(isArray) {
            value = ReadGenericArray(reader, attrib, instance, field, field.FieldType, ns);
          } else {
            value = ReadGeneric(reader, attrib, instance, field, field.FieldType, ns);
          }
          field.SetValue(instance, value);
        }
      }
      if(GAMEPACKETPPTEMPLATE.IsAssignableFrom(template)) {
        (instance as IGamePacketPostTemplate).PostProcessing();
      }
      return instance;
    }

    private object ReadGeneric(BinaryReader reader, GamePacketFieldAttribute attrib, IGamePacketTemplate instance, FieldInfo field, Type fieldType, Type ns) {
      if(attrib != null && attrib.Ignore == true) {
        return null;
      }
      ulong size = 0;
      Encoding encoding = Encoding.Default;
      if(attrib == null) {
        size = (ulong)(reader.BaseStream.Length - reader.BaseStream.Position);
      } else {
        size = attrib.RefSize(instance);
        encoding = attrib.GetEncoding();
      }

      switch(fieldType.Name) {
        case "String":
          byte[] bytes = reader.ReadBytes((int)size);
          return encoding.GetString(bytes);
        case "Single":
          return reader.ReadSingle();
        case "Boolean":
          return reader.ReadByte() != 0;
        case "Int16":
          return reader.ReadInt16();
        case "UInt16":
          return reader.ReadUInt16();
        case "Int32":
          return reader.ReadInt32();
        case "UInt32":
          return reader.ReadUInt32();
        case "Int64":
          return reader.ReadInt64();
        case "UInt64":
          return reader.ReadUInt64();
        case "Byte":
          return reader.ReadByte();
        case "SByte":
          return reader.ReadSByte();
        case "MemoryStream":
          return new MemoryStream(reader.ReadBytes((int)size));
        case "IGamePacketTemplate": {
            MemoryStream sandbox = new MemoryStream(reader.ReadBytes((int)size));
            Type template = null;
            if(attrib != null) {
              template = attrib.Polymorph(instance, ns);
            }
            if(template == null) {
              template = MEMORYTEMPLATE;
            }
            return Read(sandbox, ns, template);
          }
        default:
          if(GAMEPACKETTEMPLATE.IsAssignableFrom(fieldType) && fieldType.IsInterface) {
            MemoryStream sandbox = new MemoryStream(reader.ReadBytes((int)size));
            Type template = null;
            if(attrib != null) {
              template = attrib.Polymorph(instance, ns);
            }
            if(template == null) {
              template = MEMORYTEMPLATE;
            }
            return Read(sandbox, ns, template);
          } else if(GAMEPACKETTEMPLATE.IsAssignableFrom(fieldType) && !fieldType.IsInterface) {
            return Read(reader.BaseStream, ns, fieldType);
          }
          Console.Error.WriteLine($"Warning: No read method defined for {fieldType.Name}");
          break;
      }
      return null;
    }

    private object ReadGenericArray(BinaryReader reader, GamePacketFieldAttribute attrib, IGamePacketTemplate instance, FieldInfo field, Type fieldType, Type ns) {
      if(attrib != null && attrib.Ignore == true) {
        return null;
      }
      ulong size = 0;
      Encoding encoding = Encoding.Default;
      if(attrib == null) {
        if(fieldType.GetElementType().Name == "Byte") {
          size = (ulong)(reader.BaseStream.Length - reader.BaseStream.Position);
        } else {
          throw new Exception($"Array values must have GamePacketFieldAttribute at {field.DeclaringType.FullName}.{field.Name}");
        }
      } else {
        size = attrib.RefSize(instance);
        encoding = attrib.GetEncoding();
      }
      object[] ret = new object[size];

      switch(fieldType.GetElementType().Name) {
        case "Single":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadSingle();
          break;
        case "Boolean":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadByte() != 0;
          break;
        case "Int16":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadInt16();
          break;
        case "UInt16":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadUInt16();
          break;
        case "Int32":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadInt32();
          break;
        case "UInt32":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadUInt32();
          break;
        case "Int64":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadInt64();
          break;
        case "UInt64":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadUInt64();
          break;
        case "Byte":
          return reader.ReadBytes((int)size);
        case "SByte":
          for(ulong i = 0; i < size; ++i) ret[i] = reader.ReadSByte();
          break;
        default:
          Console.Error.WriteLine($"Warning: No read method defined for {fieldType.Name}");
          return null;
      }
      return ret;
    }

    private Dictionary<string, Dictionary<uint, Type>> templateCache = new Dictionary<string, Dictionary<uint, Type>>();
    private Dictionary<string, Type> namespaceCache = new Dictionary<string, Type>();

    public Type GetTemplate(Type ns, uint type) {
      if(!templateCache.ContainsKey(ns.FullName)) {
        LoadTemplates(ns);
      }

      if(templateCache[ns.FullName].ContainsKey(type)) {
        return templateCache[ns.FullName][type];
      }

      return null;
    }

    public Type GetNamespace(string version) {
      string fixedVersion = string.Join(".", version.Split(new char[3] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries));
      foreach(KeyValuePair<string, Type> pair in namespaceCache) { 
        if(pair.Key.StartsWith(fixedVersion) || fixedVersion.StartsWith(pair.Key)) {
          return pair.Value;
        }
      }
      return null;
    }

    private void LoadTemplates(Type Tns) {
      IGamePacketNamespace ns = Activator.CreateInstance(Tns) as IGamePacketNamespace;
      if(ns == null) {
        return;
      }

      namespaceCache[ns.GameVersion()] = Tns;
      templateCache[Tns.FullName] = new Dictionary<uint, Type>();

      foreach(Type T in GetTemplates(Tns.Namespace)) {
        GamePacketAttribute attrib = GetGamePacketAttribute(T);
        if(attrib == null) {
          continue;
        }
        templateCache[Tns.FullName][attrib.Type] = T;
      }
    }

    private GamePacketAttribute GetGamePacketAttribute(Type T) {
      object[] attribs = T.GetCustomAttributes(GAMEPACKETATTRIB, true);
      if(attribs.Length == 0) {
        return null;
      }
      return attribs[0] as GamePacketAttribute;
    }

    private GamePacketFieldAttribute GetGamePacketFieldAttribute(FieldInfo T) {
      object[] attribs = T.GetCustomAttributes(GAMEPACKETFIELDATTRIB, true);
      if(attribs.Length == 0) {
        return null;
      }
      return attribs[0] as GamePacketFieldAttribute;
    }

    public Type GetSubtype(Type t, uint v, Type ns) {
      GamePacketAttribute attrib = GetGamePacketAttribute(t);
      if(attrib != null && attrib.SubTypes == true) {
        Type[] subtypes = GetTemplates($"{t.Namespace}.{attrib.Name}Subtypes");
        foreach(Type Ts in subtypes) {
          GamePacketAttribute subattrib = GetGamePacketAttribute(Ts);
          if(subattrib.Type == v) {
            return Ts;
          }
        }
      } else {
        return GetTemplate(ns, v);
      }
      return null;
    }

    public void Dispose() {
      foreach(Dictionary<uint, Type> d in templateCache.Values) {
        d.Clear();
      }
      templateCache.Clear();
      namespaceCache.Clear();
    }

    public static GamePacketTemplateFactory GetInstance() {
      if(__INSTANCE == null) {
        __INSTANCE = new GamePacketTemplateFactory();
      }
      return __INSTANCE;
    }
  }
}
