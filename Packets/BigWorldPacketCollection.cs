using System.Collections.Generic;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets {
  public class BigWorldPacketCollection {
    private List<BigWorldPacket> packets = new List<BigWorldPacket>();
    private Dictionary<uint, List<BigWorldPacket>> packetsByType = new Dictionary<uint, List<BigWorldPacket>>();
    private Dictionary<float, List<BigWorldPacket>> packetsByTime = new Dictionary<float, List<BigWorldPacket>>();
    private Dictionary<uint, Dictionary<uint, List<BigWorldPacket>>> packetsBySubtype = new Dictionary<uint, Dictionary<uint, List<BigWorldPacket>>>();
    private SortedSet<float> timeIndex = new SortedSet<float>();

    public int Count => packets.Count;
    public int TypeCount => packetsByType.Keys.Count;
    public int TimeCount => packetsByTime.Keys.Count;

    public List<BigWorldPacket> Packets => packets;
    public Dictionary<uint, List<BigWorldPacket>> ByType => packetsByType;
    public Dictionary<float, List<BigWorldPacket>> ByTime => packetsByTime;
    public Dictionary<uint, Dictionary<uint, List<BigWorldPacket>>> BySubtype => packetsBySubtype;
    public SortedSet<float> TimeIndex => timeIndex;
    
    private bool frozen = false;
    public bool IsReadOnly => !frozen;

    public enum CollectionMode {
      Packets = 0x1,
      Type = 0x2,
      Time = 0x4,
      SubType = 0x8
    }
    
    public static CollectionMode COLLECT_ALL => CollectionMode.Packets | CollectionMode.Type | CollectionMode.Time | CollectionMode.SubType;
    public static CollectionMode COLLECT_FAST => CollectionMode.Packets | CollectionMode.Type | CollectionMode.Time;

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


      if(mode.HasFlag(CollectionMode.SubType)) {
        if(item.HasSubtypes()) {
          if(!packetsBySubtype.ContainsKey(item.Type)) {
            packetsBySubtype[item.Type] = new Dictionary<uint, List<BigWorldPacket>>();
          }
          uint subtype = item.GetSubtype();
          if(!packetsBySubtype[item.Type].ContainsKey(subtype)) {
            packetsBySubtype[item.Type][subtype] = new List<BigWorldPacket>();
          }
          packetsBySubtype[item.Type][subtype].Add(item);
        }
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
      foreach(Dictionary<uint, List<BigWorldPacket>> d in packetsBySubtype.Values) {
        foreach(List<BigWorldPacket> l in d.Values) {
          l.Clear();
        }
        d.Clear();
      }
      packetsBySubtype.Clear();
      timeIndex.Clear();
      frozen = false;
    }

    public bool Contains(BigWorldPacket item) => packets.Contains(item);

    public bool ContainsTime(float time) => timeIndex.Contains(time);

    public bool ContainsType(uint type) => packetsByType.ContainsKey(type);

    public bool ContainsSubtype(uint type, uint subtype) => packetsBySubtype.ContainsKey(type) && packetsBySubtype[type].ContainsKey(subtype);
  }
}
