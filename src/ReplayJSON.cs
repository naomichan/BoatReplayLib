using System.Collections.Generic;

namespace BoatReplayLib {
    public class ReplayJSON {
        public string clientVersionFromXml;
        public int gameMode;
        public string clientVersionFromExe;
        public string mapDisplayName;
        public int mapId;
        public string matchGroup;
        public int duration;
        public string gameLogic;
        public string name;
        public string scenario;
        public int playerID;
        public IList<ReplayJSONVehicle> vehicles;
        public int playersPerTeam;
        public string dateTime;
        public string mapName;
        public string playerName;
        public int scenarioConfigId;
        public int teamsCount;
        public string logic;
        public string playerVehicle;
    }

    public class ReplayJSONVehicle {
        public long shipId;
        public int relation;
        public int id;
        public string name;
    }
}
