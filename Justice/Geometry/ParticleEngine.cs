using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Geometry
{
    public class ParticleEngine : IRenderable
    {
        private const float SPREAD = 15.0f;

        public struct Particle
        {
            public bool Alive;
            public Color Color;
            public Vector3 Position;
            public Vector3 Direction;
        }

        private Particle[] myParticles;
        private VertexPositionColor[] myVertices;
        private VertexBuffer myVertexBuffer;
        private BasicEffect myEffect;
        private Random rand;
        private IScene myScene;
        private Vector3 myCamPos;

        private const int VERTS_PER_NODE = 10;

        private Vector3[] OFFSETS;

        public override BoundingBox RenderBounds
        {
            get { return new BoundingBox(new Vector3(float.MinValue), new Vector3(float.MaxValue)); }
        }

        public override bool IsVisible
        {
            get { return true; }
        }
        
        public ParticleEngine(GraphicsDevice graphics, IScene scene, int particleCount)
        {
            myScene = scene;
            rand = new Random();

            myParticles = new Particle[particleCount];
            myVertices = new VertexPositionColor[particleCount * 2 * VERTS_PER_NODE];

            OFFSETS = new Vector3[VERTS_PER_NODE];
            for(int i = 0; i < VERTS_PER_NODE; i ++)
            {
                OFFSETS[i] = new Vector3((float)((rand.NextDouble() - 0.5f) * 5), (float)((rand.NextDouble() - 0.5f) * 5), (float)(rand.NextDouble() * 10));
            }

            myVertexBuffer = new VertexBuffer(graphics, VertexPositionColor.VertexDeclaration, myVertices.Length, BufferUsage.WriteOnly);

            myEffect = new BasicEffect(graphics);
            myEffect.VertexColorEnabled = true;
            myEffect.TextureEnabled = false;
            myEffect.LightingEnabled = false;

        }

        //Ray myRay;
        public void Update(GameTime gameTime)
        {
            //Matrix matrix = Matrix.CreateTranslation(myCamPos);

            for (int index = 0; index < myParticles.Length; index ++)
            {
                if (myParticles[index].Alive)
                {
                    myParticles[index].Position += myParticles[index].Direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (myParticles[index].Position.Z < -10)
                    {
                        SpawnParticle(ref myParticles[index], false);
                    }
                    
                    for (int i = 0; i < VERTS_PER_NODE; i ++)
                    {
                        Vector3.Add(ref myParticles[index].Position, ref OFFSETS[i], out myVertices[index * VERTS_PER_NODE + (i * 2) + 1].Position);
                        Vector3.Subtract(ref myVertices[index * VERTS_PER_NODE + (i * 2) + 1].Position, ref myParticles[index].Direction, out myVertices[index * VERTS_PER_NODE + (i * 2)].Position);
                    }
                }
            }
        }

        public override void PreRender(GraphicsDevice graphics, ICamera camera)
        {
            WorldTransform = Matrix.CreateTranslation(Matrix.Invert(camera.Matrices.View).Translation);
        }
        
        public override void RenderShadow(GraphicsDevice graphics, CameraMatrices matrices)
        {
            // We don't cast shadows fo the skybox
        }

        public override void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            myVertexBuffer.SetData(myVertices);
            graphics.SetVertexBuffer(myVertexBuffer);

            graphics.DrawPrimitives(PrimitiveType.LineList, 0, myVertices.Length / 2);
        }

        private void SpawnParticle(ref Particle particle, bool init = true)
        {
            if (init)
            {
                particle.Position = new Vector3((float)((rand.NextDouble() - 0.5) * SPREAD), (float)((rand.NextDouble() - 0.5) * SPREAD), (float)(rand.NextDouble() * 50));
                particle.Direction = new Vector3(0, 0, -(float)(15 + rand.NextDouble()));
            }
            else
            {
                particle.Position.Z = 50 + rand.Next(-5, 5) * 1.0f;
            }

        }

        public override void Init(GraphicsDevice graphics)
        {
            for(int index = 0; index < myParticles.Length; index ++)
            {
                myParticles[index].Color = Color.Black;
                SpawnParticle(ref myParticles[index]);
                //myParticles[index].Position.X = index % (myParticles.Length / 25) - 12.5f;
                //myParticles[index].Position.Y = index / (myParticles.Length / 25) - 12.5f;
                myParticles[index].Alive = true;
            }
        }

        public override bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
