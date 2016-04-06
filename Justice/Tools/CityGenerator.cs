using Justice.Geometry;
using Justice.Tools;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public class CityGenerator
    {
        Random rand;

        public CityGenerator()
        {
            rand = new Random();
        }

        public void GenerateCity(GeometryBuilder<VertexPositionNormalTexture> builder, float xBlocks, int yBlocks)
        {
            for(int x = 0; x < xBlocks; x ++)
            {
                for(int y = 0; y < yBlocks; y ++)
                {
                    GenerateBlock(builder, x * 160, y * 160, 110, 110);

                    // Sidewalks
                    builder.AddCube(x * 160 - 50, y * 160 - 5, 0, x * 160 - 45, y * 160 + 115, 0.215f);
                    builder.AddCube(x * 160 - 5,  y * 160 - 5, 0, x * 160,      y * 160 + 115, 0.215f);

                    builder.AddCube(x * 160, y * 160 - 50, 0, x * 160 + 110, y * 160 - 45 , 0.215f);
                    builder.AddCube(x * 160, y * 160 - 5, 0, x * 160 + 110, y * 160, 0.215f);

                    // Roads
                    builder.AddCube(x * 160 - 45, y * 160 - 5, 0, x * 160 - 5, y * 160 + 115, 0.015f);
                    builder.AddCube(x * 160 - 5, y * 160 - 45, 0, x * 160 + 115, y * 160 - 5, 0.015f);

                    // Intersection
                    builder.AddCube(x * 160 - 50, y * 160 - 50, 0, x * 160, y * 160, 0.015f);
                }
            }
        }

        public void GenerateBlock(GeometryBuilder<VertexPositionNormalTexture> builder, float startX, float startY, int width, int height)
        {            
            if (width % 10 != 0 || height % 10 != 0)
                throw new ArgumentException("Width and height must be multiples of 10");

            bool[,] occupied = new bool[width / 10, height / 10];

            for(int x = 0; x < occupied.GetLength(0); x ++)
            {
                for(int y = 0; y < occupied.GetLength(1); y++)
                {
                    occupied[x, y] = true;
                }
            }

            int numAllies = rand.Next(2, 8);

            for(int allyIndex = 0; allyIndex < numAllies; )
            {
                int direction = rand.Next(4);

                int xPos = 0;
                int yPos = 0;

                switch(direction)
                {
                    case 2: // Going right
                        xPos = 1;
                        yPos = rand.Next(2, occupied.GetLength(1) - 4);
                        occupied[xPos - 1, yPos] = false;
                        break;
                    case 0: // Going left
                        xPos = occupied.GetLength(0) - 2;
                        yPos = rand.Next(2, occupied.GetLength(1) - 4);
                        occupied[xPos + 1, yPos] = false;
                        break;
                    case 3: // Going down
                        xPos = rand.Next(2, occupied.GetLength(0) - 4);
                        yPos = 1;
                        occupied[xPos, yPos - 1] = false;
                        break;
                    case 1: // Going up
                        xPos = rand.Next(2, occupied.GetLength(0) - 4);
                        yPos = occupied.GetLength(1) - 2;
                        occupied[xPos, yPos + 1] = false;
                        break;
                }

                bool ignore = false;

                switch(direction)
                {
                    case 2:
                    case 0:
                        if (occupied[xPos, yPos - 1] == false || occupied[xPos, yPos + 1] == false)
                            ignore = true;
                        break;

                    case 1:
                    case 3:
                        if (occupied[xPos -1, yPos] == false || occupied[xPos + 1, yPos] == false)
                            ignore = true;
                        break;
                }

                if (ignore)
                    continue;
                else
                {
                    switch (direction)
                    {
                        case 2: // Going right
                            occupied[xPos - 1, yPos] = false;
                            break;
                        case 0: // Going left
                            occupied[xPos + 1, yPos] = false;
                            break;
                        case 3: // Going down
                            occupied[xPos, yPos - 1] = false;
                            break;
                        case 1: // Going up
                            occupied[xPos, yPos + 1] = false;
                            break;
                    }
                }
                
                bool end = false;

                int turn = rand.Next(0, 2);

                while (!end && xPos > 0 && xPos < occupied.GetLength(0) - 1 && yPos > 0 && yPos < occupied.GetLength(1) - 1)
                {
                    int length = rand.Next(8, 11);

                    switch (direction)
                    {
                        case 2: // Going right
                            for(int x = 0; xPos < occupied.GetLength(0); x++, xPos++)
                            {
                                if (occupied[xPos, yPos] == false) { end = true; }

                                occupied[xPos, yPos] = false;
                            }
                            break;
                        case 0: // Going left
                            for (int x = 0; xPos >= 0; x++, xPos--)
                            {
                                if (occupied[xPos, yPos] == false) { end = true; }

                                occupied[xPos, yPos] = false;
                            }
                            break;
                        case 3: // Going down
                            for (int y = 0; yPos < occupied.GetLength(1); y++, yPos++)
                            {
                                if (occupied[xPos, yPos] == false) { end = true; }

                                occupied[xPos, yPos] = false;
                            }
                            break;
                        case 1: // Going up
                            for (int y = 0; yPos >= 0; y++, yPos--)
                            {
                                if (occupied[xPos, yPos] == false) { end = true; }

                                occupied[xPos, yPos] = false;
                            }
                            break;
                    }

                    turn = turn == 0 ? 1 : 0;

                    direction += turn == 0 ? -1 : 1;

                    direction = direction < 0 ? 4 - direction : direction > 3 ? direction - 4 : direction;
                }

                allyIndex++;
            }

            // Remove 1 wide buildings
            for (int x = 1; x < occupied.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < occupied.GetLength(1) - 1; y++)
                {
                    if (occupied[x, y])
                    {
                        if (!occupied[x - 1, y] && !occupied[x + 1, y] && (occupied[x, y - 1] || occupied[x, y + 1]))
                        {
                            occupied[x, y] = false;
                        }

                        if (!occupied[x, y - 1] && !occupied[x, y + 1] && (occupied[x - 1, y] || occupied[x + 1, y]))
                        {
                            occupied[x, y] = false;
                        }
                    }
                }
            }

            int[,] buildingIds = new int[occupied.GetLength(0), occupied.GetLength(1)];

            int id = 1;

            // Get all attached buildings
            for (int x = 1; x < occupied.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < occupied.GetLength(1) - 1; y++)
                {
                    if (buildingIds[x, y] == 0 && occupied[x, y])
                    {
                        int tileCount = 0;
                        Utils.FloodFill(buildingIds, occupied, x, y, 0, id, ref tileCount);

                        if (tileCount > 6)
                            id++;
                        else
                            Utils.FloodFill(buildingIds, x, y, id, 0, ref tileCount);

                    }
                }
            }

            int[] buildingHeights = new int[id - 1];

            for(int index = 0; index < buildingHeights.Length; index ++)
            {
                buildingHeights[index] = rand.Next(4, 11) * 10;
            }

            for(int xIndex = 0; xIndex < occupied.GetLength(0); xIndex ++)
            {
                for(int yIndex = 0; yIndex < occupied.GetLength(1); yIndex ++)
                {
                    if (buildingIds[xIndex, yIndex] != 0)
                    {
                        builder.AddCube(startX + xIndex * 10, startY +yIndex * 10, 0, startX + (xIndex + 1) * 10, startY + (yIndex + 1) * 10, buildingHeights[buildingIds[xIndex, yIndex] - 1]);
                    }
                }
            }
        }
    }
}
