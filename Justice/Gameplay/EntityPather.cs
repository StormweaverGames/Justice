using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using BEPUphysics.Paths.PathFollowing;
using BEPUphysics.Paths;
using BEPUphysics;

namespace Justice.Gameplay
{
    public class EntityPather : IUpdateable
    {
        PhysicsEntity myEntity;
        EntityMover myEntityMover;
        EntityRotator myEntityRotator;

        Path<BEPUutilities.Vector3> myPositionPath;
        Path<BEPUutilities.Quaternion> myRotationPath;

        public bool IsActive
        {
            get;
            set;
        }
        protected EntityPather()
        {
            IsActive = true;
        }

        public EntityPather(PhysicsEntity entity) : this()
        {
            myEntity = entity;

            myEntityMover = new EntityMover(myEntity);
            myEntityRotator = new EntityRotator(myEntity);
        }

        public void AddToScene(Space space)
        {
            space.Add(myEntityMover);
            space.Add(myEntityRotator);
        }

        public void SetPositionPath(Path<BEPUutilities.Vector3> path)
        {
            myPositionPath = path;
        }

        public void SetRotationPath(Path<BEPUutilities.Quaternion> path)
        {
            myRotationPath = path;
        }

        public void Update(GameTime gameTime)
        {
            if (myPositionPath != null)
                myEntityMover.TargetPosition = myPositionPath.Evaluate(gameTime.TotalGameTime.TotalSeconds);

            if (myRotationPath != null)
                myEntityRotator.TargetOrientation = myRotationPath.Evaluate(gameTime.TotalGameTime.TotalSeconds);
        }
    }
}
