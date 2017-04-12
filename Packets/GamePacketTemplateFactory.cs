using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoatReplayLib.Interfaces;

namespace BoatReplayLib.Packets {
  public class GamePacketTemplateFactory : IDisposable {
    public static GamePacketTemplateFactory INSTANCE = new GamePacketTemplateFactory();

    public GamePacketTemplateFactory() {
      Type T = typeof(IGamePacketNamespace);

      foreach(Type Tns in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => T.IsAssignableFrom(p))) {
        LoadTemplates(Tns);
      }
    }

    public IGamePacketTemplate Read(Stream data, Type template) {
      throw new NotImplementedException();
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
      if(namespaceCache.ContainsKey(version)) {
        return namespaceCache[version];
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

      foreach(Type T in ns.GetPacketTemplates()) {
        GamePacketAttribute attrib = GetGamePacketAttribute(T);
        if(attrib == null) {
          continue;
        }
        templateCache[Tns.FullName][attrib.Type] = T;
      }
    }

    private GamePacketAttribute GetGamePacketAttribute(Type T) {
      object[] attribs = T.GetCustomAttributes(typeof(GamePacketAttribute), true);
      if(attribs.Length == 0) {
        return null;
      }
      return attribs[0] as GamePacketAttribute;
    }

    public Type GetSubtype(Type t, uint v) {
      GamePacketAttribute attrib = GetGamePacketAttribute(t);
      if(attrib == null) {
        return null;
      }
      foreach(Type Ts in attrib.SubTypes) {
        GamePacketAttribute subattrib = GetGamePacketAttribute(Ts);
        if(subattrib.Type == v) {
          return Ts;
        }
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
  }
}
