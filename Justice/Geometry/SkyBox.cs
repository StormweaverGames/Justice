using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Justice.Geometry
{
    public class SkyBox : IRenderable
    {
        private static readonly BoundingBox DEFAULT_BOUNDS = new BoundingBox(new Vector3(float.MinValue), new Vector3(float.MaxValue));

        SamplerState mySamplerState;
        DepthStencilState myDepthDisabledState;
        DepthStencilState myDefaultDepthState;

        private BasicEffect myEffect;
        private Texture2D myTexture;
        private GeometryMesh myMesh;

        public override BoundingBox RenderBounds
        {
            get { return DEFAULT_BOUNDS; }
        }

        public override bool IsVisible
        {
            get { return true; }
        }

        public override bool IsPreRendered
        {
            get { return true; }
        }

        new public Texture2D Texture
        {
            get { return myTexture; }
            set
            {
                myTexture = value;

                if (myEffect != null)
                    myEffect.Texture = value;
            }
        }

        public GeometryMesh Mesh
        {
            get { return myMesh; }
            set
            {
                myMesh = value;
                myMesh.Effect = myEffect;
            }
        }
        
        public SkyBox(GeometryMesh mesh, Texture2D texture)
        {
            myMesh = mesh;
            myTexture = texture;
        }

        public override void Init(GraphicsDevice graphics)
        {
            mySamplerState = new SamplerState();
            mySamplerState.AddressU = TextureAddressMode.Clamp;
            mySamplerState.AddressV = TextureAddressMode.Clamp;

            myDepthDisabledState = new DepthStencilState();
            myDepthDisabledState.DepthBufferEnable = false;

            myDefaultDepthState = new DepthStencilState();
            myDefaultDepthState.DepthBufferEnable = true;

            myEffect = new BasicEffect(graphics);
            myEffect.TextureEnabled = true;
            myEffect.Texture = myTexture;

            if (myMesh != null)
            {
                myMesh.Effect = myEffect;
                myMesh.Init(graphics);
            }
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            graphics.SamplerStates[0] = mySamplerState;            
            graphics.DepthStencilState = myDepthDisabledState;

            myMesh.Transformation = Matrix.CreateTranslation(Matrix.Invert(matrices.View).Translation);
            myMesh.Render(graphics, matrices);

            graphics.DepthStencilState = myDefaultDepthState;
            graphics.BlendState = BlendState.Opaque;
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
