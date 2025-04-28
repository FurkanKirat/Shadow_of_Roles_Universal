namespace game.models.gamestate
{
    public class GameSettings
    {
        
        public GameMode GameMode { get; set; }
        public RolePack RolePack { get; set; }
        public int PlayerCount { get;}
        
        public GameSettings(GameMode gameMode, RolePack rolePack, int playerCount)
        {
            GameMode = gameMode;
            RolePack = rolePack;
            PlayerCount = playerCount;
        }
    }
}