using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Controls
{
    public struct KeyEventArgs
    {
        public Keys Key;
        public ButtonState State;

        public KeyEventArgs(Keys key, ButtonState state)
        {
            Key = key;
            State = state;
        }
    }
}
