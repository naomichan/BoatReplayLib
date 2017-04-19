using System;
using System.Collections.Generic;
using BoatReplayLib.Interfaces.SuperTemplates;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets {
  public class BigWorldPacketCollection {
    private List<BigWorldPacket> packets = new List<BigWorldPacket>();
    private Dictionary<uint, List<BigWorldPacket>> packetsByType = new Dictionary<uint, List<BigWorldPacket>>();
    private Dictionary<float, List<BigWorldPacket>> packetsByTime = new Dictionary<float, List<BigWorldPacket>>();
    private Dictionary<string, List<BigWorldPacket>> packetsByName = new Dictionary<string, List<BigWorldPacket>>();
    private SortedSet<float> timeIndex = new SortedSet<float>();

    public int Count => packets.Count;
    public int TypeCount => packetsByType.Keys.Count;
    public int TimeCount => packetsByTime.Keys.Count;

    public List<BigWorldPacket> Packets => packets;
    public Dictionary<uint, List<BigWorldPacket>> ByType => packetsByType;
    public Dictionary<float, List<BigWorldPacket>> ByTime => packetsByTime;
    public Dictionary<string, List<BigWorldPacket>> ByName => packetsByName;
    public SortedSet<float> TimeIndex => timeIndex;

    private bool frozen = false;
    public bool IsReadOnly => !frozen;

    [Flags]
    public enum CollectionMode {
      Packets = 0x1,
      Type = 0x2,
      Time = 0x4,
      Name = 0x8
    }

    public static CollectionMode COLLECT_ALL => CollectionMode.Packets | CollectionMode.Type | CollectionMode.Time | CollectionMode.Name;

    private CollectionMode mode;
    public CollectionMode Mode => mode;

    public BigWorldPacketCollection(CollectionMode mode) {
      this.mode = mode;
    }

    public void Freeze() {
      frozen = true;
    }

    public void Add(BigWorldPacket item) {
      if(frozen) {
        return;
      }
      if(mode.HasFlag(CollectionMode.Packets)) {
        packets.Add(item);
      }

      if(mode.HasFlag(CollectionMode.Type)) {
        if(!packetsByType.ContainsKey(item.Type)) {
          packetsByType[item.Type] = new List<BigWorldPacket>();
        }
        packetsByType[item.Type].Add(item);
      }

      if(mode.HasFlag(CollectionMode.Time)) {
        if(!packetsByTime.ContainsKey(item.Time)) {
          packetsByTime[item.Time] = new List<BigWorldPacket>();
          timeIndex.Add(item.Time);
        }
        packetsByTime[item.Time].Add(item);
      }

      if(mode.HasFlag(CollectionMode.Name)) {
        Type t = item.Represents();

        string name = GamePacketTemplateFactory.GetInstance().GetName(t);
        if(!packetsByName.ContainsKey(name)) {
          packetsByName[name] = new List<BigWorldPacket>();
        }
        packetsByName[name].Add(item);
      }
    }

    public void Clear() {
      packets.Clear();
      foreach(List<BigWorldPacket> l in packetsByType.Values) {
        l.Clear();
      }
      packetsByType.Clear();
      foreach(List<BigWorldPacket> l in packetsByTime.Values) {
        l.Clear();
      }
      packetsByTime.Clear();
      foreach(List<BigWorldPacket> l in packetsByName.Values) {
        l.Clear();
      }
      packetsByName.Clear();
      timeIndex.Clear();
      frozen = false;
    }

    public bool Contains(BigWorldPacket item) => packets.Contains(item);

    public bool ContainsTime(float time) => timeIndex.Contains(time);

    public bool ContainsType(uint type) => packetsByType.ContainsKey(type);

    public bool ContainsName(string name) => packetsByName.ContainsKey(name);

    public List<BigWorldPacket> Get<T>() where T : IBlankSuperTemplate {
      List<BigWorldPacket> ret = new List<BigWorldPacket>();
      foreach(BigWorldPacket packet in packets) {
        if(typeof(T).IsAssignableFrom(packet.Represents())) {
          ret.Add(packet);
        }
      }
      return ret;
    }
    public BigWorldPacket GetFirst<T>() where T : IBlankSuperTemplate {
      foreach(BigWorldPacket packet in packets) {
        if(typeof(T).IsAssignableFrom(packet.Represents())) {
          return packet;
        }
      }
      return null;
    }
  }
}
