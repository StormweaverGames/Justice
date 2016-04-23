using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Justice.Geometry;
using Microsoft.Xna.Framework.Input;

namespace Justice.Controls
{
    public class SimpleCameraController : IInputController
    {
        const float DEFAULT_ROTATION_SPEED = 0.05f;
        const float DEFUALT_MOVE_SPEED = 1.0f;

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

        public SimpleCameraController(SimpleCamera camera)
        {
            myCamera = camera;

            IsActive = true;
            

            myRotationSpeed = DEFAULT_ROTATION_SPEED;
            myMoveSpeed = DEFUALT_MOVE_SPEED;
        }
     
        public void Update(GameTime gameTime)
        {
            if (KeyboardManager.IsKeyDown(Keys.S))
                myCamera.Position -= myCamera.Normal * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.W))
                myCamera.Position += myCamera.Normal * myMoveSpeed;

            if (KeyboardManager.IsKeyDown(Keys.A))
                myCamera.Position += Vector3.Cross(myCamera.Up, myCamera.Normal) * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.D))
                myCamera.Position -= Vector3.Cross(myCamera.Up, myCamera.Normal) * myMoveSpeed;

            if (KeyboardManager.IsKeyDown(Keys.Space))
                myCamera.Position += myCamera.Up * myMoveSpeed;
            if (KeyboardManager.IsKeyDown(Keys.LeftControl))
                myCamera.Position -= myCamera.Up * myMoveSpeed;
            
            if (KeyboardManager.IsKeyDown(Keys.Q))
                myCamera.RotateAxis(myCamera.Normal, -myRotationSpeed);
            if (KeyboardManager.IsKeyDown(Keys.E))
                myCamera.RotateAxis(myCamera.Normal, myRotationSpeed);
            if (KeyboardManager.IsKeyDown(Keys.Left))
                myCamera.RotateAxis(myCamera.Up, myRotationSpeed);
            if (KeyboardManager.IsKeyDown(Keys.Right))
                myCamera.RotateAxis(myCamera.Up, -myRotationSpeed);
            if (KeyboardManager.IsKeyDown(Keys.Up))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), -myRotationSpeed);
            if (KeyboardManager.IsKeyDown(Keys.Down))
                myCamera.RotateAxis(Vector3.Cross(myCamera.Up, myCamera.Normal), myRotationSpeed);
        }
    }
}
