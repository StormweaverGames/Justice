using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public class BasicEffectInterface : IEffectInterface
    {
        private Effect myEffectInstance;
        private EffectTechnique myTechnique;

        private EffectParameter myWorldParameter;
        private EffectParameter myInverseWorldParameter;
        private EffectParameter myViewParameter;
        private EffectParameter myEyePositionParam;
        private EffectParameter myProjectionParameter;
        private EffectParameter myDiffuseParameter;

        private EffectParameter myWVPParameter;

        private Matrix myWorld;
        private Matrix myView;
        private Matrix myProjection;
        
        public EffectTechnique Technique
        {
            get { return myTechnique; }
        }
        
        public BasicEffectInterface(Effect effect, string technique)
        {
            myEffectInstance = effect;
            myTechnique = effect.Techniques[technique];
        }

        public void Init(string worldParam, string viewParam, string projectionParam, string diffuseParam)
        {
            myWorldParameter = myEffectInstance.Parameters[worldParam];
            myViewParameter = myEffectInstance.Parameters[viewParam];
            myProjectionParameter = myEffectInstance.Parameters[projectionParam];

            myDiffuseParameter = myEffectInstance.Parameters[diffuseParam];
        }

        public void InitBasicEffect()
        {
            myWVPParameter = myEffectInstance.Parameters["WorldViewProj"];
            myEyePositionParam = myEffectInstance.Parameters["EyePosition"];
            myInverseWorldParameter = myEffectInstance.Parameters["WorldInverseTranspose"];
            myDiffuseParameter = myEffectInstance.Parameters["Texture"];
        }

        public void CalculateFrameParams()
        {
            if (myEffectInstance is BasicEffect)
            {
                
                Matrix viewInverse;

                Matrix.Invert(ref myView, out viewInverse);

                myEyePositionParam.SetValue(viewInverse.Translation);
            }
        }

        Matrix __worldView;
        Matrix __wvp;
        public void CalculateInstanceParams()
        {
            if (myWVPParameter != null)
            {
                Matrix.Multiply(ref myWorld, ref myView, out __worldView);
                Matrix.Multiply(ref __worldView, ref myProjection, out __wvp);

                myWVPParameter.SetValue(__wvp);
            }

            if (myInverseWorldParameter != null)
            {
                Matrix worldTranspose;
                Matrix worldInverseTranspose;

                Matrix.Invert(ref myWorld, out worldTranspose);
                Matrix.Transpose(ref worldTranspose, out worldInverseTranspose);

                myWorldParameter?.SetValue(myWorld);
                myInverseWorldParameter?.SetValue(worldInverseTranspose);
            }
        }

        public void SetWorldMatrix(Matrix value)
        {
            if (myWorldParameter != null)
                myWorldParameter.SetValue(value);
            else
                myWorld = value;
        }

        public void ApplyMaterial(EffectMaterial material)
        {
            SetDiffuseTexture(material.DiffuseTexture);
        }

        public void SetViewMatrix(Matrix value)
        {
            if (myViewParameter != null)
                myViewParameter.SetValue(value);
            else
                myView = value;
        }

        public void SetProjectionMatrix(Matrix value)
        {
            if (myProjectionParameter != null)
                myProjectionParameter.SetValue(value);
            else
                myProjection = value;
        }

        public void SetDiffuseTexture(Texture value)
        {
            myDiffuseParameter?.SetValue(value);
        }

        public void SetParameter(string name, Matrix value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Matrix[] value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, bool value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, float value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, float[] value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, int value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Quaternion value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Texture value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector2 value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector2[] value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector3 value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector3[] value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector4 value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }

        public void SetParameter(string name, Vector4[] value)
        {
            myEffectInstance.Parameters[name].SetValue(value);
        }
    }
}
