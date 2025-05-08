using System;
using game.models.player;
using Game.Models.Roles.Enums;
using game.Services.GameServices;
using Managers;

namespace game.models.roles.interfaces.abilities
{
    public interface IProtectiveAbility : IRoleAbility
    {
        AbilityResult Heal(Player roleOwner, Player target, BaseGameService gameService)
        {
            gameService.MessageService.SendAbilityMessage(TextManager.Translate("abilities.heal"), roleOwner);
            
            target.Role.Template.RoleProperties.Defence.Current = Math.Max(1, target.Role.Template.RoleProperties.Defence.Current);

            return AbilityResult.Success;
        }
    }
}