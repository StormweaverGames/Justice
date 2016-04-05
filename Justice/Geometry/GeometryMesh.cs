using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class GeometryMesh : IRenderable
    {
        VertexBuffer myVertices;
        IndexBuffer myIndices;
        PrimitiveType myPrimitiveType;
        int myPrimitiveCount;
        Effect myEffect;

        /// <summary>
        /// Gets or sets the bounds for this geometry
        /// </summary>
        public BoundingBox Bounds
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets whether this geometry is visible
        /// </summary>
        public bool IsVisible
        {
            get;
            set;
        }
        public Effect Effect
        {
            get { return myEffect; }
            set { myEffect = value; }
        }

        public GeometryMesh(Effect effect, VertexBuffer vertices, PrimitiveType primitiveType)
        {
            myEffect = effect;
            myVertices = vertices;
            myPrimitiveType = primitiveType;
        }

        public GeometryMesh(Effect effect,VertexBuffer vertices, IndexBuffer indices, PrimitiveType primitiveType)
        {
            myEffect = effect;
            myVertices = vertices;
            myIndices = indices;
            myPrimitiveType = primitiveType;
        }

        public void Init(GraphicsDevice graphics)
        {
            switch(myPrimitiveType)
            {
                case PrimitiveType.LineList:
                    myPrimitiveCount = myIndices == null ? myVertices.VertexCount / 2 : myIndices.IndexCount / 2;
                    break;
                case PrimitiveType.LineStrip:
                    myPrimitiveCount = myIndices == null ? myVertices.VertexCount - 1 : myIndices.IndexCount - 1;
                    break;
                case PrimitiveType.TriangleList:
                    myPrimitiveCount = myIndices == null ? myVertices.VertexCount / 3 : myIndices.IndexCount / 3;
                    break;
                case PrimitiveType.TriangleStrip:
                    myPrimitiveCount = myIndices == null ? myVertices.VertexCount - 2 : myIndices.IndexCount - 2;
                    break;
            }
        }

        public void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            (myEffect as BasicEffect).View = matrices.View;
            (myEffect as BasicEffect).Projection = matrices.Projection;

            for (int passIndex = 0; passIndex < myEffect.CurrentTechnique.Passes.Count; passIndex++)
            {
                myEffect.CurrentTechnique.Passes[passIndex].Apply();
                graphics.SetVertexBuffer(myVertices);

                if (myIndices != null)
                {
                    graphics.Indices = myIndices;
                    graphics.DrawIndexedPrimitives(myPrimitiveType, 0, 0, myPrimitiveCount);
                }
                else
                {
                    graphics.DrawPrimitives(myPrimitiveType, 0, myPrimitiveCount);
                }
            }
        }

        public bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
