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
        
        [JsonConstructor]
        public GameSettings(GameMode gameMode, RolePack rolePack, int playerCount)
        {
            GameMode = gameMode;
            RolePack = rolePack;
            PlayerCount = playerCount;
        }
    }
}