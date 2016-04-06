using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public static class MeshGenerator
    {
        private static readonly Vector3 TOP_FRONT_RIGHT =       new Vector3( 1,  1,  1);
        private static readonly Vector3 TOP_FRONT_LEFT =        new Vector3(-1,  1,  1);
        private static readonly Vector3 TOP_BACK_RIGHT =        new Vector3( 1, -1,  1);
        private static readonly Vector3 TOP_BACK_LEFT =         new Vector3(-1, -1,  1);
        private static readonly Vector3 BOTTOM_FRONT_RIGHT =    new Vector3( 1,  1, -1);
        private static readonly Vector3 BOTTOM_FRONT_LEFT =     new Vector3(-1,  1, -1);
        private static readonly Vector3 BOTTOM_BACK_RIGHT =     new Vector3( 1, -1, -1);
        private static readonly Vector3 BOTTOM_BACK_LEFT =      new Vector3(-1, -1, -1);

        private static readonly Vector3 NORMAL_TOP =            new Vector3( 0,  0,  1);
        private static readonly Vector3 NORMAL_BOTTOM =         new Vector3( 0,  0, -1);
        private static readonly Vector3 NORMAL_FRONT =          new Vector3( 0,  1,  0);
        private static readonly Vector3 NORMAL_BACK =           new Vector3( 0, -1,  0);
        private static readonly Vector3 NORMAL_LEFT =           new Vector3(-1,  0,  0);
        private static readonly Vector3 NORMAL_RIGHT =          new Vector3( 1,  0,  0);

        private static readonly Vector2 TEXTURE_TOP_LEFT =      new Vector2(0, 0);
        private static readonly Vector2 TEXTURE_TOP_RIGHT =     new Vector2(1, 0);
        private static readonly Vector2 TEXTURE_BOTTOM_LEFT =   new Vector2(0, 1);
        private static readonly Vector2 TEXTURE_BOTTOM_RIGHT =  new Vector2(1, 1);

        public static void AddSkyBox(this GeometryBuilder<VertexPositionNormalTexture> builder)
        {
            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[24];
            int[] indices = new int[36];
            
            Vector3 center = Vector3.Zero;
            Vector3 size = new Vector3(0.5f);

            Vector2 tex_topface_tl = new Vector2(0.25f, 0f);
            Vector2 tex_topface_tr = new Vector2(0.5f, 0f);
            Vector2 tex_topface_bl = new Vector2(0.25f, 1 / 3.0f);
            Vector2 tex_topface_br = new Vector2(0.5f, 1 / 3.0f);

            Vector2 tex_leftface_tl = new Vector2(0f, 1 / 3.0f);
            Vector2 tex_leftface_tr = new Vector2(0.25f, 1 / 3.0f);
            Vector2 tex_leftface_bl = new Vector2(0f, 2 / 3.0f);
            Vector2 tex_leftface_br = new Vector2(0.25f, 2 / 3.0f);

            Vector2 tex_frontface_tl = new Vector2(0.25f, 1 / 3.0f);
            Vector2 tex_frontface_tr = new Vector2(0.5f, 1 / 3.0f);
            Vector2 tex_frontface_bl = new Vector2(0.25f, 2 / 3.0f);
            Vector2 tex_frontface_br = new Vector2(0.5f, 2 / 3.0f);

            Vector2 tex_rightface_tl = new Vector2(0.5f, 1 / 3.0f);
            Vector2 tex_rightface_tr = new Vector2(0.75f, 1 / 3.0f);
            Vector2 tex_rightface_bl = new Vector2(0.5f, 2 / 3.0f);
            Vector2 tex_rightface_br = new Vector2(0.75f, 2 / 3.0f);

            Vector2 tex_backface_tl = new Vector2(0.75f, 1 / 3.0f);
            Vector2 tex_backface_tr = new Vector2(1f, 1 / 3.0f);
            Vector2 tex_backface_bl = new Vector2(0.75f, 2 / 3.0f);
            Vector2 tex_backface_br = new Vector2(1f, 2 / 3.0f);

            Vector2 tex_bottomface_tl = new Vector2(0.25f, 2 / 3.0f);
            Vector2 tex_bottomface_tr = new Vector2(0.5f, 2 / 3.0f);
            Vector2 tex_bottomface_bl = new Vector2(0.25f, 1f);
            Vector2 tex_bottomface_br = new Vector2(0.5f, 1f);

            // Top surface
            verts[0] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_TOP, tex_topface_bl);
            verts[1] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_TOP, tex_topface_br);
            verts[2] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_TOP, tex_topface_tl);
            verts[3] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_TOP, tex_topface_tr);

            // Bottom Surface
            verts[4] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_BOTTOM, tex_bottomface_bl);
            verts[5] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_BOTTOM, tex_bottomface_br);
            verts[6] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_BOTTOM, tex_bottomface_tl);
            verts[7] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_BOTTOM, tex_bottomface_tr);

            // Front Surface
            verts[8] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_FRONT, tex_frontface_bl);
            verts[9] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_FRONT, tex_frontface_br);
            verts[10] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_FRONT, tex_frontface_tl);
            verts[11] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_FRONT, tex_frontface_tr);

            // Back Surface
            verts[12] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_BACK, tex_backface_bl);
            verts[13] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_BACK, tex_backface_br);
            verts[14] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_BACK, tex_backface_tl);
            verts[15] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_BACK, tex_backface_tr);

            // Left Surface
            verts[16] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_LEFT, tex_leftface_bl);
            verts[17] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_LEFT, tex_leftface_br);
            verts[18] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_LEFT, tex_leftface_tl);
            verts[19] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_LEFT, tex_leftface_tr);

            // Right Surface
            verts[20] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_RIGHT, tex_rightface_bl);
            verts[21] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_RIGHT, tex_rightface_br);
            verts[22] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_RIGHT, tex_rightface_tl);
            verts[23] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_RIGHT, tex_rightface_tr);

            int startIndex = builder.AddVertices(verts);

            // Iterate over and add each face
            for (int vIndex = 0; vIndex < indices.Length; vIndex += 6)
            {
                indices[vIndex + 0] = startIndex + (vIndex / 6 * 4) + 1;
                indices[vIndex + 1] = startIndex + (vIndex / 6 * 4) + 0;
                indices[vIndex + 2] = startIndex + (vIndex / 6 * 4) + 2;

                indices[vIndex + 3] = startIndex + (vIndex / 6 * 4) + 1;
                indices[vIndex + 4] = startIndex + (vIndex / 6 * 4) + 2;
                indices[vIndex + 5] = startIndex + (vIndex / 6 * 4) + 3;
            }

            builder.AddIndices(indices);
        }

        public static void AddCube(this GeometryBuilder<VertexPositionNormalTexture> builder, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            builder.AddCube(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2));
        }

        public static void AddCube(this GeometryBuilder<VertexPositionNormalTexture> builder, Vector3 min, Vector3 max)
        {
            VertexPositionNormalTexture[] verts = new VertexPositionNormalTexture[24];
            int[] indices = new int[36];

            Vector3 size = Vector3.Max(min, max) - Vector3.Min(min, max);
            Vector3 center = (max + min) / 2.0f;

            size /= 2.0f;

            // Top surface
            verts[0] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_TOP, TEXTURE_BOTTOM_LEFT);
            verts[1] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_TOP, TEXTURE_BOTTOM_RIGHT);
            verts[2] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_TOP, TEXTURE_TOP_LEFT);
            verts[3] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_TOP, TEXTURE_TOP_RIGHT);

            // Bottom Surface
            verts[4] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_BOTTOM, TEXTURE_BOTTOM_LEFT);
            verts[5] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_BOTTOM, TEXTURE_BOTTOM_RIGHT);
            verts[6] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_BOTTOM, TEXTURE_TOP_LEFT);
            verts[7] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_BOTTOM, TEXTURE_TOP_RIGHT);

            // Front Surface
            verts[8] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_FRONT, TEXTURE_BOTTOM_LEFT);
            verts[9] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_FRONT, TEXTURE_BOTTOM_RIGHT);
            verts[10] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_FRONT, TEXTURE_TOP_LEFT);
            verts[11] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_FRONT, TEXTURE_TOP_RIGHT);

            // Back Surface
            verts[12] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_BACK, TEXTURE_BOTTOM_LEFT);
            verts[13] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_BACK, TEXTURE_BOTTOM_RIGHT);
            verts[14] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_BACK, TEXTURE_TOP_LEFT);
            verts[15] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_BACK, TEXTURE_TOP_RIGHT);

            // Left Surface
            verts[16] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_LEFT, NORMAL_LEFT, TEXTURE_BOTTOM_LEFT);
            verts[17] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_LEFT, NORMAL_LEFT, TEXTURE_BOTTOM_RIGHT);
            verts[18] = new VertexPositionNormalTexture(center + size * TOP_BACK_LEFT, NORMAL_LEFT, TEXTURE_TOP_LEFT);
            verts[19] = new VertexPositionNormalTexture(center + size * TOP_FRONT_LEFT, NORMAL_LEFT, TEXTURE_TOP_RIGHT);

            // Right Surface
            verts[20] = new VertexPositionNormalTexture(center + size * BOTTOM_FRONT_RIGHT, NORMAL_RIGHT, TEXTURE_BOTTOM_LEFT);
            verts[21] = new VertexPositionNormalTexture(center + size * BOTTOM_BACK_RIGHT, NORMAL_RIGHT, TEXTURE_BOTTOM_RIGHT);
            verts[22] = new VertexPositionNormalTexture(center + size * TOP_FRONT_RIGHT, NORMAL_RIGHT, TEXTURE_TOP_LEFT);
            verts[23] = new VertexPositionNormalTexture(center + size * TOP_BACK_RIGHT, NORMAL_RIGHT, TEXTURE_TOP_RIGHT);

            int startIndex = builder.AddVertices(verts);

            // Iterate over and add each face
            for(int vIndex = 0; vIndex < indices.Length; vIndex += 6)
            {
                indices[vIndex + 0] = startIndex + (vIndex / 6 * 4) + 0;
                indices[vIndex + 1] = startIndex + (vIndex / 6 * 4) + 1;
                indices[vIndex + 2] = startIndex + (vIndex / 6 * 4) + 2;
                indices[vIndex + 3] = startIndex + (vIndex / 6 * 4) + 2;
                indices[vIndex + 4] = startIndex + (vIndex / 6 * 4) + 1;
                indices[vIndex + 5] = startIndex + (vIndex / 6 * 4) + 3;
            }

            builder.AddIndices(indices);
        }
    }
}
