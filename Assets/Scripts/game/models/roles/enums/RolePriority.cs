namespace Game.Models.Roles.Enums
{
    public enum RolePriority
    {
        None = 0,
        Heal = 1,
        Immune = 2,
        Blinder = 3,
        RoleBlock = 4,
        LoreKeeper = 5,
        LastJoke = 6
    }

    public static class RolePriorityExtensions
    {
        // Get the priority of the RolePriority enum
        public static int GetPriority(this RolePriority rolePriority)
        {
            return (int)rolePriority;
        }
    }
}