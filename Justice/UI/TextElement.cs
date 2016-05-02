using System;
using Justice.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.UI
{
    public class TextElement : ScreenElement
    {
        private SpriteFont myFont;

        private string myText;
        private Color myColor;

        public virtual Vector2 Position { get; set; }
        public virtual string Text
        {
            get { return myText; }
            set
            {
                myText = value;

                if (AutoSize)
                    CalculateBounds();
            }
        }

        public Color TextColor
        {
            get { return myColor; }
            set { myColor = value; }
        }

        public bool AutoSize
        {
            get;
            set;
        } = true;

        public TextElement()
        {
            myFont = SharedContent.Load<SpriteFont>(ResourceNames.DefaultFont);
            myColor = Color.Black;
        }

        public TextElement(SpriteFont font)
        {
            myFont = font;
            myColor = Color.Black;
        }

        private void CalculateBounds()
        {
            if (!string.IsNullOrWhiteSpace(myText))
            {
                Vector2 size = myFont.MeasureString(myText);

                myBounds = new Rectangle((int)Position.X, (int)Position.Y, (int)size.X + 1, (int)size.Y + 1);
            }
        }

        protected override void OnBatchRender(SpriteBatch batch)
        {
            if (!string.IsNullOrWhiteSpace(myText))
            {
                batch.DrawString(myFont, myText, Position, myColor);
            }
        }

        protected override void OnBoundsUpdating(Rectangle myBounds, Rectangle value)
        {
            Position = new Vector2(myBounds.Left, myBounds.Top);
        }
    }
}