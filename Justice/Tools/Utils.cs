using BEPUphysics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public static class Utils
    {
        public static Microsoft.Xna.Framework.Matrix Convert(this BEPUutilities.Matrix source)
        {
            Microsoft.Xna.Framework.Matrix result;

            result.M11 = source.M11;
            result.M12 = source.M12;
            result.M13 = source.M13;
            result.M14 = source.M14;

            result.M21 = source.M21;
            result.M22 = source.M22;
            result.M23 = source.M23;
            result.M24 = source.M24;

            result.M31 = source.M31;
            result.M32 = source.M32;
            result.M33 = source.M33;
            result.M34 = source.M34;

            result.M41 = source.M41;
            result.M42 = source.M42;
            result.M43 = source.M43;
            result.M44 = source.M44;

            return result;
        }

        public static BEPUutilities.Matrix Convert(this Microsoft.Xna.Framework.Matrix source)
        {
            BEPUutilities.Matrix result;

            result.M11 = source.M11;
            result.M12 = source.M12;
            result.M13 = source.M13;
            result.M14 = source.M14;

            result.M21 = source.M21;
            result.M22 = source.M22;
            result.M23 = source.M23;
            result.M24 = source.M24;

            result.M31 = source.M31;
            result.M32 = source.M32;
            result.M33 = source.M33;
            result.M34 = source.M34;

            result.M41 = source.M41;
            result.M42 = source.M42;
            result.M43 = source.M43;
            result.M44 = source.M44;

            return result;
        }

        public static  Microsoft.Xna.Framework.Vector3 Convert(this BEPUutilities.Vector3 source)
        {
            Microsoft.Xna.Framework.Vector3 result;

            result.X = source.X;
            result.Y = source.Y;
            result.Z = source.Z;

            return result;
        }

        public static BEPUutilities.Vector3 Convert(this Microsoft.Xna.Framework.Vector3 source)
        {
            BEPUutilities.Vector3 result;

            result.X = source.X;
            result.Y = source.Y;
            result.Z = source.Z;

            return result;
        }

        public static Microsoft.Xna.Framework.Vector2 Convert(this BEPUutilities.Vector2 source)
        {
            Microsoft.Xna.Framework.Vector2 result;

            result.X = source.X;
            result.Y = source.Y;

            return result;
        }

        public static BEPUutilities.Vector2 Convert(this Microsoft.Xna.Framework.Vector2 source)
        {
            BEPUutilities.Vector2 result;

            result.X = source.X;
            result.Y = source.Y;

            return result;
        }

        public static BEPUphysics.Entities.Prefabs.Box GeneratePhysicsBox(this Microsoft.Xna.Framework.BoundingBox bounds)
        {
            BEPUutilities.Vector3 center = ((bounds.Min + bounds.Max) / 2).Convert();
            Microsoft.Xna.Framework.Vector3 size = bounds.Max - bounds.Min;

            return new BEPUphysics.Entities.Prefabs.Box(center, size.X, size.Y, size.Z);
        }

        public static BEPUphysics.Entities.Prefabs.Box GeneratePhysicsBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            BEPUutilities.Vector3 center = new BEPUutilities.Vector3(maxX + minX, maxY + minY, maxZ + minZ) / 2.0f;
            BEPUutilities.Vector3 size = new BEPUutilities.Vector3(maxX - minX, maxY - minY, maxZ - minZ);

            return new BEPUphysics.Entities.Prefabs.Box(center, size.X, size.Y, size.Z);
        }


        public static BEPUphysics.Entities.Prefabs.Box GeneratePhysicsBox(float minX, float minY, float minZ, float maxX, float maxY, float maxZ, float friction)
        {
            BEPUutilities.Vector3 center = new BEPUutilities.Vector3(maxX + minX, maxY + minY, maxZ + minZ) / 2.0f;
            BEPUutilities.Vector3 size = new BEPUutilities.Vector3(maxX - minX, maxY - minY, maxZ - minZ);

            BEPUphysics.Entities.Prefabs.Box box = new BEPUphysics.Entities.Prefabs.Box(center, size.X, size.Y, size.Z);

            box.Material = new Material(friction, friction, 1.0f);
            return box;
        }

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
