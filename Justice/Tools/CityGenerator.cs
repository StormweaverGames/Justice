using BEPUphysics.Entities.Prefabs;
using Justice.Geometry;
using Justice.Tools;
using Microsoft.Xna.Framework;
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

        private struct Building
        {
            public BoundingBox[] Bounds;

            public Building(float x, float y, float z, float width, float height, float depth)
            {
                Bounds = new BoundingBox[] { new BoundingBox(new Vector3(x, y, z), new Vector3(x + width, y + height, z + depth)) };
            }

            public Building(IEnumerable<BoundingBox> bounds)
            {
                Bounds = bounds.ToArray();
            }
        }

        public CityGenerator()
        {
            rand = new Random((int)DateTime.Now.Ticks);
        }

        public void GenerateCity(IScene scene, GraphicsDevice graphics, Effect effect, float xBlocks, int yBlocks)
        {
            GeometryBuilder<VertexPositionNormalTexture> builder = new GeometryBuilder<VertexPositionNormalTexture>(PrimitiveType.TriangleList);
            //MultiMesh mesh = new MultiMesh();

            for (int x = 0; x < xBlocks; x ++)
            {
                for(int y = 0; y < yBlocks; y ++)
                {
                    GenerateBlock(builder, scene, x * 160, y * 160, 110, 110);

                    // Sidewalks
                    builder.AddCube(x * 160 - 5, y * 160 - 5, 0, x * 160 + 115f, y * 160 + 115, 0.215f);

                    // Roads
                    builder.AddCube(x * 160 - 45, y * 160 - 5, 0, x * 160 - 5, y * 160 + 115, 0.015f);
                    builder.AddCube(x * 160 - 5, y * 160 - 45, 0, x * 160 + 115, y * 160 - 5, 0.015f);

                    // Intersection
                    builder.AddCube(x * 160 - 45, y * 160 - 45, 0, x * 160 - 5, y * 160 - 5, 0.015f);

                    Box floorPlane = Utils.GeneratePhysicsBox(x * 160 - 45, y * 160 - 45, -5, x * 160 + 115, y * 160 + 115, 0.215f, 0.25f);
                    scene.AddCollider(floorPlane);              
                }
            }

            GeometryMesh part = builder.Bake(graphics, 0);
            scene.AddRenderable("opaque", part);

            //mesh.Add(part);

            //return mesh;
        }

        public void GenerateBlock(GeometryBuilder<VertexPositionNormalTexture> builder, IScene scene, float startX, float startY, int width, int height)
        {            
            if (width % 10 != 0 || height % 10 != 0)
                throw new ArgumentException("Width and height must be multiples of 10");

            int numTilesX = width / 10;
            int numTilesY = height / 10;

            List<Building> buildings = new List<Building>();
            List<BoundingBox> tempBuildingBounds = new List<BoundingBox>();

            for(int x = 0; x < numTilesX; )
            {
                int buildingWidth = rand.Next(3, numTilesX - x);

                if (numTilesX - x - buildingWidth < 3)
                {
                    buildingWidth = numTilesX - x;
                }

                int buildingDepth = rand.Next(3, numTilesY);

                if (numTilesY - buildingDepth < 3)
                    buildingDepth = numTilesY;

                buildings.Add(new Building(startX + x * 10, startY, 0, buildingWidth * 10, buildingDepth * 10, rand.Next(4, 8) * 10));

                if (buildingDepth < numTilesY)
                {
                    if (rand.Next(2) == 0)
                    {
                        buildings.Add(new Building(tempBuildingBounds));
                        tempBuildingBounds.Clear();

                        buildings.Add(new Building(startX + x * 10, startY + buildingDepth * 10, 0, buildingWidth * 10, (numTilesY - buildingDepth) * 10, rand.Next(4, 8) * 10));
                    }
                    else
                    {
                        int buildingHeight = rand.Next(4, 8) * 10;

                        if (tempBuildingBounds.Count > 0)
                            buildingHeight = (int)tempBuildingBounds[0].Max.Z;

                        tempBuildingBounds.Add(new BoundingBox(
                            new Vector3(startX + x * 10, startY + buildingDepth * 10, 0),
                            new Vector3(startX + x * 10 + buildingWidth * 10, startY + height, buildingHeight)));
                    }
                }

               x += buildingWidth;
            }

            if (tempBuildingBounds.Count > 0)
                buildings.Add(new Building(tempBuildingBounds));

            foreach(Building b in buildings)
            {
                foreach(BoundingBox bounds in b.Bounds)
                {
                    builder.AddCube(bounds.Min.X + 2.5f, bounds.Min.Y + 2.5f, bounds.Min.Z, bounds.Max.X - 2.5f, bounds.Max.Y - 2.5f, bounds.Max.Z);
                    Vector3 center = (bounds.Max + bounds.Min) / 2.0f;
                    Box buildingBox = new Box(new BEPUutilities.Vector3(center.X, center.Y, center.Z), bounds.Max.X - bounds.Min.X - 5, bounds.Max.Y - bounds.Min.Y - 5, bounds.Max.Z - bounds.Min.Z);
                    scene.AddCollider(buildingBox);
                }
            }
        }
    }
}
