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
    public class SimpleEntityController : IInputController
    {
        const float DEFAULT_ROTATION_SPEED = MathHelper.Pi;
        const float DEFUALT_MOVE_SPEED = 5.0f;

        private Entity myEntity;
        private SimpleCamera myCamera;
        private float myRotationSpeed;
        private float myMoveSpeed;

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

        public SimpleEntityController(Entity entity, SimpleCamera camera)
        {
            myEntity = entity;

            IsActive = true;

            myCamera = camera;

            myRotationSpeed = DEFAULT_ROTATION_SPEED;
            myMoveSpeed = DEFUALT_MOVE_SPEED;
        }
     
        public void Update(GameTime gameTime)
        {
            Vector3 velocityChange = Vector3.Zero;

            float hSpeedMultiplier = 1.0f;

            if (KeyboardManager.IsKeyDown(Keys.LeftShift))
                hSpeedMultiplier = 2.0f;

            if (KeyboardManager.IsKeyDown(Keys.S))
                velocityChange -= myCamera.Normal * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.W))
                velocityChange += myCamera.Normal * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.A))
                velocityChange += Vector3.Cross(myCamera.Up, myCamera.Normal) * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.D))
                velocityChange -= Vector3.Cross(myCamera.Up, myCamera.Normal) * myMoveSpeed;

            velocityChange *= hSpeedMultiplier;
            velocityChange.Z = 0;

            if (KeyboardManager.IsKeyDown(Keys.Space) && myEntity.Position.Z < 0.1f)
                velocityChange += Vector3.UnitZ * 10;

            myEntity.Velocity += velocityChange;

            if (KeyboardManager.IsKeyDown(Keys.Left))
                myCamera.RotateAxis(Vector3.UnitZ, myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Right))
                myCamera.RotateAxis(Vector3.UnitZ, -myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Up))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), -myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (KeyboardManager.IsKeyDown(Keys.Down))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), myRotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            myCamera.Position = myEntity.Position + new Vector3(0, 0, 2);
        }
    }
}
