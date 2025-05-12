using System;
using Newtonsoft.Json;

namespace game.models.gamestate
{
    [Serializable]
    public class GameSettings
    {
        public GameMode GameMode { get; set; }
        public RolePack RolePack { get; set; }
        public int PlayerCount { get;}
        public int PhaseTime { get; }
        
        [JsonConstructor]
        public GameSettings(GameMode gameMode, RolePack rolePack, int playerCount, int phaseTime = 30)
        {
            GameMode = gameMode;
            RolePack = rolePack;
            PlayerCount = playerCount;
            PhaseTime = phaseTime;
        }
    }
}