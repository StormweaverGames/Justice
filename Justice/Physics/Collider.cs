using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Justice.Physics
{
    public class Collider : ICollider
    {
        private ICollider[] myColliders;
        private BoundingBox myBounds;

        public BoundingBox RoughBounds
        {
            get { return myBounds; }
        }

        public Collider()
        {
            myColliders = new ICollider[0];
            myBounds = new BoundingBox(new Vector3(0), new Vector3(0));
        }

        public void Add(ICollider collider)
        {
            Array.Resize(ref myColliders, myColliders.Length + 1);
            myColliders[myColliders.Length - 1] = collider;

            myBounds.Min = Vector3.Min(myBounds.Min, collider.RoughBounds.Min);
            myBounds.Max = Vector3.Max(myBounds.Max, collider.RoughBounds.Max);
        }

        public bool Intersects(OrientedBoundingBox value)
        {
            for (int index = 0; index < myColliders.Length; index++)
                if (myColliders[index].Intersects(value))
                    return true;

            return false;
        }

        public bool Intersects(BoundingFrustum value)
        {
            for (int index = 0; index < myColliders.Length; index++)
                if (myColliders[index].Intersects(value))
                    return true;

            return false;
        }

        public bool Intersects(ICollider value)
        {
            for (int index = 0; index < myColliders.Length; index++)
                if (myColliders[index].Intersects(value))
                    return true;

            return false;
        }

        public bool Intersects(BoundingSphere value)
        {
            for (int index = 0; index < myColliders.Length; index++)
                if (myColliders[index].Intersects(value))
                    return true;

            return false;
        }

        public bool Intersects(BoundingBox value)
        {
            for (int index = 0; index < myColliders.Length; index++)
                if (myColliders[index].Intersects(value))
                    return true;

            return false;
        }

        public float? Intersects(Ray value)
        {
            float? distance = null;

            for (int index = 0; index < myColliders.Length; index++)
            {
                float? d = myColliders[index].Intersects(value);
                if (d.HasValue)
                    distance = distance == null ? d : d.Value < distance.Value ? d.Value : distance.Value;
            }

            return distance;
        }
    }
}
