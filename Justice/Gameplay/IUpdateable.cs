using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Gameplay
{
    public interface IUpdateable
    {
        bool IsActive { get; set; }

        void Update(GameTime gameTime);
    }
}
