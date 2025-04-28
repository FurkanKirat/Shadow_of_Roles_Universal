using System.Collections.Generic;
using game.models.player;
using game.Services;

namespace game.models.DataTransferObjects
{
    [System.Serializable]
    public class EndGameDto
    {
        private FinishGameService FinishGameService { get;}
        private List<Player> AllPlayers {get;}

        public EndGameDto(FinishGameService finishGameService, List<Player> allPlayers) {
            FinishGameService = finishGameService;
            AllPlayers = allPlayers;
        }
    }
}

