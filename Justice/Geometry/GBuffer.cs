using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public class GBuffer
    {
        private static readonly Color DIFFUSE_CLEAR = Color.White;
        private static readonly Color LIGHTING_CLEAR = new Color(0, 0, 0, 0);

        private RenderTarget2D myDiffuseTexture;
        private RenderTarget2D myLightingTexture;
        private RenderTarget2D myDepthBuffer;
        private RenderTarget2D myNormalBuffer;

        private RenderTargetBinding[] myBindings;
        
        private int myWidth;
        private int myHeight;

        GraphicsDevice myGraphics;

        public GBuffer(int width, int height, GraphicsDevice graphics)
        {
            myWidth = width;
            myHeight = height;

            myGraphics = graphics;

            myDiffuseTexture = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8);
            myDepthBuffer = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8);
            myNormalBuffer = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8);
            myLightingTexture = new RenderTarget2D(graphics, width, height, false, SurfaceFormat.Rgba64, DepthFormat.Depth24Stencil8);

            myBindings = new RenderTargetBinding[4]
            {
                myDiffuseTexture,
                myLightingTexture,
                myNormalBuffer,
                myDepthBuffer
            };
        }

        public void Bind()
        {
            myGraphics.SetRenderTargets(myBindings);
        }

        public void Clear()
        {
            myGraphics.SetRenderTarget(myDiffuseTexture);
            myGraphics.Clear(DIFFUSE_CLEAR);
            myGraphics.SetRenderTarget(myLightingTexture);
            myGraphics.Clear(LIGHTING_CLEAR);
        }
    }
}
