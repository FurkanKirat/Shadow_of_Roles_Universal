using System.Collections.Generic;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.interfaces.abilities
{
    public interface IAttackAbility : IRoleAbility
    {
        AbilityResult Attack(Player roleOwner, Player target, BaseGameService gameService, CauseOfDeath causeOfDeath){
            
            var messageService = gameService.MessageService;
            if (roleOwner.Role.Template.RoleProperties.Attack.Current <= target.Role.Template.RoleProperties.Defence.Current)
            {
                var messageTemplate = new MessageTemplate
                {
                    MessageKey = "abilities.defence"
                };
                messageService.SendPrivateMessage(messageTemplate, roleOwner);
                return AbilityResult.AttackInsufficient;
            }
            
            target.KillPlayer(gameService.TimeService.TimePeriod, causeOfDeath, gameService.GameSettings.GameMode);
            
            var killMessageTemplate = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(causeOfDeath, "kill_message")
            };
            
            var killAnnouncementTemplate = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(causeOfDeath, "kill_announcement"),
                PlaceHolders = new Dictionary<string, string>()
                {
                    { "playerName", target.GetNameAndNumber() },
                    { "roleId", target.Role.Template.RoleID.FormatEnum() }
                }
            };
            
            messageService.SendPrivateMessage(killMessageTemplate, roleOwner);
            messageService.SendPublicMessage(killAnnouncementTemplate);
            
            return AbilityResult.Success;
            
        }
    }
}