using System;
using game.Constants;

namespace game.models.roles.properties
{
    public class ChanceProperty
    {
        public static readonly int NoMaxLimit = GameConstants.MaxPlayerCount;
        public const int Unique = 1;
        public int Chance { get; }
        public int MaxNumber { get; }

        public ChanceProperty(int chance, int maxNumber)
        {
            if (chance <= 0)
            {
                throw new ArgumentException("Chance must be positive");
            }

            if (maxNumber <= 0)
            {
                throw new ArgumentException("Max number must be positive");
            }

            Chance = chance;
            MaxNumber = maxNumber;
        }

        public override string ToString()
        {
            return $"ChanceProperty{{chance={Chance}, maxNumber={MaxNumber}}}";
        }
    }
}