using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class MultiMesh : IRenderable
    {
        private IRenderable[] myRenderables;
        private BoundingBox myBounds;

        public BoundingBox Bounds
        {
            get { return myBounds; }
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public MultiMesh()
        {
            myRenderables = new IRenderable[0];
        }

        public void Add(IRenderable renderable)
        {
            Array.Resize(ref myRenderables, myRenderables.Length + 1);
            myRenderables[myRenderables.Length - 1] = renderable;

            myBounds.Min = Vector3.Min(renderable.Bounds.Min, myBounds.Min);
            myBounds.Max = Vector3.Max(renderable.Bounds.Max, myBounds.Max);
        }

        public void Init(GraphicsDevice graphics)
        {
            for (int index = 0; index < myRenderables.Length; index++)
                myRenderables[index].Init(graphics);
        }

        public void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            for (int index = 0; index < myRenderables.Length; index++)
                myRenderables[index].Render(graphics, matrices);
        }

        public bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
