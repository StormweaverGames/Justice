using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents an instance that can be drawn
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets the instance's bounding box, used for rough pass culling
        /// </summary>
        BoundingBox Bounds { get; }
        /// <summary>
        /// Gets whether the instance is visible and should be rendered
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Initializes this renderable instance
        /// </summary>
        /// <param name="graphics">The graphics device to use for intitialization</param>
        void Init(GraphicsDevice graphics);

        /// <summary>
        /// Gets whether this renderable should be rendered in the current draw call
        /// </summary>
        /// <param name="cameraFrustum">The camera frustum to check against</param>
        /// <returns>True if this instance should be rendered, false if otherwise</returns>
        bool ShouldRender(BoundingFrustum cameraFrustum);

        /// <summary>
        /// Renders this renderable instance
        /// </summary>
        /// <param name="graphics">The graphics device to use for rendering</param>
        /// <param name="matrices">The camera's transformation matrices</param>
        void Render(GraphicsDevice graphics, CameraMatrices matrices);
    }
}
