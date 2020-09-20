using System;

namespace Game.Scripts.Helpers
{
    public static class Extensions
    {
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
    }
}