using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justice.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Justice.UI
{
    public class ScreenElement : IUserInterface
    {
        protected Rectangle myBounds;

        public bool IsInitialized
        {
            get;
            set;
        }

        public virtual Rectangle Bounds
        {
            get { return myBounds; }
            set
            {
                OnBoundsUpdating(myBounds, value);
                myBounds = value;
            }
        }

        public void HandleGraphicsReset(GraphicsDevice graphics)
        {
            OnGraphicsReset(graphics);
        }
        
        public void HandleMousePress(MouseState mouseArgs)
        {
            OnMousePressed(mouseArgs);

            if (Bounds.Contains(mouseArgs.Position))
                OnMouseClicked(mouseArgs);
        }

        public void HandleTextEntered(TextHandleArgs textArgs)
        {
            if (!textArgs.IsHandled)
                OnTextEntered(textArgs);
        }

        public void HandleViewportResized(Viewport viewport)
        {
            OnViewportResized(viewport);
        }

        public void Init(GraphicsDevice graphics)
        {
            // TODO: Initialize event here

            OnInit(graphics);

            IsInitialized = true;
        }

        public void Render(GraphicsDevice graphics, GameTime gameTime)
        {
            OnPreRender();
            OnRender(graphics, gameTime);
            OnPostRender();
        }

        public void BatchRender(SpriteBatch batch)
        {
            OnPreBatchRender();
            OnBatchRender(batch);
            OnPostBatchRender();
        }

        public void Update(GameTime gameTime)
        {
            OnUpdating(gameTime);
        }

        #region Protected Methods

        protected virtual void OnUpdating(GameTime gameTime) { }

        protected virtual void OnPreRender() { }

        protected virtual void OnRender(GraphicsDevice graphics, GameTime gameTime) { }

        protected virtual void OnPostRender() { }

        protected virtual void OnPreBatchRender() { }

        protected virtual void OnBatchRender(SpriteBatch batch) { }

        protected virtual void OnPostBatchRender() { }
        
        protected virtual void OnInit(GraphicsDevice graphics) { }

        protected virtual void OnViewportResized(Viewport viewport) { }

        protected virtual void OnTextEntered(TextHandleArgs textArgs) { }

        /// <summary>
        /// Invoked when the mouse is pressed. The mouse does not need to be over the control for this method
        /// to be invoked
        /// </summary>
        /// <param name="mouseArgs">The current state of the mouse, including the buttons pressed</param>
        protected virtual void OnMousePressed(MouseState mouseArgs) { }

        /// <summary>
        /// Invoked when the mouse is pressed while hoving over this element.
        /// </summary>
        /// <param name="mouseArgs">The current state of the mouse, including the buttons pressed</param>
        protected virtual void OnMouseClicked(MouseState mouseArgs) { }

        protected virtual void OnBoundsUpdating(Rectangle myBounds, Rectangle value) { }

        protected virtual void OnGraphicsReset(GraphicsDevice graphics) { }


        #endregion
    }
}
