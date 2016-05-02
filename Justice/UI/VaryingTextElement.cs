using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public class VaryingTextElement : TextElement
    {
        protected Func<string> myTextSource;

        public VaryingTextElement(Func<string> textSource, Vector2 position) : base()
        {
            base.Position = position;

            myTextSource = textSource;
        }

        protected override void OnPreBatchRender()
        {
            base.OnPreRender();

            base.Text = myTextSource();
        }
    }
}
