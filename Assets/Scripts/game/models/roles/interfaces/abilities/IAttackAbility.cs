using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.Services.GameServices;
using game.Utils;
using Managers;

namespace game.models.roles.interfaces.abilities
{
    public interface IAttackAbility : IRoleAbility
    {
        AbilityResult Attack(Player roleOwner, Player target, BaseGameService gameService, CauseOfDeath causeOfDeath){
            
            var messageService = gameService.MessageService;
            if (roleOwner.Role.Template.RoleProperties.Attack.Current <= target.Role.Template.RoleProperties.Defence.Current)
            {
                messageService.SendAbilityMessage(TextManager.Translate("abilities.defence"), roleOwner);
                return AbilityResult.AttackInsufficient;
            }
            
            target.KillPlayer(gameService.TimeService.TimePeriod, causeOfDeath, gameService.GameSettings.GameMode);
            
            string killMessage = TextManager.TranslateEnum(causeOfDeath,"kill_message");

            string killAnnouncement = TextManager.TranslateEnum(causeOfDeath, "kill_announcement")
                .Replace("playerName", target.GetNameAndNumber()
                    .Replace("roleName", target.Role.Template.GetName())
                );
            messageService.SendAbilityMessage(killMessage, roleOwner);
            
            messageService.SendAbilityAnnouncement(killAnnouncement);
            return AbilityResult.Success;
            
        }
    }
}