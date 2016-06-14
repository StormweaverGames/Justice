using BEPUphysics;
using Justice.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.Paths;
using Microsoft.Xna.Framework;
using Justice.Tools;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Gameplay
{
    public class SimplePhysicsEntity : PhysicsEntity
    {
        public LinearInterpolationCurve3D FollowPath
        {
            get;
            set;
        }

        public override Matrix WorldTransform
        {
            get
            {
                if (myRenderable != null)
                    return myRenderable.WorldTransform;
                else
                    return Matrix.Identity;
            }
            set
            {
                myPhysicsEntity.WorldTransform = value.Convert();
            }
        }

        public SimplePhysicsEntity(BEPUphysics.Entities.Entity physicsObject, float mass)
        {
            myPhysicsEntity = physicsObject;
            Mass = mass;
        }

        public SimplePhysicsEntity(BEPUphysics.Entities.Entity physicsObject, float mass, IRenderable renderable)
        {
            myPhysicsEntity = physicsObject;
            Mass = mass;
            myRenderable = renderable;
            
            myPhysicsEntity.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (myRenderable != null && myPhysicsEntity != null)
                myRenderable.WorldTransform = myPhysicsEntity.WorldTransform.Convert();
            
            if (FollowPath != null)
                myPhysicsEntity.Position = FollowPath.Evaluate(gameTime.TotalGameTime.TotalSeconds);
        }
    }
}
