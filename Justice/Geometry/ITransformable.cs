using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents an instance that can have a transformation applied to it
    /// </summary>
    public interface ITransformable
    {
        Matrix Transformation { get; set; }
    }
}
