using game.Constants;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces;
using game.Services.GameServices;
using Managers;

namespace game.Services
{
    public class AbilityService
    {
        public readonly int LoreKeLoreKeeperWinningCount;
        private readonly BaseGameService _gameService;

        public AbilityService(BaseGameService gameService) {
            _gameService = gameService;
            int playerCount = gameService.AllPlayers.Count;
            LoreKeLoreKeeperWinningCount = playerCount >= 8 ? 3 : 2;
        }


        /**
         * It sends a message about who is player using their ability on.
         */
        private void SendChosenPlayerMessages(Player player){

            Player chosenPlayer = player.Role.ChosenPlayer;

            AbilityType abilityType = player.Role.Template.AbilityType;
            const TextCategory category = TextCategory.Abilities;
            
            if (abilityType == AbilityType.Passive 
                || abilityType == AbilityType.NoAbility 
                || player.PlayerType != PlayerType.Human)
            {
                return;
            }

            string message = chosenPlayer != null
                ? category.GetTranslation("used_on")
                    .Replace("{playerName}", chosenPlayer.GetNameAndNumber())
                : category.GetTranslation("did_not_used");

            _gameService.MessageService.SendAbilityMessage(message, player);
        }

        /**
         *  Performs all abilities according to role priorities
         */
        public void PerformAllAbilities()
        {
            var players = _gameService.CopyAlivePlayers();
            _gameService.ChooseRandomPlayersForAI(players);

            // If the roles priority changes in each turn changes the priority
            foreach (var player in players)
            {
                if (player.Role.Template is IPriorityChangingRole priorityChangingRole)
                {
                    priorityChangingRole.ChangePriority(_gameService.GameSettings.RolePack);
                }

                SendChosenPlayerMessages(player);
            }

            // Sorts the roles according to priority and if priorities are same sorts
            players.Sort((player1, player2) =>
            {
                int priorityComparison = player2.Role.Template.RolePriority.GetPriority()
                    .CompareTo(player1.Role.Template.RolePriority.GetPriority());
                if (priorityComparison != 0)
                    return priorityComparison;

                return player1.Role.Template.RoleID.CompareTo(player2.Role.Template.RoleID);
            });

            // Performs the abilities in the sorted list
            foreach (var player in players)
            {
                player.Role.Template.PerformAbility(
                    player, player.Role.ChosenPlayer, _gameService);
            }

            // Send some messages to some roles
            foreach (var player in players)
            {
                _gameService.MessageService.SendSpecificRoleMessages(player);
            }

            // Resets the players' attributes according to their role
            foreach (var player in _gameService.AlivePlayers)
            {
                player.Role.ResetStates();
            }

            _gameService.UpdateAlivePlayers();
        }
    }
}

