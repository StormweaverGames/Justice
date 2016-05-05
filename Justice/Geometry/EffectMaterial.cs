using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public struct EffectMaterial
    {
        public string Name
        { get; private set; }

        public Texture DiffuseTexture;
        public SamplerState DiffuseSampler;
        
        public EffectMaterial(string name, Texture diffuseTexture)
        {
            Name = name;
            DiffuseTexture = diffuseTexture;
            DiffuseSampler = SamplerState.LinearClamp;
        }

        public override bool Equals(object obj)
        {
            return obj is EffectMaterial && ((EffectMaterial)obj) == this;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            if (DiffuseTexture != null)
                hash += 31 * DiffuseTexture.GetHashCode();
             
            return hash;
        }

        public static bool operator ==(EffectMaterial left, EffectMaterial right)
        {
            return left.DiffuseTexture == right.DiffuseTexture;
        }

        public static bool operator !=(EffectMaterial left, EffectMaterial right)
        {
            return left.DiffuseTexture != right.DiffuseTexture;
        }
    }
}
