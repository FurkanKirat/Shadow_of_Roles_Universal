using game.models.player;
using game.Services;
using Networking.DataTransferObjects;

namespace Game.Models.Roles.Enums
{
    public enum AbilityType
    {
        ActiveOthers,
        ActiveSelf,
        ActiveAll,
        OtherThanTeamMembers,
        Passive,
        NoAbility
    }

    public static class AbilityTypeExtensions
    {
        private static bool CanUseAbility(this AbilityType abilityType, int ownerId, RoleId ownerRole, int targetId, RoleId targetRole)
        {
            return abilityType switch
            {
                AbilityType.ActiveOthers => ownerId != targetId,
                AbilityType.ActiveSelf => ownerId == targetId,
                AbilityType.ActiveAll => true,
                AbilityType.OtherThanTeamMembers =>
                    targetRole == RoleId.None || RoleCatalog.GetRole(ownerRole).WinningTeam != RoleCatalog.GetRole(targetRole).WinningTeam,
                _ => false
            };
        }

        public static bool CanUseAbility(this AbilityType abilityType, Player owner, Player target)
        {
           return CanUseAbility(abilityType, owner.Number, owner.Role.Template.RoleID, target.Number, target.Role.Template.RoleID);
        }
        
        public static bool CanUseAbility(this AbilityType abilityType, PlayerDto owner, PlayerDto target)
        {
            return CanUseAbility(abilityType, owner.Number, owner.RoleDto.RoleId, target.Number, target.RoleDto.RoleId);
        }
        
        
    }
}