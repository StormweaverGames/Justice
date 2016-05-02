using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public struct TextHandleArgs
    {
        public char Character;
        public bool IsHandled;

        public TextHandleArgs(char character)
        {
            Character = character;
            IsHandled = false;
        }
    }
}
