using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public class LayeredRenderTarget
    {
        protected RenderTarget2D[] myTargets;
        protected RenderTargetBinding[] myBindings;

        protected int myWidth;
        protected int myHeight;

        public RenderTarget2D this[int layerIndex]
        {
            get { return myTargets[layerIndex]; }
        }

        public LayeredRenderTarget(GraphicsDevice graphics, int width, int height, int layerCount)
        {
            myWidth = width;
            myHeight = height;

            myTargets = new RenderTarget2D[layerCount];
            myBindings = new RenderTargetBinding[layerCount];

            for(int index = 0; index < layerCount; index ++)
            {
                myTargets[index] = new RenderTarget2D(graphics, width, height);
                myBindings[index] = myTargets[index];
            }
        }

        public void Bind(GraphicsDevice graphics)
        {
            graphics.SetRenderTargets(myBindings);
        }
    }
}
