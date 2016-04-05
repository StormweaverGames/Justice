using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents a camera that can be used to render a scene
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Gets the camera's view and projection matrices
        /// </summary>
        CameraMatrices Matrices { get; }

        /// <summary>
        /// Prepares the camera to render the current frame
        /// </summary>
        void InitFrame();
    }
}
