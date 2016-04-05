using Microsoft.Xna.Framework;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents a camera's view an projection matrices
    /// </summary>
    public struct CameraMatrices
    {
        /// <summary>
        /// Stores the view matrix
        /// </summary>
        private Matrix myView;
        /// <summary>
        /// Stores the projection matrix
        /// </summary>
        private Matrix myProjection;

        /// <summary>
        /// Gets or sets this matrice's view matrix
        /// </summary>
        public Matrix View
        {
            get { return myView; }
            set { myView = value; }
        }
        /// <summary>
        /// Gets or sets the camera's projection matrix
        /// </summary>
        public Matrix Projection
        {
            get { return myProjection; }
            set { myProjection = value; }
        }
        /// <summary>
        /// Gets the camera's combined view projection matrix
        /// </summary>
        public Matrix ViewProj
        {
            get { return (myView * myProjection); }
        }

        /// <summary>
        /// Makes a new camera matrix set
        /// </summary>
        /// <param name="view">The view matrix</param>
        /// <param name="projection">The projection matrix</param>
        public CameraMatrices(Matrix view, Matrix projection)
        {
            myView = view;
            myProjection = projection;
        }
    }
}
