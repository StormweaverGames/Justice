using Microsoft.Xna.Framework;
using Justice.Gameplay;

namespace Justice.Controls
{
    /// <summary>
    /// Represents an input controller for an entity
    /// </summary>
    public interface IInputController : Gameplay.IUpdateable
    {
        /// <summary>
        /// Gets or sets whether this camera controller has control of the mouse
        /// </summary>
        bool HasMouseControl { get; set; }
    }
}
