using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    /// <summary>
    /// Represents a single instance of a renderable model
    /// </summary>
    public class ModelInstance : IRenderable
    {
        protected Model myModel;
        protected Matrix[] myBoneTransformations;
        protected BoundingBox myBounds;
        protected Matrix myWorldTransform;
        protected Texture2D myTexture;

        /// <summary>
        /// Gets or sets the transformation matrix for this model instance
        /// </summary>
        public override Matrix WorldTransform
        {
            get { return myWorldTransform; }
            set
            {
                myWorldTransform = value;
                UpdateBoundingBox();
            }
        }
        /// <summary>
        /// Gets this model instance's bounds
        /// </summary>
        public override BoundingBox RenderBounds
        {
            get { return myBounds; }
        }
        
        /// <summary>
        /// Creates a new model instance from the given model
        /// </summary>
        /// <param name="model">The model to create an instnace of</param>
        public ModelInstance(Model model)
        {
            myModel = model;
            myBoneTransformations = new Matrix[model.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(myBoneTransformations);

            myBounds = new BoundingBox();
            UpdateBoundingBox();
        }

        public void SetTexture(Texture2D texture)
        {
            myTexture = texture;
        }

        /// <summary>
        /// Updates this model instnace's bounding box
        /// </summary>
        private void UpdateBoundingBox()
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            for(int index = 0; index < myModel.Meshes.Count; index ++)
            {
                ModelMesh mesh = myModel.Meshes[index];

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in model space w/ bone transformations applied
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), myModel.Bones[mesh.Name].ModelTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            
            myBounds.Min = Vector3.Transform(min, myWorldTransform);
            myBounds.Max = Vector3.Transform(max, myWorldTransform);
        }

        /// <summary>
        /// Initializes this model instance
        /// </summary>
        /// <param name="graphics">The graphics device to initialize with</param>
        public override void Init(GraphicsDevice graphics)
        {
        }

        /// <summary>
        /// Renders this model instance
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="matrices"></param>
        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            myModel.CopyBoneTransformsFrom(myBoneTransformations);
            myModel.Draw(myWorldTransform, matrices.View, matrices.Projection);
        }

        /// <summary>
        /// Checks whether this model instance should be rendered with the current bounding frustum
        /// </summary>
        /// <param name="cameraFrustum">The camera frustum for culling</param>
        /// <returns>True. Always.</returns>
        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return cameraFrustum.Intersects(myBounds);
        }
    }
}
