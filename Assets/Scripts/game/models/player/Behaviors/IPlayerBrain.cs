using System.Collections.Generic;

namespace game.models.player.Behaviors
{
    public interface IPlayerBrain
    {
        public void ChooseNightPlayer(Player self, List<Player> players);
        public void ChooseVotingPlayer(Player self, List<Player> players);
    }
}