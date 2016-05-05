using Justice.Geometry;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Justice.Tools
{
    public class GeometryBuilder<T> where T : struct, IVertexType
    {
        List<T> myVertexList;
        List<int> myIndexList;

        PrimitiveType myPrimitiveType;
        
        public GeometryBuilder(PrimitiveType primitiveType)
        {
            myPrimitiveType = primitiveType;
            myVertexList = new List<T>();
            myIndexList = new List<int>();
        }

        public void Clear()
        {
            myVertexList.Clear();
            myIndexList.Clear();
        }

        public int AddVertices(T[] vertices)
        {
            int index = myVertexList.Count;
            myVertexList.AddRange(vertices);
            return index;
        }

        public int AddVertex(T vertex)
        {
            myVertexList.Add(vertex);
            return myVertexList.Count - 1;
        }

        public void AddIndices(int[] indices)
        {
            myIndexList.AddRange(indices);
        }

        public void AddIndex(int index)
        {
            myIndexList.Add(index);
        }
        
        public GeometryMesh Bake(GraphicsDevice device, int materialId)
        {
            if (myVertexList.Count > 0)
            {
                VertexBuffer vBuffer = new VertexBuffer(device, typeof(T), myVertexList.Count, BufferUsage.WriteOnly);
                vBuffer.SetData(myVertexList.ToArray());

                IndexBuffer iBuffer = null;

                if (myIndexList.Count > 0)
                {
                    iBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, myIndexList.Count, BufferUsage.WriteOnly);
                    iBuffer.SetData(myIndexList.ToArray());
                }

                GeometryMesh result = new GeometryMesh(vBuffer, iBuffer, myPrimitiveType);
                result.MaterialId = materialId;
                result.SetBounds(GetBounds());
                return result;
            }
            else
                throw new InvalidOperationException("Cannot bake empty mesh");
        }
        
        public BoundingBox GetBounds()
        {
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            FieldInfo fInfo = typeof(T).GetField("Position");

            for(int index = 0; index < myVertexList.Count; index ++)
            {
                min = Vector3.Min(min, (Vector3)fInfo.GetValue(myVertexList[index]));
                max = Vector3.Max(max, (Vector3)fInfo.GetValue(myVertexList[index]));
            }

            return new BoundingBox(min, max);
        }
    }
}
