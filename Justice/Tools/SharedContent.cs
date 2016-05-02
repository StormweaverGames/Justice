using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    public static class SharedContent
    {
        private static ContentManager myContentManager;

        public static void Init(ContentManager content)
        {
            myContentManager = content;
        }

        public static T Load<T>(string fileName)
        {
            return myContentManager.Load<T>(fileName);
        }
    }
}
