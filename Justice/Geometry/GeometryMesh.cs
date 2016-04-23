using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class GeometryMesh : IRenderable, ITransformable
    {
        VertexBuffer myVertices;
        IndexBuffer myIndices;
        PrimitiveType myPrimitiveType;
        int myPrimitiveCount;
        Effect myEffect;
        Matrix myTransform;
        BoundingBox myBounds;
        BoundingBox myRenderBounds;

        /// <summary>
        /// Gets or sets the bounds for this geometry
        /// </summary>
        public override BoundingBox RenderBounds
        {
            get { return myRenderBounds; }
        }

        public override Texture2D Texture
        {
            get
            {
                return (myEffect as BasicEffect).Texture;
            }
        }

        public Effect Effect
        {
            get { return myEffect; }
            set { myEffect = value; }
        }
        public Matrix Transformation
        {
            get { return myTransform; }
            set
            {
                myTransform = value;

                Vector3 min = Vector3.Transform(myBounds.Min, value);
                Vector3 max = Vector3.Transform(myBounds.Max, value);

                myRenderBounds.Min = Vector3.Min(min, max);
                myRenderBounds.Max = Vector3.Max(min, max);
            }
        }
        
        public GeometryMesh(Effect effect, VertexBuffer vertices, PrimitiveType primitiveType)
        {
            myEffect = effect;
            myVertices = vertices;
            myPrimitiveType = primitiveType;
            myTransform = Matrix.Identity;
            IsVisible = true;
        }

        public GeometryMesh(Effect effect,VertexBuffer vertices, IndexBuffer indices, PrimitiveType primitiveType)
        {
            myEffect = effect;
            myVertices = vertices;
            myIndices = indices;
            myPrimitiveType = primitiveType;
            myTransform = Matrix.Identity;
            IsVisible = true;
        }

        public void SetBounds(BoundingBox bounds)
        {
            myBounds = bounds;
            Transformation = myTransform;
        }

        public override void Init(GraphicsDevice graphics)
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

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            (myEffect as BasicEffect).View = matrices.View;
            (myEffect as BasicEffect).Projection = matrices.Projection;
            (myEffect as BasicEffect).World = myTransform;

            graphics.SetVertexBuffer(myVertices);
            graphics.Indices = myIndices;

            for (int passIndex = 0; passIndex < myEffect.CurrentTechnique.Passes.Count; passIndex++)
            {
                myEffect.CurrentTechnique.Passes[passIndex].Apply();

                if (myIndices != null)
                {
                    graphics.DrawIndexedPrimitives(myPrimitiveType, 0, 0, myPrimitiveCount);
                }
                else
                {
                    graphics.DrawPrimitives(myPrimitiveType, 0, myPrimitiveCount);
                }
            }
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true; //cameraFrustum.Intersects(myBounds);
        }
    }
}
