using Justice.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public interface IUserInterface
    {
        bool IsInitialized { get; set; }

        void HandleMousePress(MouseState mouseArgs);
        
        void HandleTextEntered(TextHandleArgs textArgs);

        void HandleViewportResized(Viewport viewport);

        void HandleGraphicsReset(GraphicsDevice graphics);

        void Update(GameTime gameTime);

        void Init(GraphicsDevice graphics);

        void Render(GraphicsDevice graphics, GameTime gameTime);

        void BatchRender(SpriteBatch batch);
    }
}
