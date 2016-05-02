using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public class ElementMover : ScreenElement
    {
        protected Func<ScreenElement, Vector2> myPositionSource;
        protected ScreenElement myTarget;

        public ElementMover(Func<ScreenElement, Vector2> positionSource, ScreenElement target) : base()
        {
            myPositionSource = positionSource;
            myTarget = target;
        }

        protected override void OnPreBatchRender()
        {
            OnPreRender();
        }

        protected override void OnPreRender()
        {
            base.OnPreRender();

            Vector2 position = myPositionSource(myTarget);
            myTarget.Bounds = new Rectangle((int)position.X, (int)position.Y, myTarget.Bounds.Width, myTarget.Bounds.Y);
        }
    }
}
