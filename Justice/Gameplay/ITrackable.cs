using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Gameplay
{
    /// <summary>
    /// Represents an object with a position that can be tracked, say by a camera
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// Get's the instances postion
        /// </summary>
        Vector3 Position { get; }
    }
}
