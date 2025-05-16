using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.models.roles.interfaces.abilities
{
    public static class AbilityValidator
    {

        public static AbilityResult IsPlayerChosen(Player roleOwner)
        {
            return roleOwner.Role.ChosenPlayer < 1 ? AbilityResult.NoOneSelected : AbilityResult.Success;
        }
        public static AbilityResult CheckBlocked(Player roleOwner, BaseGameService gameService)
        {
            if (!roleOwner.Role.CanPerform && !roleOwner.Role.IsImmune)
            {
                var blockedMessage = new MessageTemplate
                {
                    MessageKey = "role_block.role_blocked_message"
                };
                gameService.MessageService.SendPrivateMessage(blockedMessage, roleOwner);
                return AbilityResult.RoleBlocked;
            }

            return AbilityResult.Success;
        }

        public static AbilityResult CheckAbilityReady(Player roleOwner)
        {
            return roleOwner.Role.Template.RoleProperties.CanUseAbility ? AbilityResult.Success : AbilityResult.AbilityNotReady;
        }
        public static AbilityResult CanPerformAbility(Player roleOwner, Player target, BaseGameService gameService)
        {
            var role = roleOwner.Role.Template;
            // Check if the role is blocked
            var blockedCheck = CheckBlocked(roleOwner, gameService);
            if (blockedCheck != AbilityResult.Success && 
                !role.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune)) return blockedCheck;
            
            // Check if the ability is ready
            var abilityCheck = CheckAbilityReady(roleOwner);
            if (abilityCheck != AbilityResult.Success) return abilityCheck;
            
            var targetCheck = IsPlayerChosen(roleOwner);

            
            if(targetCheck != AbilityResult.Success 
               && role.AbilityType != AbilityType.Passive && role.AbilityType != AbilityType.NoAbility) return targetCheck;
            // Additional checks specific to the target or other conditions
            
            if (target != null && target.Role.IsImmune)
                return AbilityResult.TargetImmune;
            return AbilityResult.Success;
        }
    }
}