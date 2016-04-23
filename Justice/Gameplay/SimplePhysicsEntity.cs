using BEPUphysics;
using Justice.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.Paths;
using Microsoft.Xna.Framework;

namespace Justice.Gameplay
{
    public class SimplePhysicsEntity : PhysicsEntity
    {
        public LinearInterpolationCurve3D FollowPath
        {
            get;
            set;
        }

        public SimplePhysicsEntity(BEPUphysics.Entities.Entity physicsObject, float mass)
        {
            myPhysicsEntity = physicsObject;
            Mass = mass;
        }

        public SimplePhysicsEntity(BEPUphysics.Entities.Entity physicsObject, float mass, ITransformable transformTarget)
        {
            myPhysicsEntity = physicsObject;
            Mass = mass;
            myTransformTarget = transformTarget;
            
            myPhysicsEntity.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;

            if (transformTarget is IRenderable)
                myRenderable = transformTarget as IRenderable;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (FollowPath != null)
                myPhysicsEntity.Position = FollowPath.Evaluate(gameTime.TotalGameTime.TotalSeconds);
        }
    }
}
