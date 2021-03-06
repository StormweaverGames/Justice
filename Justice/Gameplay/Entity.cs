﻿using Justice.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Justice.Tools;

namespace Justice.Gameplay
{
    public abstract class Entity : IRenderable, ITrackable, IUpdateable
    {
        protected IRenderable myRenderable;
        
        public override BoundingBox RenderBounds
        {
            get
            {
                return myRenderable.RenderBounds;
            }
        }
        
        public override bool IsVisible
        {
            get { return myRenderable != null && myRenderable.IsVisible; }
        }

        public abstract Vector3 Position
        {
            get;
            set;
        }
        
        public bool IsActive
        {
            get;
            set;
        }
        public override Matrix WorldTransform
        {
            get
            {
                return myRenderable != null ? myRenderable.WorldTransform : Matrix.Identity;
            }
            set
            {
                if (myRenderable != null)
                    myRenderable.WorldTransform = value;
            }
        }

        public override Matrix LocalTransform
        {
            get
            {
                return myRenderable != null ? myRenderable.LocalTransform : Matrix.Identity;
            }

            set
            {
                if (myRenderable != null)
                    myRenderable.LocalTransform = value;
            }
        }

        protected Entity()
        {
            myRenderable = null;
        }

        protected Entity(IRenderable renderer)
        {
            myRenderable = renderer;
            IsActive = true;
        }

        public override void Init(GraphicsDevice graphics)
        {
            myRenderable?.Init(graphics);
        }
        
        public override void RenderShadow(GraphicsDevice graphics, CameraMatrices matrices)
        {
            myRenderable?.RenderShadow(graphics, matrices);
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            myRenderable?.Render(graphics, matrices);
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return myRenderable != null ? myRenderable.ShouldRender(cameraFrustum) : false;
        }

        public abstract void Update(GameTime gameTime);
    }
}
