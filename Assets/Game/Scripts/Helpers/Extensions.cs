using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts.Helpers
{
    public static class Extensions
    {
        private static Random Random = new Random();
        public static int FindIndex<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item);
        }
        public static (int i, int j) FindIndex<T>(this T[,] array, T item)
        {
            int w = array.GetLength(0);
            int h = array.GetLength(1);

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (array[x, y].Equals(item))
                        return (x,y);
                }
            }

            throw new ArgumentOutOfRangeException("Could not find the element in the given array");
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            var index = Random.Next(0, array.Length);
            return array[index];
        }
        public static T GetRandomElement<T>(this List<T> list)
        {
            var index = Random.Next(0, list.Count);
            return list[index];
        }
    }
}