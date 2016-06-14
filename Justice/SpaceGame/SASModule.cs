using Justice.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Justice.SpaceGame
{
    public class SASModule : Justice.Gameplay.IUpdateable
    {
        private Ship myShip;

        public SASModule(Ship ship)
        {
            myShip = ship;
            IsActive = false;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public Vector3 GravityCompensation
        {
            get;
            set;
        }

        public void Update(GameTime gameTime)
        {
            Vector3 movement = myShip.Velocity * 2.0f;
            Vector3 rotation = myShip.AngularVelocity;

            myShip.ApplyRelativeImpulse(Vector3.Zero, -movement);
            myShip.ApplyAngularImpulse(-rotation * 0.1f);
        }
    }
}
