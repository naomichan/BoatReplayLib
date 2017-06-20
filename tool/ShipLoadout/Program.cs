using BoatReplayLib;
using System;
using System.IO;
using Ionic.Zlib;
using System.Linq;
using System.Collections.Generic;
using ReplayXML;
using System.Xml.Linq;

namespace ShipLoadout {
    class MainClass {
        public static void Main(string[] args) {
            if (args.Length == 0) {
                Console.Error.WriteLine("Usage: ShipLoadout GameParams.data");
                return;
            }

            if (!File.Exists(args[0])) {
                Console.Error.WriteLine($"File {args[0]} does not exist");
                return;
            }

            byte[] bytes = File.ReadAllBytes(args[0]).Reverse().ToArray();
            byte[] data = ZlibStream.UncompressBuffer(bytes);

            Dictionary<string, object> GameParams = Unpickler.FlattenPickle(Unpickler.LoadPickle(data) as Dictionary<object, object>);

            Dictionary<long, Dictionary<object, object>> loadouts = new Dictionary<long, Dictionary<object, object>>();
            Dictionary<long, string> names = new Dictionary<long, string>();

            Type LONG = typeof(long);

            foreach(KeyValuePair<string, object> pair in GameParams) {
                Dictionary<string, object> GPData = pair.Value as Dictionary<string, object>;

                Dictionary<string, object> typeinfo = GPData["typeinfo"] as Dictionary<string, object>;
                string type = "Q";
                if (!typeinfo.ContainsKey("type")) {
                    if(pair.Key[2] == 'S') {
                        type = "Ship";
                    }
                } else {
                    type = typeinfo["type"] as string;
                }
                if(type == "Ship") {
                    long shipId = (long)Convert.ChangeType(GPData["id"], LONG);
                    loadouts[shipId] = new Dictionary<object, object>();
                    names[shipId] = GPData["index"] as string;
                    Dictionary<string, object> ShipUpgradeInfo = GPData["ShipUpgradeInfo"] as Dictionary<string, object>;
                    foreach (KeyValuePair<string, object> upgradePair in ShipUpgradeInfo) {
                        if(upgradePair.Value is Dictionary<string, object> upgrade) {
                            string upgradeComponentType = upgrade["ucType"] as string;
                            upgradeComponentType = upgradeComponentType.Substring(1, 1).ToLowerInvariant() + upgradeComponentType.Substring(2);
                            if(upgradeComponentType == "suo") {
                                upgradeComponentType = "fireControl";
                            }
                            Dictionary<string, object> components = upgrade["components"] as Dictionary<string, object>;
                            List<object> componentsN = components[upgradeComponentType] as List<object>;
                            loadouts[shipId][componentsN[0]] = upgradePair.Key;
                        }
                    }
                }
            }

            XElement root = new XElement("Loadouts");

            foreach(KeyValuePair<long, Dictionary<object, object>> pair in loadouts) {
                XElement loadout = new XElement("Loadout", new XAttribute("ShipId", pair.Key), new XAttribute("Index", names[pair.Key]));
                PickledXMLWriter.DictToXML(loadout, pair.Value);
                root.Add(loadout);
            }

            Console.Out.WriteLine(root);

            if (args.Length <= 1) return;

            using (var sw = new StreamWriter(args[2]))
            {
                sw.Write(root);
            } //end using
        }
    }
}
