namespace game.models.player
{
    public interface IPlayer
    {
        public int Number { get; }
        public string Name { get; }
    }
    
    public static class PlayerExtensions
    {
        public static bool IsSamePlayer(this IPlayer self, IPlayer other)
        {
            if (other == null) return false;
            return self.Number == other.Number;
        }

        public static bool IsSamePlayer(this IPlayer self, int number)
        {
            return self.Number == number;
        }
        
        public static string GetNameAndNumber(this IPlayer self)
        {
            return $"{self.Name} #{self.Number}";
        }
    }
}