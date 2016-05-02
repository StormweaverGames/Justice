using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justice.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Justice.UI
{
    public class InterfaceManger : IUserInterface
    {
        private UIRenderer myRenderer;
        private GraphicsDevice myGraphics;
        private SpriteBatch mySpriteBatch;

        private List<IUserInterface> myInterfaceElements;

        public bool IsInitialized
        {
            get { return true; }
            set { }
        }

        public InterfaceManger(GraphicsDevice graphics)
        {
            myGraphics = graphics;
                        
            myInterfaceElements = new List<IUserInterface>();

            myRenderer = new UIRenderer(myGraphics);
            mySpriteBatch = new SpriteBatch(myGraphics);
        }
        
        public void AddElement(IUserInterface element)
        {
            if (!element.IsInitialized)
            {
                element.Init(myGraphics);
                element.IsInitialized = true;
            }

            myInterfaceElements.Add(element);
        }

        public void HandleMousePress(MouseState mouseArgs)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].HandleMousePress(mouseArgs);
        }
        
        public void HandleTextEntered(TextHandleArgs textArgs)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].HandleTextEntered(textArgs);
        }

        public void HandleViewportResized(Viewport viewport)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].HandleViewportResized(viewport);
        }

        public void HandleGraphicsReset(GraphicsDevice graphics)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].HandleGraphicsReset(graphics);
        }

        public void Update(GameTime gameTime)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].Update(gameTime);
        }

        public void Init(GraphicsDevice graphics)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                if (!myInterfaceElements[index].IsInitialized)
                {
                    myInterfaceElements[index].Init(myGraphics);
                    myInterfaceElements[index].IsInitialized = true;
                }
        }

        public void Render(GraphicsDevice graphics, GameTime gameTime)
        {
            mySpriteBatch.Begin();
            BatchRender(mySpriteBatch);
            mySpriteBatch.End();

            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].Render(graphics, gameTime);
        }

        public void BatchRender(SpriteBatch batch)
        {
            for (int index = 0; index < myInterfaceElements.Count; index++)
                myInterfaceElements[index].BatchRender(batch);
        }
    }
}
