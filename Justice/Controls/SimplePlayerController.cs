using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Justice.Geometry;
using Microsoft.Xna.Framework.Input;
using Justice.Gameplay;

namespace Justice.Controls
{
    public class SimplePlayerController : IInputController
    {
        const float DEFAULT_ROTATION_SPEED = MathHelper.Pi;

        private Player myPlayer;
        private SimpleCamera myCamera;
        private float myRotationSpeed;

        public bool IsActive
        {
            get;
            set;
        }
        public bool HasMouseControl
        {
            get;
            set;
        }
        public bool ToggleCrouch
        {
            get;
            set;
        }
        public bool ToggleProne
        {
            get;
            set;
        }

        public SimplePlayerController(Player player, SimpleCamera camera)
        {
            myPlayer = player;

            IsActive = true;
            
            ToggleCrouch = true;

            myCamera = camera;

            myRotationSpeed = DEFAULT_ROTATION_SPEED;
        }
     
        public void Update(GameTime gameTime)
        {
            Vector2 velocityChange = Vector2.Zero;

            myPlayer.IsSprinting = KeyboardManager.IsKeyDown(Keys.LeftShift);

            if (KeyboardManager.IsKeyDown(Keys.S))
                velocityChange.Y += -1;
            if (KeyboardManager.IsKeyDown(Keys.W))
                velocityChange.Y += 1;
            if (KeyboardManager.IsKeyDown(Keys.A))
                velocityChange.X += -1;
            if (KeyboardManager.IsKeyDown(Keys.D))
                velocityChange.X += 1;
            
            myPlayer.RequestMovement(velocityChange);

            if (KeyboardManager.IsKeyPressed(Keys.Space))
                myPlayer.Jump();
            
            if (KeyboardManager.IsKeyDown(Keys.Left))
                myCamera.RotateAxis(Vector3.UnitZ, myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Right))
                myCamera.RotateAxis(Vector3.UnitZ, -myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Up))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), -myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Down))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (ToggleCrouch)
            {
                if (KeyboardManager.IsKeyPressed(Keys.LeftControl) && myPlayer.Stance == BEPUphysics.Character.Stance.Standing)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Crouching);
                else if (KeyboardManager.IsKeyPressed(Keys.LeftControl) && myPlayer.Stance == BEPUphysics.Character.Stance.Crouching)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Standing);
            }
            else
            {
                if (KeyboardManager.IsKeyDown(Keys.LeftControl) && myPlayer.Stance == BEPUphysics.Character.Stance.Standing)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Crouching);
                else if (KeyboardManager.IsKeyUp(Keys.LeftControl) && myPlayer.Stance == BEPUphysics.Character.Stance.Crouching)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Standing);
            } 

            if (ToggleProne)
            {
                if (KeyboardManager.IsKeyPressed(Keys.Z) && myPlayer.Stance == BEPUphysics.Character.Stance.Crouching)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Prone);
                else if (KeyboardManager.IsKeyPressed(Keys.Z) && myPlayer.Stance == BEPUphysics.Character.Stance.Crouching)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Crouching);
            }
            else
            {
                if (KeyboardManager.IsKeyDown(Keys.Z) && myPlayer.Stance == BEPUphysics.Character.Stance.Crouching)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Prone);
                else if (KeyboardManager.IsKeyUp(Keys.Z) && myPlayer.Stance == BEPUphysics.Character.Stance.Prone)
                    myPlayer.RequestStance(BEPUphysics.Character.Stance.Crouching);
            }

            myPlayer.SetViewDirection(myCamera.Normal);

            myCamera.Position = myPlayer.Position + new Vector3(0, 0, myPlayer.CurrentHeight);
        }
    }
}
