using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public interface IEffectInterface
    {
        EffectTechnique Technique { get; }
        
        /// <summary>
        /// Invoked after the view and projection matrices are applied, this lets the effect prepare any per-frame parameters
        /// </summary>
        void CalculateFrameParams();

        /// <summary>
        /// Invoked before an instance is rendered, this lets effect re-calculate any parameters required for per-instance rendering
        /// </summary>
        void CalculateInstanceParams();

        void ApplyMaterial(EffectMaterial material);

        void SetWorldMatrix(Matrix value);

        void SetLocalMatrix(Matrix localTransform);

        void SetViewMatrix(Matrix value);

        void SetProjectionMatrix(Matrix value);

        void SetDiffuseTexture(Texture value);

        void SetParameter(string name, Matrix value);

        void SetParameter(string name, Matrix[] value);

        void SetParameter(string name, bool value);

        void SetParameter(string name, float value);

         void SetParameter(string name, float[] value);

        void SetParameter(string name, int value);

        void SetParameter(string name, Quaternion value);

        void SetParameter(string name, Texture value);

        void SetParameter(string name, Vector2 value);

        void SetParameter(string name, Vector2[] value);

        void SetParameter(string name, Vector3 value);

        void SetParameter(string name, Vector3[] value);

        void SetParameter(string name, Vector4 value);

        void SetParameter(string name, Vector4[] value);
    }
}
