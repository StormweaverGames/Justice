using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Justice.Physics
{
    public class AABBCollider : ICollider
    {
        public BoundingBox Bounds;

        public BoundingBox RoughBounds
        {
            get { return Bounds; }
        }

        public AABBCollider(BoundingBox bounds)
        {
            Bounds = bounds;
        }

        public AABBCollider(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            Bounds = new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
        }

        public bool Intersects(OrientedBoundingBox value)
        {
            return value.Intersects(Bounds);
        }

        public bool Intersects(BoundingFrustum value)
        {
            return Bounds.Intersects(value);
        }

        public bool Intersects(ICollider value)
        {
            return value.Intersects(Bounds);
        }

        public bool Intersects(BoundingSphere value)
        {
            return Bounds.Intersects(value);
        }

        public bool Intersects(BoundingBox value)
        {
            return Bounds.Intersects(value);
        }

        public float? Intersects(Ray value)
        {
            return Bounds.Intersects(value);
        }
    }
}
