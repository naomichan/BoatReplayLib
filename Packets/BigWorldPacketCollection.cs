using System.Collections.Generic;
using BoatReplayLib.Packets.Generic;

namespace BoatReplayLib.Packets {
  public class BigWorldPacketCollection {
    private List<BigWorldPacket> packets = new List<BigWorldPacket>();
    private Dictionary<uint, List<BigWorldPacket>> packetsByType = new Dictionary<uint, List<BigWorldPacket>>();
    private Dictionary<float, List<BigWorldPacket>> packetsByTime = new Dictionary<float, List<BigWorldPacket>>();
    private SortedSet<float> timeIndex = new SortedSet<float>();

    public int Count => packets.Count;
    public int TypeCount => packetsByType.Keys.Count;
    public int TimeCount => packetsByTime.Keys.Count;

    public List<BigWorldPacket> Packets => packets;
    public Dictionary<uint, List<BigWorldPacket>> ByType => packetsByType;
    public Dictionary<float, List<BigWorldPacket>> ByTime => packetsByTime;
    public SortedSet<float> TimeIndex => timeIndex;
    
    private bool frozen = false;
    public bool IsReadOnly => !frozen;

    public void Freeze() {
      frozen = true;
    }

    public void Add(BigWorldPacket item) {
      if(frozen) {
        return;
      }
      packets.Add(item);

      if(!packetsByType.ContainsKey(item.Type)) {
        packetsByType[item.Type] = new List<BigWorldPacket>();
      }
      packetsByType[item.Type].Add(item);

      if(!packetsByTime.ContainsKey(item.Time)) {
        packetsByTime[item.Time] = new List<BigWorldPacket>();
        timeIndex.Add(item.Time);
      }
      packetsByTime[item.Time].Add(item);
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
      timeIndex.Clear();
      frozen = false;
    }

    public bool Contains(BigWorldPacket item) => packets.Contains(item);

    public bool ContainsTime(float time) => timeIndex.Contains(time);

    public bool ContainsType(uint type) => packetsByType.ContainsKey(type);
  }
}
