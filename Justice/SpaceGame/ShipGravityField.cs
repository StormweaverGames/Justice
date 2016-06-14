using BEPUphysics.UpdateableSystems.ForceFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEPUphysics.Entities;
using BEPUutilities;
using Justice.Tools;

namespace Justice.SpaceGame
{
    public class ShipGravityField : ForceField
    {
        private float myMultiplier;
        private Ship myParentShip;

        public float Multiplier
        {
            get { return myMultiplier; }
            set { myMultiplier = value; }
        }

        public ShipGravityField(Ship ship, ForceFieldShape shape) : base(shape)
        {
            // LOL ship shape XD
            myParentShip = ship;
            myMultiplier = 9.81f;
        }

        protected override void PreUpdate()
        {
            base.PreUpdate();

            //Shape.
        }

        protected override void CalculateImpulse(Entity e, float dt, out Vector3 impulse)
        {
            if (e.Tag != myParentShip)
            {
                impulse = myParentShip.WorldTransform.Forward.Convert();
            }
            else
                impulse = Vector3.Zero;
        }
    }
}
