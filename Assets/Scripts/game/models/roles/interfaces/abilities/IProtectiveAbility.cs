using System;
using game.models.player;
using Game.Models.Roles.Enums;
using game.Services.GameServices;

namespace game.models.roles.interfaces.abilities
{
    public interface IProtectiveAbility : IRoleAbility
    {
        AbilityResult Heal(Player roleOwner, Player target, BaseGameService gameService)
        {
            var messageTemplate = new MessageTemplate
            {
                MessageKey = "abilities.heal"
            };
            gameService.MessageService.SendPrivateMessage(messageTemplate, roleOwner);
            
            target.Role.Template.RoleProperties.Defence.Current = Math.Max(1, target.Role.Template.RoleProperties.Defence.Current);

            return AbilityResult.Success;
        }
    }
}