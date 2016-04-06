using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Physics
{
    public interface ICollider
    {
        Vector3? Intersects(Ray value);

        bool Intersects(BoundingBox value);

        bool Intersects(OrientedBoundingBox value);

        bool Intersects(BoundingSphere value);

        bool Intersects(BoundingFrustum value);

        bool Intersects(ICollider value);
    }
}
