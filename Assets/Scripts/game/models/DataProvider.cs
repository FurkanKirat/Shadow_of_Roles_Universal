using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;

namespace game.models
{
    public interface IDataProvider
    {
        Dictionary<TimePeriod, List<Message>> GetMessages();
        TimePeriod GetLastMessagePeriod();
        List<Player> GetAlivePlayers();
        List<Player> GetDeadPlayers();
        TimePeriod GetTimePeriod();
        Player GetCurrentPlayer();
        GameSettings GetGameSettings();
        List<Player> GetAllPlayers();
        GameStatus GetGameStatus();
    }
}