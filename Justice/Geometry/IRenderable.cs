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
    public abstract class IRenderable
    {
        /// <summary>
        /// Gets the instance's bounding box, used for rough pass culling
        /// </summary>
        public abstract BoundingBox RenderBounds { get; }
        /// <summary>
        /// Gets or sets whether the instance is visible and should be rendered
        /// </summary>
        public virtual bool IsVisible { get; set; } = true;
        /// <summary>
        /// Gets whether this renderable is partially transparent
        /// </summary>
        public virtual bool IsTransparent { get; } = false;
        
        /// <summary>
        /// Gets the texture for this renderable isntance
        /// </summary>
        public Texture2D Texture { get { return (Texture2D)MaterialManager.Instance[MaterialId].DiffuseTexture; } }

        /// <summary>
        /// Gets whether this renderable is a "pre-render" renderable, which will be rendered before all other renderables
        /// </summary>
        public virtual bool IsPreRendered { get; }

        /// <summary>
        /// Gets or sets the world transformation matrix for this renderable
        /// </summary>
        public virtual Matrix WorldTransform { get; set; } = Matrix.Identity;

        /// <summary>
        /// Gets or sets the material ID for this renderable instance
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Initializes this renderable instance
        /// </summary>
        /// <param name="graphics">The graphics device to use for intitialization</param>
        public abstract void Init(GraphicsDevice graphics);

        /// <summary>
        /// Gets whether this renderable should be rendered in the current draw call
        /// </summary>
        /// <param name="cameraFrustum">The camera frustum to check against</param>
        /// <returns>True if this instance should be rendered, false if otherwise</returns>
        public virtual bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return cameraFrustum.Intersects(RenderBounds);
        }

        public virtual void PreRender(GraphicsDevice graphics, ICamera camera) { }

        /// <summary>
        /// Renders this renderable instance
        /// </summary>
        /// <param name="graphics">The graphics device to use for rendering</param>
        /// <param name="matrices">The camera's transformation matrices</param>
        /// <param name="worldTransform">The world transformation to be applied to the renderable</param>
        public abstract void Render(GraphicsDevice graphics, CameraMatrices matrices);
    }
}
