namespace Game.Models.Roles.Enums
{
    public enum RoleCategory
    {
        FolkAnalyst = 0,
        FolkProtector = 1,
        FolkKilling = 2,
        FolkSupport = 3,
        FolkUnique = 4,
        FolkVillager = 5,
        
        CorrupterAnalyst = 6,
        CorrupterKilling = 7,
        CorrupterSupport = 8,
        
        NeutralEvil = 9,
        NeutralKilling = 10,
        NeutralChaos = 11,
        NeutralGood = 12,
        
    }

    public static class RoleCategoryExtensions
    {
        public static Team GetTeam(this RoleCategory category)
        {
            switch (category)
            {
                case RoleCategory.FolkAnalyst:
                case RoleCategory.FolkProtector:
                case RoleCategory.FolkKilling:
                case RoleCategory.FolkSupport:
                case RoleCategory.FolkUnique:
                case RoleCategory.FolkVillager:
                    return Team.Folk;
                
                case RoleCategory.CorrupterAnalyst:
                case RoleCategory.CorrupterKilling:
                case RoleCategory.CorrupterSupport:
                    return Team.Corrupter;
                
                case RoleCategory.NeutralEvil:
                case RoleCategory.NeutralKilling:
                case RoleCategory.NeutralChaos:
                case RoleCategory.NeutralGood:
                default:
                        return Team.Neutral;
            }
        }
    }
}