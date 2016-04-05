using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class SimpleCamera : ICamera
    {
        public static readonly Vector3 DEFAULT_POSTION = Vector3.Zero;
        public static readonly Vector3 DEFAULT_NORMAL = Vector3.UnitX;
        public static readonly Vector3 DEFAULT_UP = Vector3.UnitZ;

        public const float DEFAULT_FOV = 60.0f;
        public const float DEFAULT_ASPECT_RATIO = 16.0f / 9.0f;
        public const float DEFAULT_NEAR_FIELD = 0.1f;
        public const float DEFAULT_FAR_FIELD = 1000.0f;

        protected CameraMatrices myMatrices;

        protected float myFov;
        protected float myAspectRatio;
        protected float myNearField;
        protected float myFarField;

        protected Vector3 myPosition;
        protected Vector3 myNormal;
        protected Vector3 myUp;

        public CameraMatrices Matrices
        {
            get { return myMatrices; }
        }

        public Vector3 Position
        {
            get { return myPosition; }
            set { myPosition = value; }
        }
        public Vector3 Normal
        {
            get { return myNormal; }
            set
            {
                myNormal = value;
                myNormal.Normalize();
            }
        }
        public Vector3 Up
        {
            get { return myUp; }
            set { myUp = value; }
        }

        public SimpleCamera()
            : this(DEFAULT_POSTION, DEFAULT_NORMAL, DEFAULT_UP, DEFAULT_FOV, DEFAULT_ASPECT_RATIO, DEFAULT_NEAR_FIELD, DEFAULT_FAR_FIELD)
        {

        }

        public SimpleCamera(float fov, float aspectRatio, float nearField = 0.1f, float farField = 1000f)
            : this(DEFAULT_POSTION, DEFAULT_NORMAL, DEFAULT_UP, fov, aspectRatio, nearField, farField)
        {
        }

        public SimpleCamera(Vector3 position, Vector3 normal, Vector3 up, float fov, float aspectRatio, float nearField, float farField)
        {
            myFov = fov;
            myAspectRatio = aspectRatio;
            myNearField = nearField;
            myFarField = farField;

            myPosition = position;
            myNormal = normal;
            myUp = up;

            CalculateProjectionatrix();
            CalculateViewMatrix();
        }

        public void RotateAxis(Vector3 axis, float angle)
        {
            Matrix rotationMatrix = Matrix.CreateFromAxisAngle(axis, angle);
            Normal = Vector3.TransformNormal(Normal, rotationMatrix);
            Up = Vector3.TransformNormal(Up, rotationMatrix);
        }

        protected void CalculateProjectionatrix()
        {
            myMatrices.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(myFov), myAspectRatio, myNearField, myFarField);
        }

        protected void CalculateViewMatrix()
        {
            myMatrices.View = Matrix.CreateLookAt(myPosition, myPosition + myNormal, myUp);
        }

        public void InitFrame()
        {
            CalculateViewMatrix();
            CalculateProjectionatrix();
        }
    }
}
