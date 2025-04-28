using game.models.player;

namespace Game.Models.Roles.Enums
{
    public enum AbilityType
    {
        ActiveOthers,
        ActiveSelf,
        ActiveAll,
        OtherThanCorrupted,
        Passive,
        NoAbility
    }

    public static class AbilityTypeExtensions
    {
        public static bool CanUseAbility(this AbilityType abilityType, Player roleOwner, Player target)
        {

            return abilityType switch
            {
                AbilityType.ActiveOthers => !roleOwner.IsSamePlayer(target),
                AbilityType.ActiveSelf => roleOwner.IsSamePlayer(target),
                AbilityType.ActiveAll => true,
                AbilityType.OtherThanCorrupted => target.Role.Template.WinningTeam.GetTeam() != Team.Corrupter,
                _ => false
            };
        }
        
        
    }
}