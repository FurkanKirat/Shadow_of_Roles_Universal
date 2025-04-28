using game.Constants;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Services;
using game.Services.GameServices;
using Managers;

namespace game.models.roles.interfaces.abilities
{
    public interface IPerformAbility
    {
        

        // Perform the ability by delegating logic to the CanPerformAbility method
        AbilityResult DefaultPerformAbility(Player roleOwner, Player target, BaseGameService gameService)
        {
            var canPerformCheck = AbilityValidator.CanPerformAbility(roleOwner, target, gameService);
            if (canPerformCheck != AbilityResult.Success)
                return canPerformCheck;

            return roleOwner.Role.Template.ExecuteAbility(roleOwner, target, gameService);
        }

        AbilityResult PerformAbilityForPassiveRoles(Player roleOwner, BaseGameService gameService)
        {
            var canPerformCheck = AbilityValidator.CanPerformAbility(roleOwner, null, gameService);
            if (canPerformCheck != AbilityResult.Success)
                return canPerformCheck;

            return roleOwner.Role.Template.ExecuteAbility(roleOwner, null, gameService);
        }

        AbilityResult PerformAbilityForBlockImmuneRoles(Player roleOwner, Player target, BaseGameService gameService)
        {
            var canPerformCheck = AbilityValidator.CanPerformAbility(roleOwner, target, gameService);
            if (canPerformCheck != AbilityResult.Success)
                return canPerformCheck;

            return roleOwner.Role.Template.ExecuteAbility(roleOwner, target, gameService);
        }

        AbilityResult PerformAbilityForImmuneAbilityRoles(Player roleOwner, Player target, BaseGameService gameService)
        {
            var canPerformCheck = AbilityValidator.CanPerformAbility(roleOwner, target, gameService);
            if (canPerformCheck != AbilityResult.Success)
                return canPerformCheck;

            return roleOwner.Role.Template.ExecuteAbility(roleOwner, target, gameService);
        }

        AbilityResult PerformAbilityLoreKeeper(Player roleOwner, Player target, BaseGameService gameService, RoleTemplate targetRole)
        {
            var canPerformCheck = AbilityValidator.CanPerformAbility(roleOwner, target, gameService);
            if (canPerformCheck != AbilityResult.Success)
                return canPerformCheck;

            if (targetRole == null)
                return AbilityResult.NoRoleSelected;

            return roleOwner.Role.Template.ExecuteAbility(roleOwner, target, gameService);
        }

        AbilityResult PerformAbilityForNoAbilityRoles()
        {
            return AbilityResult.NoAbilityExists;
        }
    }
}

