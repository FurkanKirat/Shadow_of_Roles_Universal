using System;
using System.Collections.Generic;
using game.Constants;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
using game.Services.GameServices;
using game.Utils;
using Managers;

namespace game.models.roles.interfaces.abilities
{
    public interface IInvestigativeAbility : IRoleAbility
    {
            /**
         *
         * @param roleOwner
         * @param choosenPlayer
         * @param gameService
         * @return
         */
        AbilityResult DetectiveAbility(Player roleOwner, Player target, BaseGameService gameService){
            
            RoleTemplate randRole = RoleCatalog.GetRandomRole(target.Role.Template);
            bool firstIsChosen = RandomUtils.GetRandomBoolean();
            
            string roleName1 = firstIsChosen ? target.Role.Template.GetName() : randRole.GetName();
            string roleName2 = firstIsChosen ? randRole.GetName() : target.Role.Template.GetName();
            
            var placeholders = new Dictionary<string, string>
            {
                { "roleName1", roleName1 },
                { "roleName2", roleName2 }
            };
            string message = TextManager.FormatMessage( RoleId.Detective, "ability_message", placeholders);

            gameService.MessageService.SendAbilityMessage(message, roleOwner);

            return AbilityResult.Success;
        }

        /**
         *
         * @param roleOwner
         * @param choosenPlayer
         * @param gameService
         * @return
         */
        AbilityResult ObserverAbility(Player roleOwner, Player target, BaseGameService gameService){
            string teamName = TextCategory.Teams.GetEnumTranslation(target.Role.Template.WinningTeam.GetTeam());
            gameService.MessageService.SendAbilityMessage(
                TextManager.GetEnumCategoryTranslation(RoleId.Observer, "ability_message")
                            .Replace("{teamName}", teamName),roleOwner);
            return AbilityResult.Success;
        }

        /**
         *
         * @param roleOwner
         * @param choosenPlayer
         * @param gameService
         * @return
         */
        AbilityResult StalkerAbility(Player roleOwner, Player target, BaseGameService gameService){
            string message;
            if(target.Role.ChosenPlayer == null || !target.Role.CanPerform){
                message = TextManager.GetEnumCategoryTranslation(RoleId.Stalker,"ability_message_nobody");
            }
            else{
                message = TextManager.GetEnumCategoryTranslation(RoleId.Stalker,"ability_message")
                        .Replace("{playerName}", target.Role.ChosenPlayer.Name);
            }

            gameService.MessageService.SendAbilityMessage(message,roleOwner);
            return AbilityResult.Success;
        }

        /**
         *
         * @param roleOwner
         * @param choosenPlayer
         * @param gameService
         * @return
         */
        AbilityResult DarkRevealerAbility(Player roleOwner, Player target, BaseGameService gameService){
            
            string message = TextManager.GetEnumCategoryTranslation(RoleId.DarkRevealer,"ability_message")
                .Replace("{roleName}",target.Role.Template.GetName());
            gameService.MessageService.SendAbilityMessage(message,roleOwner);

            return AbilityResult.Success;
        }


        /**
         *
         * @param roleOwner
         * @param gameService
         * @return
         */
        AbilityResult DarkSeerAbility(Player roleOwner, BaseGameService gameService){
            List<Player> players = gameService.CopyAlivePlayers();
            players.Remove(roleOwner);
            
            players.Shuffle();
            String message;
            const RoleId category = RoleId.DarkSeer;

            if (players.Count >= 2) {
                message = TextManager.GetEnumCategoryTranslation(category,"ability_message")
                        .Replace("{roleName1}",players[0].Role.Template.GetName())
                        .Replace("{roleName2}",players[1].Role.Template.GetName());
            }
            else if (players.Count == 1) {
                message = TextManager.GetEnumCategoryTranslation(category,"ability_message_one_left")
                        .Replace("{roleName}",players[0].Role.Template.GetName());
            }
            else
            {
                message = TextManager.GetEnumCategoryTranslation(category, "ability_message_no_left");
            }

            gameService.MessageService.SendAbilityMessage(message,roleOwner);
            return AbilityResult.Success;
        }
    }
}