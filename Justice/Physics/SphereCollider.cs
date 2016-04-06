using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Justice.Physics
{
    public class SphereCollider : ICollider
    {
        public BoundingSphere Sphere;
        
        public BoundingBox RoughBounds
        {
            get { return new BoundingBox(Sphere.Center - new Vector3(Sphere.Radius), Sphere.Center + new Vector3(Sphere.Radius)); }
        }

        public SphereCollider(BoundingSphere sphere)
        {
            Sphere = sphere;
        }

        public bool Intersects(OrientedBoundingBox value)
        {
            return value.Intersects(Sphere);
        }

        public bool Intersects(BoundingFrustum value)
        {
            return value.Intersects(Sphere);
        }

        public bool Intersects(ICollider value)
        {
            return value.Intersects(Sphere);
        }

        public bool Intersects(BoundingSphere value)
        {
            return Sphere.Intersects(value);
        }

        public bool Intersects(BoundingBox value)
        {
            return Sphere.Intersects(value);
        }

        public float? Intersects(Ray value)
        {
            return Sphere.Intersects(value);
        }
    }
}
