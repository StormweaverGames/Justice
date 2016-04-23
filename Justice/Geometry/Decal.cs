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

        public override BoundingBox RenderBounds
        {
            get { return myRenderable.RenderBounds; }
        }

        public override bool IsTransparent
        {
            get { return true; }
        }

        public override Texture2D Texture
        {
            get
            {
                return myRenderable.Texture;
            }
        }

        public override bool IsVisible
        {
            get { return myRenderable.IsVisible; }
            set { myRenderable.IsVisible = value; }
        }

        public SamplerState Sampler { get; internal set; }
        
        public Decal(IRenderable renderable)
        {
            myRenderable = renderable;
        }

        public override void Init(GraphicsDevice graphics)
        {
            myRenderable.Init(graphics);
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            graphics.BlendState = BlendState.AlphaBlend;
            myRenderable.Render(graphics, matrices);
            graphics.BlendState = BlendState.Opaque;
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return myRenderable.ShouldRender(cameraFrustum);
        }
    }
}
