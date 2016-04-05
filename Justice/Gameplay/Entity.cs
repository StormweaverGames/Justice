using Justice.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Gameplay
{
    public class Entity : IRenderable, ITrackable, IUpdateable
    {
        protected IRenderable myRenderable;
        protected Vector3 myVelocity;

        public BoundingBox Bounds
        {
            get { return myRenderable.Bounds; }
        }

        public bool IsVisible
        {
            get { return myRenderable.IsVisible; }
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 Velocity
        {
            get { return myVelocity; }
            set { myVelocity = value; }
        }
        public float Mass
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public Entity(IRenderable renderer)
        {
            myRenderable = renderer;
            IsActive = true;

            Mass = 5.0f;
        }

        public void Init(GraphicsDevice graphics)
        {
            if (myRenderable != null)
                myRenderable.Init(graphics);
        }

        public void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            if (myRenderable != null)
                myRenderable.Render(graphics, matrices);
        }

        public bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return myRenderable != null ? myRenderable.ShouldRender(cameraFrustum) : false;
        }

        public void Update(GameTime gameTime)
        {
            Position += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            myVelocity.X *= 0.5f;
            myVelocity.Y *= 0.5f;

            Velocity += new Vector3(0, 0, (float)(-9.81 * gameTime.ElapsedGameTime.TotalSeconds));

            if (Position.Z < 0)
                Position = new Vector3(Position.X, Position.Y, 0);
        }
    }
}
