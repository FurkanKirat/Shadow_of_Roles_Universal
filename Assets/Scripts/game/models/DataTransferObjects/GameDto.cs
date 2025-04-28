using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using game.models.player;
using game.Services;

namespace game.models.DataTransferObjects
{
    [System.Serializable]
    public class GameDto : IDataProvider
    {
        private readonly Dictionary<TimePeriod, List<Message>> _messages;
        private readonly List<Player> _deadPlayers;
        private readonly List<Player> _alivePlayers;
        private readonly TimePeriod _timePeriod;
        private readonly GameStatus _gameStatus;
        private readonly int _playerNumber;
        private readonly GameSettings _gameSettings;
        private readonly TimePeriod _lastCheckTimePeriod;   

        public GameDto(Dictionary<TimePeriod, List<Message>> messages, List<Player> deadPlayers, 
            List<Player> alivePlayers, TimePeriod timePeriod, GameStatus gameStatus, int playerNumber,
            GameSettings gameSettings, TimePeriod lastCheckTimePeriod)
        {
            _messages = messages;
            _deadPlayers = deadPlayers;
            _alivePlayers = alivePlayers;
            _timePeriod = timePeriod;
            _gameStatus = gameStatus;
            _playerNumber = playerNumber;
            _gameSettings = gameSettings;
            _lastCheckTimePeriod = lastCheckTimePeriod;
        }

        public Dictionary<TimePeriod, List<Message>> GetMessages()
        {
            return _messages;
        }

        public TimePeriod GetLastMessagePeriod()
        {
            return _lastCheckTimePeriod;
        }

        public List<Player> GetAlivePlayers()
        {
            return _alivePlayers;
        }

        public List<Player> GetDeadPlayers()
        {
            return _deadPlayers;
        }

        public List<Player> GetAllPlayers()
        {
            return _alivePlayers.Concat(_deadPlayers).OrderBy(player => player.Number).ToList();
        }

        public TimePeriod GetTimePeriod()
        {
            return _timePeriod;
        }

        public Player GetCurrentPlayer()
        {
            var allPlayers = _alivePlayers.Concat(_deadPlayers);
            return allPlayers.FirstOrDefault(player => player.IsSamePlayer(_playerNumber));
        }


        public GameSettings GetGameSettings()
        {
            return _gameSettings;
        }

        public GameStatus GetGameStatus()
        {
            return _gameStatus;
        }
    }
}