using System;
using System.Collections.Generic;
using System.Linq;

namespace game.Utils
{
    
    public static class RandomUtils
    {
        private static readonly Random Rand = new ();
        
        public static bool GetRandomBoolean()
        {
            return Rand.Next(2) == 0;
            
        }

        public static int GetRandomNumber(int min, int max)
        {
            return Rand.Next(min, max + 1);
        }
        
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var list = source as IList<T> ?? source.ToList();
            if (list.Count == 0)
                throw new InvalidOperationException("Collection is empty.");

            return list[Rand.Next(0, list.Count)];
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