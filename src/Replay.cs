﻿using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using System.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using static BoatReplayLib.Packets.GamePacketTemplateFactory;

namespace BoatReplayLib {
    public class Replay : IDisposable {
        private readonly uint magic;
        private readonly uint version;
        private readonly uint jsonSize;
        private readonly string json;
        private readonly ReplayJSON parsedJson;
        private readonly uint uncompressedSize;
        private readonly uint compressedSize;
        private readonly MemoryStream data;

        public uint Magic => magic;
        public uint Version => version;
        public uint JSONSize => jsonSize;
        public string JSON => json;
        public ReplayJSON ParsedJSON => parsedJson;
        public uint UncompressedSize => uncompressedSize;
        public uint CompressedSize => compressedSize;
        public Stream Data => data;

        public static Dictionary<ulong, byte[]> Keys = new Dictionary<ulong, byte[]>() {
            {GenerateGameVersion(0, 1, 0), new byte[] { 0xDE, 0x72, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xB1, 0xDE, 0xFE, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF } },
            {GenerateGameVersion(0, 4, 0), new byte[] { 0x29, 0xB7, 0xC9, 0x09, 0x38, 0x3F, 0x84, 0x88, 0xFA, 0x98, 0xEC, 0x4E, 0x13, 0x19, 0x79, 0xFB } }
        };

        public static byte[] GetClosestKey(short major, short minor, short patch) {
            return GetClosestKey(GenerateGameVersion(major, minor, patch));
        }

        public static byte[] GetClosestKey(ulong version) {
            KeyValuePair<ulong, byte[]> ret = new KeyValuePair<ulong, byte[]>(0, new byte[] { });
            foreach (KeyValuePair<ulong, byte[]> pair in Keys) {
                if (pair.Key > ret.Key && version >= pair.Key) {
                    ret = pair;
                }
            }
            return ret.Value;
        }

        public void Dispose() {
            data.Dispose();
        }

        public static byte[] IV = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public short[] gameVersion = { 0, 0, 0 };
        public short[] GameVersion => gameVersion;

        public ulong GameVersionULong => GenerateGameVersion(GameVersion[0], GameVersion[1], GameVersion[2]);

        public Replay(Stream input, bool leaveOpen = false, short[] overrideVersion = null) {
            using (BinaryReader reader = new BinaryReader(input, System.Text.Encoding.UTF8, leaveOpen)) {
                magic = reader.ReadUInt32();
                version = reader.ReadUInt32();
                jsonSize = reader.ReadUInt32();
                json = new string(reader.ReadChars((int)jsonSize));
                parsedJson = JsonConvert.DeserializeObject<ReplayJSON>(json);
                uncompressedSize = reader.ReadUInt32();
                compressedSize = reader.ReadUInt32();

                if (overrideVersion == null) {
                    short[] versions = parsedJson.clientVersionFromExe.Split(new char[3] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries).Take(3).Select(short.Parse).ToArray();
                    gameVersion = new short[3] { versions[0], versions[1], versions[2] };
                } else {
                    gameVersion = overrideVersion;
                }

                BlowfishEngine bf = new BlowfishEngine();
                BufferedBlockCipher cipher = new BufferedBlockCipher(bf);
                cipher.Init(false, new KeyParameter(GetClosestKey(GameVersionULong)));

                byte[] prev = (byte[])IV.Clone();
                byte[] dataBytes = new byte[compressedSize];
                for (uint i = 0; i < compressedSize; i += 8) {
                    byte[] output = new byte[8];
                    cipher.Reset();
                    byte[] buffer = new byte[8];
                    input.Read(buffer, 0, 8);
                    cipher.ProcessBytes(buffer, output, 0);
                    for (int j = 0; j < output.Length; ++j) {
                        output[j] ^= prev[j];
                        if (i + j < compressedSize) {
                            dataBytes[i + j] = output[j];
                        }
                    }
                    prev = (byte[])output.Clone();
                }

                data = new MemoryStream(ZlibStream.UncompressBuffer(dataBytes)) {
                    Position = 0
                };
            }
        }
    }
}
