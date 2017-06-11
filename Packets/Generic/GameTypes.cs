﻿namespace BoatReplayLib.Packets.Generic {
    public static class GameTypes {
        public enum GameRibbon : byte {
            Torpedo = 1,
            AircraftFrag = 3,
            Incapacitation = 4,
            Frag = 5,
            Fire = 6,
            Flooding = 7,
            Citadel = 8,
            Defended = 9,
            Capture = 10,
            CaptureAssist = 11,
            MainBatteryHit = 12,
            SecondaryHit = 13,
            OverPenetration = 14,
            Penetration = 15,
            UnderPenetration = 16,
            Ricochet = 17
        }
    }
}
