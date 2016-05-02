using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.UI
{
    public class TestComponent : ScreenElement
    {
        private UIRenderer myRenderer;

        protected override void OnInit(GraphicsDevice graphics)
        {
            base.OnInit(graphics);
            myRenderer = new UIRenderer(graphics);
        }

        protected override void OnViewportResized(Viewport viewport)
        {
            base.OnViewportResized(viewport);

            myRenderer.ViewportResised(viewport);
        }

        protected override void OnRender(GraphicsDevice graphics, GameTime gameTime)
        {
            base.OnRender(graphics, gameTime);

            myRenderer.DrawRectangle(Bounds, 5.0f, Color.Yellow, BorderStyle.Centered);

            myRenderer.DrawFilledRectangleWithOutline(new Rectangle(200, 300, 100, 100), 10.0f, Color.Yellow, Color.Green, BorderStyle.Centered);
            myRenderer.Flush();
        }
    }
}
