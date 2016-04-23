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

        public override BoundingBox RenderBounds
        {
            get { return myBounds; }
        }
        
        public MultiMesh()
        {
            myRenderables = new IRenderable[0];
            IsVisible = true;
        }

        public void Add(IRenderable renderable)
        {
            Array.Resize(ref myRenderables, myRenderables.Length + 1);
            myRenderables[myRenderables.Length - 1] = renderable;

            myBounds.Min = Vector3.Min(renderable.RenderBounds.Min, myBounds.Min);
            myBounds.Max = Vector3.Max(renderable.RenderBounds.Max, myBounds.Max);
        }

        public override void Init(GraphicsDevice graphics)
        {
            for (int index = 0; index < myRenderables.Length; index++)
                myRenderables[index].Init(graphics);
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            for (int index = 0; index < myRenderables.Length; index++)
                myRenderables[index].Render(graphics, matrices);
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
