using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Justice.Geometry;
using Justice.Tools;

namespace Justice.Gameplay
{
    public abstract class PhysicsEntity : Entity
    {
        protected BEPUphysics.Entities.Entity myPhysicsEntity;
                
        public override Vector3 Position
        {
            get { return myPhysicsEntity.Position.Convert(); }
            set { myPhysicsEntity.Position = value.Convert(); }
        }
        public Matrix Transformation
        {
            get { return myPhysicsEntity.WorldTransform.Convert(); }
            set { myPhysicsEntity.WorldTransform = value.Convert(); }
        }
        public Vector3 Velocity
        {
            get { return myPhysicsEntity.LinearVelocity.Convert(); }
            set { myPhysicsEntity.LinearVelocity = value.Convert(); }
        }
        public Vector3 AngularVelocity
        {
            get { return myPhysicsEntity.AngularVelocity.Convert(); }
            set { myPhysicsEntity.AngularVelocity = value.Convert(); }
        }
        public Vector3 Momentum
        {
            get { return myPhysicsEntity.LinearMomentum.Convert(); }
            set { myPhysicsEntity.LinearMomentum = value.Convert(); }
        }
        public Vector3 AngularMomentum
        {
            get { return myPhysicsEntity.AngularMomentum.Convert(); }
            set { myPhysicsEntity.AngularMomentum = value.Convert(); }
        }
        public float Mass
        {
            get { return myPhysicsEntity == null ? 0 : myPhysicsEntity.Mass; }
            set { myPhysicsEntity.Mass = value; }
        }

        protected PhysicsEntity()
        {
            myPhysicsEntity = null;
        }

        protected PhysicsEntity(BEPUphysics.Entities.Entity physicsEntity)
        {
            myPhysicsEntity = physicsEntity;
        }

        protected PhysicsEntity(BEPUphysics.Entities.Entity physicsEntity, IRenderable renderable) : base(renderable)
        {
            myPhysicsEntity = physicsEntity;
        }

        public virtual void AddToScene(BEPUphysics.Space space)
        {
            space.Add(myPhysicsEntity);
            IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (myRenderable != null)
                myRenderable.WorldTransform = myPhysicsEntity.WorldTransform.Convert();
        }

        public static implicit operator BEPUphysics.Entities.Entity (PhysicsEntity entity)
        {
            return entity.myPhysicsEntity;
        }
    }
}
