
using System.Collections.Generic;
using game.Constants;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.Services;
using game.Services.GameServices;
using Managers;

namespace game.models.roles.interfaces.abilities
{
    public interface IAttackAbility : IRoleAbility
    {
        AbilityResult Attack(Player roleOwner, Player target, BaseGameService gameService, CauseOfDeath causeOfDeath){
            
            var messageService = gameService.MessageService;
            if (roleOwner.Role.Attack <= target.Role.Defence)
            {
                messageService.SendAbilityMessage(TextCategory.Abilities.GetTranslation("defence"), roleOwner);
                return AbilityResult.AttackInsufficient;
            }
            
            target.KillPlayer(gameService.TimeService.TimePeriod, causeOfDeath);
            
            string killMessage = TextManager.GetEnumCategoryTranslation(causeOfDeath, "kill_message");
            
            string killAnnouncement = TextManager.FormatEnumCategoryMessage(causeOfDeath, "kill_announcement", new Dictionary<string, string>{
                {"playerName", target.GetNameAndNumber()},
                {"roleName", target.Role.Template.GetName()}
                }); 
            
            messageService.SendAbilityMessage(killMessage, roleOwner);
            
            messageService.SendAbilityAnnouncement(killAnnouncement);
            return AbilityResult.Success;
            
        }
    }
}