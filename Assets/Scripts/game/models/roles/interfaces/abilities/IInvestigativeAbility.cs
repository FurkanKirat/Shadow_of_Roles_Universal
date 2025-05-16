using System.Collections.Generic;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
using game.Services.GameServices;
using game.Utils;

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

            var template = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleId.Detective, "ability_message"),
                PlaceHolders = new Dictionary<string, string>()
                {
                    { "role1Id", roleName1 },
                    { "role2Id", roleName2 },
                }
            };
                    
            gameService.MessageService.SendPrivateMessage(template, roleOwner);

            return AbilityResult.Success;
        }

        /**
         *
         * @param roleOwner
         * @param choosenPlayer
         * @param gameService
         * @return
         */
        AbilityResult ObserverAbility(Player roleOwner, Player target, BaseGameService gameService)
        {
            var messageTemplate = new MessageTemplate
            {
                MessageKey = $"{RoleId.Observer.FormatEnum()}.ability_message",
                PlaceHolders = new Dictionary<string, string>
                {
                    { "teamId", target.Role.Template.WinningTeam.GetTeam().ToString().ToLower() }
                }
            };
            
            gameService.MessageService.SendPrivateMessage(messageTemplate, roleOwner);
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
            Player targetsTarget = gameService.GetPlayer(target.Role.ChosenPlayer);

            MessageTemplate messageTemplate;
            
            if(targetsTarget == null || !target.Role.CanPerform)
            {
                messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(RoleId.Stalker, "ability_message_nobody")
                };
            }
            else{
                
                messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(RoleId.Stalker, "ability_message"),
                    PlaceHolders = new Dictionary<string, string>()
                    {
                        { "playerName", targetsTarget.GetNameAndNumber() }
                    }
                };
            }

            gameService.MessageService.SendPrivateMessage(messageTemplate,roleOwner);
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
            
            var messageTemplate = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleId.DarkRevealer, "ability_message"),
                PlaceHolders = new Dictionary<string, string>
                {
                    {"roleId",target.Role.Template.RoleID.FormatEnum()}
                }
            };
            gameService.MessageService.SendPrivateMessage(messageTemplate,roleOwner);

            return AbilityResult.Success;
        }


        /**
         *
         * @param roleOwner
         * @param gameService
         * @return
         */
        AbilityResult DarkSeerAbility(Player roleOwner, BaseGameService gameService){
            List<Player> players = gameService.GetAlivePlayersAsPlayerList();
            players.Remove(roleOwner);
            
            players.Shuffle();
            MessageTemplate messageTemplate;
            const RoleId roleId = RoleId.DarkSeer;

            if (players.Count >= 2)
            {
                messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(roleId, "ability_message"),
                    PlaceHolders = new Dictionary<string, string>()
                    {
                        { "role1Id", players[0].Role.Template.RoleID.FormatEnum() },
                        { "role2Id", players[1].Role.Template.RoleID.FormatEnum() }
                    }
                };
            }
            else if (players.Count == 1)
            {
                messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(roleId, "ability_message_one_left"),
                    PlaceHolders = new Dictionary<string, string>()
                    {
                        { "role1Id", players[0].Role.Template.RoleID.FormatEnum() },
                    }
                };
            }
            else
            {
                messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(roleId, "ability_message_no_left")
                };
            }

            gameService.MessageService.SendPrivateMessage(messageTemplate, roleOwner);
            return AbilityResult.Success;
        }
    }
}