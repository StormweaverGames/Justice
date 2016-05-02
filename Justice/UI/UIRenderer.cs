using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.UI
{
    public class UIRenderer
    {
        private const int MY_BUFFER_SIZE = 1024;

        private GraphicsDevice myGraphics;
        private VertexPositionColor[] myLineVertexCache;
        private VertexPositionColor[] myTriVertexCache;
        private VertexPositionColorTexture[] myTexturedVertexCache;

        private VertexBuffer myVertexBuffer;
        private VertexBuffer myTexturedVertexBuffer;
        private BasicEffect myEffect;

        private Texture2D myCurrentTexture;

        private int myLineCount;
        private int myTriCount;
        private int myTexturedCount;

        public UIRenderer(GraphicsDevice graphics)
        {
            myGraphics = graphics;

            myLineVertexCache = new VertexPositionColor[MY_BUFFER_SIZE];
            myTriVertexCache = new VertexPositionColor[MY_BUFFER_SIZE];
            myTexturedVertexCache = new VertexPositionColorTexture[MY_BUFFER_SIZE];

            myVertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), MY_BUFFER_SIZE, BufferUsage.WriteOnly);
            myTexturedVertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColorTexture), MY_BUFFER_SIZE, BufferUsage.WriteOnly);

            myEffect = new BasicEffect(graphics);
            myEffect.VertexColorEnabled = true;
        }

        public void ResetClipBounds()
        {
            myGraphics.ScissorRectangle = myGraphics.Viewport.Bounds;
        }

        public void SetClipBounds(Rectangle bounds)
        {
            myGraphics.ScissorRectangle = bounds;
        }

        /// <summary>
        /// Draws a line between two points
        /// </summary>
        /// <param name="x1">The x coordinate of the first point</param>
        /// <param name="y1">The y coordinate of the first point</param>
        /// <param name="x2">The x coordinate of the second point</param>
        /// <param name="y2">The x coordinate of the second point</param>
        /// <param name="color">The color of the line</param>
        /// <param name="depth">The depth to draw the line at, from -1 to +1</param>
        public void DrawLine(float x1, float y1, float x2, float y2, Color color, float depth = 0)
        {
            DrawLine(x1, y1, x2, y2, color, color, depth);
        }

        /// <summary>
        /// Draws a line between two points
        /// </summary>
        /// <param name="x1">The x coordinate of the first point</param>
        /// <param name="y1">The y coordinate of the first point</param>
        /// <param name="x2">The x coordinate of the second point</param>
        /// <param name="y2">The x coordinate of the second point</param>
        /// <param name="color1">The color of the line at the first point</param>
        /// <param name="color2">The color of the line at the second point</param>
        /// <param name="depth">The depth to draw the line at, from -1 to +1</param>
        public void DrawLine(float x1, float y1, float x2, float y2, Color color1, Color color2, float depth = 0)
        {
            if (myLineCount + 1 > MY_BUFFER_SIZE / 2)
                FlushBuffer(ref myLineCount, myLineVertexCache, PrimitiveType.LineList);

            myLineVertexCache[myLineCount * 2].Position.X = x1;
            myLineVertexCache[myLineCount * 2].Position.Y = y1;
            myLineVertexCache[myLineCount * 2].Position.Z = depth;
            myLineVertexCache[myLineCount * 2].Color = color1;

            myLineVertexCache[myLineCount * 2 + 1].Position.X = x2;
            myLineVertexCache[myLineCount * 2 + 1].Position.Y = y2;
            myLineVertexCache[myLineCount * 2 + 1].Position.Z = depth;
            myLineVertexCache[myLineCount * 2 + 1].Color = color2;

            myLineCount++;
        }

        /// <summary>
        /// Draws a rectangular outline in 1 color
        /// </summary>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="left">The left side of the rectangle</param>
        /// <param name="bottom">The bottom side of the rectangle</param>
        /// <param name="right">The right side of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="depth">The depth to draw the line at, from -1 to +1</param>
        public void DrawRectangle(float top, float left, float bottom, float right, Color color, float depth = 0)
        {
            if (myLineCount + 4 > MY_BUFFER_SIZE / 2)
                FlushBuffer(ref myLineCount, myLineVertexCache, PrimitiveType.LineList);

            DrawLine(top, left, top, right, color, depth);
            DrawLine(bottom, left, bottom, right, color, depth);
            DrawLine(top, left, bottom, left, color, depth);
            DrawLine(top, right, bottom, right, color, depth);
        }

        public void DrawTri(float x1, float y1, float x2, float y2, float x3, float y3, Color color, float depth)
        {
            if (myTriCount + 1 > MY_BUFFER_SIZE / 3)
                FlushBuffer(ref myTriCount, myTriVertexCache, PrimitiveType.TriangleList);

            myTriVertexCache[myTriCount * 3].Position.X = x1;
            myTriVertexCache[myTriCount * 3].Position.Y = y1;
            myTriVertexCache[myTriCount * 3].Position.Z = depth;
            myTriVertexCache[myTriCount * 3].Color = color;

            myTriVertexCache[myTriCount * 3 + 1].Position.X = x2;
            myTriVertexCache[myTriCount * 3 + 1].Position.Y = y2;
            myTriVertexCache[myTriCount * 3 + 1].Position.Z = depth;
            myTriVertexCache[myTriCount * 3 + 1].Color = color;

            myTriVertexCache[myTriCount * 3 + 2].Position.X = x3;
            myTriVertexCache[myTriCount * 3 + 2].Position.Y = y3;
            myTriVertexCache[myTriCount * 3 + 2].Position.Z = depth;
            myTriVertexCache[myTriCount * 3 + 2].Color = color;
        }

        public void DrawFilledRectangle(float top, float left, float bottom, float right, Color color, float depth = 0)
        {
            if (myTriCount + 2 > MY_BUFFER_SIZE / 3)
                FlushBuffer(ref myTriCount, myTriVertexCache, PrimitiveType.TriangleList);

            DrawTri(top, left, top, right, bottom, right, color, depth);
            DrawTri(top, left, bottom, right, bottom, left, color, depth);
        }

        /// <summary>
        /// Draws a rectangular outline in 1 color
        /// </summary>
        /// <param name="top">The top of the rectangle</param>
        /// <param name="left">The left side of the rectangle</param>
        /// <param name="bottom">The bottom side of the rectangle</param>
        /// <param name="right">The right side of the rectangle</param>
        /// <param name="width">The width of the rectangle bounds</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="depth">The depth to draw the line at, from -1 to +1</param>
        public void DrawRectangle(float top, float left, float bottom, float right, float width, Color color, BorderStyle borderStyle = BorderStyle.Centered, float depth = 0)
        {
            if (myTriCount + 8 > MY_BUFFER_SIZE / 3)
                FlushBuffer(ref myTriCount, myTriVertexCache, PrimitiveType.TriangleList);
            
            switch(borderStyle)
            {
                case BorderStyle.Centered:
                    float halfWidth = width / 2;

                    // Left line
                    DrawTri(left - halfWidth, top - halfWidth, left + halfWidth, bottom + halfWidth, left - halfWidth, bottom + halfWidth, color, depth);
                    DrawTri(left - halfWidth, top - halfWidth, left + halfWidth, top - halfWidth, left + halfWidth, bottom + halfWidth, color, depth);

                    // Right line
                    DrawTri(right - halfWidth, top - halfWidth, right + halfWidth, bottom + halfWidth, right - halfWidth, bottom + halfWidth, color, depth);
                    DrawTri(right - halfWidth, top - halfWidth, right + halfWidth, top - halfWidth, right + halfWidth, bottom + halfWidth, color, depth);

                    // Top line
                    DrawTri(left + halfWidth, top - halfWidth, right - halfWidth, top - halfWidth, right - halfWidth, top + halfWidth, color, depth);
                    DrawTri(left + halfWidth, top - halfWidth, right - halfWidth, top + halfWidth, left + halfWidth, top + halfWidth, color, depth);

                    // Bottom line
                    DrawTri(left + halfWidth, bottom - halfWidth, right - halfWidth, bottom - halfWidth, right - halfWidth, bottom + halfWidth, color, depth);
                    DrawTri(left + halfWidth, bottom - halfWidth, right - halfWidth, bottom + halfWidth, left + halfWidth, bottom + halfWidth, color, depth);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Handles flushing all of the renderer's buffers and draws the result to the screen
        /// </summary>
        public void Flush()
        {
            FlushBuffer(ref myLineCount, myLineVertexCache, PrimitiveType.LineList);
            FlushBuffer(ref myTriCount, myLineVertexCache, PrimitiveType.TriangleList);
            FlushBuffer(ref myTexturedCount, myTexturedVertexCache, PrimitiveType.TriangleList);
        }

        private void FlushBuffer(ref int numElements, VertexPositionColorTexture[] verts, PrimitiveType primitiveMode)
        {
            if (numElements > 0)
            {
                int numVertices =
                    primitiveMode == PrimitiveType.LineList ? numElements * 2 :
                    primitiveMode == PrimitiveType.LineStrip ? numElements + 1 :
                    primitiveMode == PrimitiveType.TriangleList ? numElements * 3 :
                    numElements + 2; // TriangleStrip

                myTexturedVertexBuffer.SetData(verts, 0, numVertices);
                myGraphics.SetVertexBuffer(myTexturedVertexBuffer);


                myEffect.Texture = myCurrentTexture;
                myEffect.TextureEnabled = true;

                for (int index = 0; index < myEffect.CurrentTechnique.Passes.Count; index++)
                {
                    myEffect.CurrentTechnique.Passes[index].Apply();
                    myGraphics.DrawPrimitives(PrimitiveType.LineList, 0, numElements);
                }

                numElements = 0;
            }
        }

        private void FlushBuffer(ref int numElements, VertexPositionColor[] verts, PrimitiveType primitiveMode)
        {
            if (numElements > 0)
            {
                int numVertices =
                    primitiveMode == PrimitiveType.LineList ? numElements * 2 :
                    primitiveMode == PrimitiveType.LineStrip ? numElements + 1 :
                    primitiveMode == PrimitiveType.TriangleList ? numElements * 3 :
                    numElements + 2; // TriangleStrip

                myVertexBuffer.SetData(verts, 0, numVertices);
                myGraphics.SetVertexBuffer(myVertexBuffer);

                myEffect.TextureEnabled = false;

                for (int index = 0; index < myEffect.CurrentTechnique.Passes.Count; index++)
                {
                    myEffect.CurrentTechnique.Passes[index].Apply();
                    myGraphics.DrawPrimitives(PrimitiveType.LineList, 0, numElements);
                }

                numElements = 0;
            }
        }
    }
}
