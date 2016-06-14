using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justice.Geometry;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Character;
using BEPUphysics;
using Justice.Tools;

namespace Justice.Gameplay
{
    public class Player : PhysicsEntity
    {
        public const float SPRINT_SPEED = 20.0f;
        public const float WALK_SPEED = 10.0f;
        public const float CROUCH_SPEED = 4.0f;
        public const float CRAWL_SPEED = 1f;

        public const float STAND_HEIGHT = 1.8f;
        public const float CROUCH_HEIGHT = 0.8f;
        public const float PRONE_HEIGHT = 0.3f;

        public const float PLAYER_WEIGHT = 90.0f;

        CharacterController myController;
        Space mySpace;

        protected bool isSprinting;
        public bool IsSprinting
        {
            get { return isSprinting; }
            set
            {
                isSprinting = value && (Stance != Stance.Crouching && Stance != Stance.Prone);

                if (isSprinting)
                    myController.StandingSpeed = SPRINT_SPEED;
                else
                    myController.StandingSpeed = WALK_SPEED;
            }
        }

        public float CurrentHeight
        {
            get { return Stance == Stance.Standing ? STAND_HEIGHT : Stance == Stance.Crouching ? CROUCH_HEIGHT : PRONE_HEIGHT; }
        }

        public Stance Stance
        {
            get { return myController.StanceManager.CurrentStance; }
        }
        
        public Player(Vector3 position)
        {
            myController = new CharacterController();
            myController.Body.Mass = PLAYER_WEIGHT;
            myController.Body.Position = position.Convert();
            myController.Down = new BEPUutilities.Vector3(0, 0, -1);
            myController.BodyRadius = 1.0f;
            myController.StandingSpeed = WALK_SPEED;
            myController.CrouchingSpeed = CROUCH_SPEED;
            myController.ProneSpeed = CRAWL_SPEED;
            myController.HorizontalMotionConstraint.MovementMode = MovementMode.Floating;
            myController.JumpSpeed = 15;
            myController.TractionForce = myController.TractionForce * 9.0f;
            myController.SlidingForce = myController.SlidingForce * 9.0f;
            myController.AirForce = myController.AirForce * 9.0f;

            myController.StanceManager.StandingHeight = STAND_HEIGHT;
            myController.StanceManager.CrouchingHeight = CROUCH_HEIGHT;
            myController.StanceManager.ProneHeight = PRONE_HEIGHT;

            myController.ViewDirection = new BEPUutilities.Vector3(1, 0, 0);
            myController.IsUpdating = true;

            myPhysicsEntity = myController.Body;
        }

        public override void AddToScene(Space space)
        {
            space.Add(myController);
            mySpace = space;
        }

        BEPUutilities.Ray __myRayCastRay;
        RayCastResult __myRayCastResult;
        public bool RayCast(float length)
        {
            if (mySpace != null)
            {
                __myRayCastRay.Position = (Position + new Vector3(0, 0, CurrentHeight)).Convert();
                __myRayCastRay.Direction = myController.ViewDirection;

                return mySpace.RayCast(__myRayCastRay, length, X => X != myController.Body.CollisionInformation, out __myRayCastResult);
            }
            else
                return false;
        }

        public void Jump()
        {
            myController.Jump();
        }

        public void RequestStance(Stance stance)
        {
            myController.StanceManager.DesiredStance = stance;
        }
        
        public void RequestMovement(Vector2 direction)
        {
            myController.HorizontalMotionConstraint.MovementDirection = direction.Convert();
        }

        public void SetViewDirection(Vector3 normal)
        {
            myController.ViewDirection = normal.Convert();
        }
    }
}
