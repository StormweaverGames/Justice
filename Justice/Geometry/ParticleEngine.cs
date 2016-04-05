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
        public struct Particle
        {
            public bool Alive { get; set; }
            public Color Color { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }
        }

        private Particle[] myParticles;
        private VertexPositionColor[] myVertices;
        private VertexBuffer myVertexBuffer;
        private BasicEffect myEffect;
        private Random rand;

        public BoundingBox Bounds
        {
            get { return new BoundingBox(new Vector3(float.MinValue), new Vector3(float.MaxValue)); }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public ParticleEngine(GraphicsDevice graphics, int particleCount)
        {
            myParticles = new Particle[particleCount];
            myVertices = new VertexPositionColor[particleCount * 2];

            myVertexBuffer = new VertexBuffer(graphics, VertexPositionColor.VertexDeclaration, particleCount * 2, BufferUsage.None);

            myEffect = new BasicEffect(graphics);
            myEffect.VertexColorEnabled = true;

            rand = new Random();
        }

        public void Update(GameTime gameTime)
        {
            for (int index = 0; index < myParticles.Length; index ++)
            {
                if (myParticles[index].Alive)
                {
                    myParticles[index].Position += myParticles[index].Direction;

                    if (myParticles[index].Position.Z < -50)
                    {
                        SpawnParticle(ref myParticles[index]);
                    }

                    myVertices[index * 2].Position = myParticles[index].Position - myParticles[index].Direction;
                    myVertices[index * 2 + 1].Position = myParticles[index].Position;
                }
            }

            myVertexBuffer.SetData(myVertices);
        }

        public void Render(GraphicsDevice graphics, CameraMatrices matrices)
        {
            myEffect.World = Matrix.CreateTranslation(Matrix.Invert(matrices.View).Translation);
            myEffect.View = matrices.View;
            myEffect.Projection = matrices.Projection;

            graphics.SetVertexBuffer(myVertexBuffer);

            for(int index = 0; index < myEffect.CurrentTechnique.Passes.Count; index ++)
            {
                myEffect.CurrentTechnique.Passes[index].Apply();

                graphics.DrawPrimitives(PrimitiveType.LineList, 0, myParticles.Length);
            }
        }

        private void SpawnParticle(ref Particle particle)
        {
            particle.Position = new Vector3((float)((rand.NextDouble() - 0.5) * 50), (float)((rand.NextDouble() - 0.5) * 50), (float)((rand.NextDouble() - 0.5) * 100));
            particle.Direction = new Vector3((float)(rand.NextDouble() * 0.05f), (float)(rand.NextDouble() * 0.05f), -(float)(1.0 + rand.NextDouble()));
        }

        public void Init(GraphicsDevice graphics)
        {
            for(int index = 0; index < myParticles.Length; index ++)
            {
                myParticles[index].Color = Color.DarkBlue;
                SpawnParticle(ref myParticles[index]);
                myParticles[index].Alive = true;
            }
        }

        public bool ShouldRender(BoundingFrustum cameraFrustum)
        {
            return true;
        }
    }
}
