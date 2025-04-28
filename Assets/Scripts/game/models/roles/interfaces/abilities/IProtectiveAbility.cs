using System;
using game.Constants;
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
            gameService.MessageService.SendAbilityMessage(TextCategory.Abilities.GetTranslation("heal"), roleOwner);
            
            target.Role.Defence = Math.Max(1, target.Role.Defence);

            return AbilityResult.Success;
        }
    }
}