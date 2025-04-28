namespace Game.Models.Roles.Enums
{
    public enum Team
    {
        Folk = 1,
        Corrupter = 2,
        Neutral = 3
    }

    public static class TeamExtensions
    {
        public static bool IsCollaborative(this Team team)
        {
            return team switch
            {
                Team.Folk => true,
                Team.Corrupter => true,
                _ => false
            };
        }
    }
}