using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public static class Utils
    {
        public static void FloodFill<T>(T[,] array, int x, int y, T replaceValue, T value, ref int numTiles)
        {
            if (replaceValue.Equals(value))
                return;

            if (x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1))
            {
                if (!array[x, y].Equals(replaceValue))
                    return;

                array[x, y] = value;
                numTiles++;

                FloodFill(array, x - 1, y, replaceValue, value, ref numTiles);
                FloodFill(array, x + 1, y, replaceValue, value, ref numTiles);
                FloodFill(array, x, y - 1, replaceValue, value, ref numTiles);
                FloodFill(array, x, y + 1, replaceValue, value, ref numTiles);
            }
        }

        public static void FloodFill<T>(T[,] array, bool[,] active, int x, int y, T replaceValue, T value, ref int numTiles)
        {
            if (replaceValue.Equals(value))
                return;
            
            if (x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1))
            {
                if (!active[x, y])
                    return;

                if (!array[x, y].Equals(replaceValue))
                    return;

                array[x, y] = value;
                numTiles++;

                FloodFill(array, active, x - 1, y, replaceValue, value, ref numTiles);
                FloodFill(array, active, x + 1, y, replaceValue, value, ref numTiles);
                FloodFill(array, active, x, y - 1, replaceValue, value, ref numTiles);
                FloodFill(array, active, x, y + 1, replaceValue, value, ref numTiles);
            }
        }
    }
}
