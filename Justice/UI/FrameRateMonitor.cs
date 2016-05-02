using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public class FrameRateMonitor : ScreenElement
    {
        const int NUM_SAMPLES = 120;
        const int NUM_UTIL_VERTS = 10;
        int index = 0;

        float[] myTimes;
        VertexPositionColor[] myVertices;
        VertexBuffer myVertexBuffer;
        BasicEffect myEffect;

        float myMaxValue;
        float myMinValue;
        float myRange;
        float mySampleWidth;
        float myAverage = 0;
        double myTimerCounter = 0;

        public float Max
        {
            get { return myMaxValue * 2; }
            set
            {
                myMaxValue = value * 2;
                myRange = myMaxValue - myMinValue;
            }
        }
        public float Min
        {
            get { return myMinValue; }
            set
            {
                myMinValue = value;
                myRange = myMaxValue - myMinValue;
            }
        }
        public float Mid
        {
            get { return (myMaxValue + myMinValue) / 2f; }
        }
        public float Average
        {
            get { return myAverage; }
        }
        public float AveragePosition
        {
            get { return Bounds.Top + (myBounds.Height * (1 - (myAverage / myRange) * 0.5f)); }
        }
        public bool AutoExpand
        {
            get;
            set;
        }
        public double SampleRate
        {
            get;
            set;
        }
        
        public FrameRateMonitor(Rectangle bounds, float min, float max, bool autoExpand)
        {
            myTimes = new float[NUM_SAMPLES];
            myVertices = new VertexPositionColor[NUM_SAMPLES + NUM_UTIL_VERTS];

            Bounds = bounds;

            myMinValue = min;
            myMaxValue = max;
            myRange = max - min;
            AutoExpand = autoExpand;

            SampleRate = 1;
            mySampleWidth = bounds.Width / (float)NUM_SAMPLES;

        }

        protected override void OnBoundsUpdating(Rectangle myBounds, Rectangle value)
        {
            mySampleWidth = myBounds.Width / (float)NUM_SAMPLES;

            for (int index = 0; index < NUM_SAMPLES; index++)
                myVertices[index].Position.X = myBounds.Left + mySampleWidth * index;

            RecalculateAllY();
        }

        protected override void OnUpdating(GameTime gameTime)
        {
            myTimerCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (myTimerCounter >= (1 / SampleRate))
            {
                PushFrame((float)(1 / gameTime.ElapsedGameTime.TotalSeconds));
                myTimerCounter -= (1 / SampleRate);
            }
        }

        private void PushFrame(float value)
        {
            if (myTimes[index] == myMaxValue || value > myMaxValue)
            {
                myTimes[index] = value;
                myMaxValue = myTimes.Max();
                myRange = myMaxValue - myMinValue;
                RecalculateAllY();
            }
            else
            {
                myTimes[index] = value;
                myVertices[NUM_UTIL_VERTS + index].Position.Y = myBounds.Top + (myBounds.Height * (1 - (value / myRange) * 0.5f));
            }

            index = index == NUM_SAMPLES - 1 ? 0 : index + 1;

            myAverage = myTimes.Average();

            myVertices[6].Position.Y = myBounds.Top + (myBounds.Height * (1 - (myAverage / myRange) * 0.5f));
            myVertices[7].Position.Y = myBounds.Top + (myBounds.Height * (1 - (myAverage / myRange) * 0.5f));

            myVertices[8].Position.X = myBounds.Left + (mySampleWidth * index);
            myVertices[9].Position.X = myBounds.Left + (mySampleWidth * index);
        }

        private void RecalculateAllY()
        {
            for (int index = 0; index < NUM_SAMPLES; index++)
                myVertices[NUM_UTIL_VERTS + index].Position.Y = myBounds.Top + (myBounds.Height * (1 - (myTimes[index] / myRange) * 0.5f));
        }
        
        protected override void OnInit(GraphicsDevice graphics)
        {
            myVertices[0].Position = new Vector3(Bounds.Left, Bounds.Top, 0);
            myVertices[0].Color = Color.Green;
            myVertices[1].Position = new Vector3(Bounds.Right, Bounds.Top, 0);
            myVertices[1].Color = Color.Green;

            myVertices[2].Position = new Vector3(Bounds.Left, Bounds.Bottom, 0);
            myVertices[2].Color = Color.Red;
            myVertices[3].Position = new Vector3(Bounds.Right, Bounds.Bottom, 0);
            myVertices[3].Color = Color.Red;
            
            myVertices[4].Position = new Vector3(Bounds.Left, Bounds.Center.Y, 0);
            myVertices[4].Color = Color.Yellow;
            myVertices[5].Position = new Vector3(Bounds.Right, Bounds.Center.Y, 0);
            myVertices[5].Color = Color.Yellow;

            myVertices[6].Position = new Vector3(Bounds.Left, Bounds.Center.Y, 0);
            myVertices[6].Color = Color.Magenta;
            myVertices[7].Position = new Vector3(Bounds.Right, Bounds.Center.Y, 0);
            myVertices[7].Color = Color.Magenta;

            myVertices[8].Position = new Vector3(Bounds.Left, Bounds.Top, 0);
            myVertices[8].Color = Color.White;
            myVertices[9].Position = new Vector3(Bounds.Left, Bounds.Bottom, 0);
            myVertices[9].Color = Color.White;

            for (int index = 0; index < NUM_SAMPLES; index++)
            {
                myVertices[NUM_UTIL_VERTS + index].Position.X = myBounds.Left + mySampleWidth * index;
                myVertices[NUM_UTIL_VERTS + index].Color = Color.Green;
            }

            myEffect = new BasicEffect(graphics);
            myEffect.VertexColorEnabled = true;
            myEffect.View = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            myEffect.World = Matrix.Identity;
            myEffect.Projection = Matrix.CreateOrthographicOffCenter(0, graphics.Viewport.Width, graphics.Viewport.Height, 0, -1, 1);
            
            myVertexBuffer = new VertexBuffer(graphics, VertexPositionColor.VertexDeclaration, NUM_SAMPLES + NUM_UTIL_VERTS, BufferUsage.WriteOnly);
        }

        protected override void OnViewportResized(Viewport viewport)
        {
            myEffect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 1);
        }

        protected override void OnRender(GraphicsDevice graphics, GameTime gameTime)
        {
            myVertexBuffer.SetData(myVertices);
            graphics.SetVertexBuffer(myVertexBuffer);

            for (int passIndex = 0; passIndex < myEffect.CurrentTechnique.Passes.Count; passIndex ++)
            {
                myEffect.CurrentTechnique.Passes[passIndex].Apply();
                graphics.DrawPrimitives(PrimitiveType.LineStrip, NUM_UTIL_VERTS, NUM_SAMPLES - 1);
                graphics.DrawPrimitives(PrimitiveType.LineList, 0, NUM_UTIL_VERTS);
            }
        }
    }
}
