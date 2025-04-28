using System;
using System.Collections.Generic;

namespace game.Utils
{
    
    public static class RandomUtils
    {
        private static readonly Random Rand = new Random();
        
        public static bool GetRandomBoolean()
        {
            return Rand.Next(2) == 0;
            
        }

        public static int GetRandomNumber(int min, int max)
        {
            return Rand.Next(min, max + 1);
        }
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
            
            return list[GetRandomNumber(0, list.Count-1)];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = Rand.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
    
}