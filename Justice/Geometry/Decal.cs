using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class Decal : IRenderable
    {
        IRenderable myRenderable;

        public BoundingBox Bounds
        {
            get { return myRenderable.Bounds; }
        }

        public bool IsVisible
        {
            get { return myRenderable.IsVisible; }
        }

        public SamplerState Sampler { get; internal set; }

        public Decal(IRenderable renderable)
        {
            myRenderable = renderable;
        }

        public void Init(GraphicsDevice graphics)
        {
            myRenderable.Init(graphics);
        }

        public void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            graphics.BlendState = BlendState.AlphaBlend;
            myRenderable.Render(graphics, matrices);
            graphics.BlendState = BlendState.Opaque;
        }

        public bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return myRenderable.ShouldRender(cameraFrustum);
        }
    }
}
