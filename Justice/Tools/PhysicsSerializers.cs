using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.DataStructures;
using BEPUutilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Justice.Tools
{
    /// <summary>
    /// Represents physics shapes that can be serialized
    /// </summary>
    public enum ShapeTypes
    {
        /// <summary>
        /// The box shape (BoxShape)
        /// </summary>
        Box = 0,
        /// <summary>
        /// The sphere shape (SphereShape)
        /// </summary>
        Sphere = 1,
        /// <summary>
        /// The cylinder shape (CylinderShape)
        /// </summary>
        Cylinder = 2,
        /// <summary>
        /// The triangle shape (TriangleShape)
        /// </summary>
        Triangle = 3,
        /// <summary>
        /// The cone shape (ConeShape)
        /// </summary>
        Cone = 4,
        /// <summary>
        /// The capsule shape (CapsuleShape)
        /// </summary>
        Capsule = 5,
        /// <summary>
        /// The compound shape (CompundShape)
        /// </summary>
        Compund = 6,
        /// <summary>
        /// The mobile mesh shape (MobileMeshShape)
        /// </summary>
        MobileMesh = 7
    }

    /// <summary>
    /// Utility methods to serialize and deserialize physics items
    /// </summary>
    public static class PhysicsSerializers
    {
        /// <summary>
        /// Serializes a box shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this BoxShape shape, BinaryWriter writer)
        {
            writer.Write(shape.Length);
            writer.Write(shape.Width);
            writer.Write(shape.Height);
        }

        /// <summary>
        /// Deserializes a box shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The box shape deserialized from the stream</returns>
        public static BoxShape DeserializeBox(BinaryReader reader)
        {
            float length = reader.ReadSingle();
            float width = reader.ReadSingle();
            float height = reader.ReadSingle();

            return new BoxShape(width, height, length);
        }

        /// <summary>
        /// Serializes a sphere shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this SphereShape shape, BinaryWriter writer)
        {
            writer.Write(shape.Radius);
        }

        /// <summary>
        /// Deserializes a sphere shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The sphere shape deserialized from the stream</returns>
        public static SphereShape DeserializeSphere(BinaryReader reader)
        {
            float radius = reader.ReadSingle();

            return new SphereShape(radius);
        }

        /// <summary>
        /// Serializes a cylinder shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this CylinderShape shape, BinaryWriter writer)
        {
            writer.Write(shape.Height);
            writer.Write(shape.Radius);
        }

        /// <summary>
        /// Deserializes a cylinder shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The cylinder shape deserialized from the stream</returns>
        public static CylinderShape DeserializeCylinder(BinaryReader reader)
        {
            float height = reader.ReadSingle();
            float radius = reader.ReadSingle();

            return new CylinderShape(height, radius);
        }

        /// <summary>
        /// Serializes a triangle shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this TriangleShape shape, BinaryWriter writer)
        {
            shape.VertexA.Serialize(writer);
            shape.VertexB.Serialize(writer);
            shape.VertexC.Serialize(writer);
        }

        /// <summary>
        /// Deserializes a triangle shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The triangle shape deserialized from the stream</returns>
        public static TriangleShape DeserializeTriangle(BinaryReader reader)
        {
            Vector3 pointA = BEPUSerializers.DeserializeVector3(reader);
            Vector3 pointB = BEPUSerializers.DeserializeVector3(reader);
            Vector3 pointC = BEPUSerializers.DeserializeVector3(reader);

            return new TriangleShape(pointA, pointB, pointC);
        }

        /// <summary>
        /// Serializes a cone shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this ConeShape shape, BinaryWriter writer)
        {
            writer.Write(shape.Radius);
            writer.Write(shape.Height);
        }

        /// <summary>
        /// Deserializes a cone shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The cone shape deserialized from the stream</returns>
        public static ConeShape DeserializeCone(BinaryReader reader)
        {
            float radius = reader.ReadSingle();
            float height = reader.ReadSingle();

            return new ConeShape(height, radius);
        }

        /// <summary>
        /// Serializes a capsule shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this CapsuleShape shape, BinaryWriter writer)
        {
            writer.Write(shape.Radius);
            writer.Write(shape.Length);
        }

        /// <summary>
        /// Deserializes a capsule shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The capsule shape deserialized from the stream</returns>
        public static CapsuleShape DeserializeCapsule(BinaryReader reader)
        {
            float radius = reader.ReadSingle();
            float length = reader.ReadSingle();

            return new CapsuleShape(length, radius);
        }

        /// <summary>
        /// Serializes a mobile mesh shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this MobileMeshShape shape, BinaryWriter writer)
        {
            TriangleMesh mesh = shape.TriangleMesh;

            // Write the counters out
            writer.Write(mesh.Data.Vertices.Length);
            writer.Write(mesh.Data.Indices.Length);

            // Write mesh vertices
            for (int localIIndex = 0; localIIndex < mesh.Data.Vertices.Length; localIIndex++)
            {
                mesh.Data.Vertices[localIIndex].Serialize(writer);
            }

            // Write mesh indices
            for (int localIIndex = 0; localIIndex < mesh.Data.Indices.Length; localIIndex++)
            {
                writer.Write(mesh.Data.Indices[localIIndex]);
            }

            // Write mesh solidity
            writer.Write((int)shape.Solidity);

            // Write the affine transform linear transformation
            shape.Transform.LinearTransform.Serialize(writer);

            // write the affine transformation translation
            shape.Transform.Translation.Serialize(writer);
        }

        /// <summary>
        /// Deserializes a mobile mesh shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The mobile mesh shape deserialized from the stream</returns>
        public static MobileMeshShape DeserializeMobileMesh(BinaryReader reader)
        {
            int numVertices = reader.ReadInt32();
            int numIndices = reader.ReadInt32();

            Vector3[] verts = new Vector3[numVertices];
            int[] indices = new int[numIndices];

            for(int index = 0; index < numVertices; index ++)
                verts[index] = BEPUSerializers.DeserializeVector3(reader);

            for (int index = 0; index < numIndices; index++)
                indices[index] = reader.ReadInt32();

            MobileMeshSolidity solidity = (MobileMeshSolidity)reader.ReadInt32();

            Matrix3x3 linearTransform = BEPUSerializers.DeserializeMatrix3x3(reader);
            Vector3 translation = BEPUSerializers.DeserializeVector3(reader);
            
            return new MobileMeshShape(verts, indices, new AffineTransform(linearTransform, translation), solidity);
        }

        /// <summary>
        /// Serializes a compound shape
        /// </summary>
        /// <param name="shape">The shape to serialize</param>
        /// <param name="writer">The binary writer to write to </param>
        public static void Serialize(this CompoundShape shape, BinaryWriter writer)
        {
            writer.Write((short)shape.Shapes.Count);

            for (int index = 0; index < shape.Shapes.Count; index++)
            {
                CompoundShapeEntry entry = shape.Shapes[index];

                // Write Translation data
                entry.LocalTransform.Position.Serialize(writer);

                // Write Orientation data
                entry.LocalTransform.Orientation.Serialize(writer);

                writer.Write(entry.Weight);
                
                switch (entry.Shape.GetType().Name)
                {
                    case "BoxShape":
                        writer.Write((int)ShapeTypes.Box);
                        BoxShape boxShape = entry.Shape as BoxShape;
                        boxShape.Serialize(writer);
                        break;
                    case "SphereShape":
                        writer.Write((int)ShapeTypes.Sphere);
                        SphereShape sphereShape = entry.Shape as SphereShape;
                        sphereShape.Serialize(writer);
                        break;
                    case "CylinderShape":
                        writer.Write((int)ShapeTypes.Cylinder);
                        CylinderShape cylinderShape = entry.Shape as CylinderShape;
                        cylinderShape.Serialize(writer);
                        break;
                    case "TriangleShape":
                        writer.Write((int)ShapeTypes.Triangle);
                        TriangleShape triangleShape = entry.Shape as TriangleShape;
                        triangleShape.Serialize(writer);
                        break;
                    case "ConeShape":
                        writer.Write((int)ShapeTypes.Cone);
                        ConeShape coneShape = entry.Shape as ConeShape;
                        coneShape.Serialize(writer);
                        break;
                    case "CapsuleShape":
                        writer.Write((int)ShapeTypes.Capsule);
                        CapsuleShape capsuleShape = entry.Shape as CapsuleShape;
                        capsuleShape.Serialize(writer);
                        break;
                    case "CompundShape":
                        writer.Write((int)ShapeTypes.Compund);
                        CompoundShape compundShape = entry.Shape as CompoundShape;
                        compundShape.Serialize(writer);
                        break;
                    case "MobileMeshShape":
                        writer.Write((int)ShapeTypes.MobileMesh);
                        MobileMeshShape mobileMeshShape = entry.Shape as MobileMeshShape;
                        mobileMeshShape.Serialize(writer);
                        break;
                    default:
                        throw new NotImplementedException(string.Format("Cannot serialize a physics entity of type \"{0}\"", entry.GetType().Name));
                }
            }
        }
        
        /// <summary>
        /// Deserializes a compund shape from a binary stream
        /// </summary>
        /// <param name="reader">The reader to decode from</param>
        /// <returns>The compund shape deserialized from the stream</returns>
        public static CompoundShape DeserializeCompound(BinaryReader reader)
        {
            List<CompoundShapeEntry> entries = new List<CompoundShapeEntry>();
            
            int shapeCount = reader.ReadInt16();

            for (int index = 0; index < shapeCount; index++)
            {
                CompoundShapeEntry entry;
                
                // Read Translation data
                entry.LocalTransform = new RigidTransform(BEPUSerializers.DeserializeVector3(reader), BEPUSerializers.DeserializeQuaternion(reader));
                entry.Weight = reader.ReadSingle();
                entry.Shape = null;

                ShapeTypes type = (ShapeTypes)reader.ReadInt32();

                switch (type)
                {
                    case ShapeTypes.Box:
                        entry.Shape = DeserializeBox(reader);
                        break;
                    case ShapeTypes.Sphere:
                        entry.Shape = DeserializeSphere(reader);
                        break;
                    case ShapeTypes.Cylinder:
                        entry.Shape = DeserializeCylinder(reader);
                        break;
                    case ShapeTypes.Triangle:
                        entry.Shape = DeserializeTriangle(reader);
                        break;
                    case ShapeTypes.Cone:
                        entry.Shape = DeserializeCone(reader);
                        break;
                    case ShapeTypes.Capsule:
                        entry.Shape = DeserializeCapsule(reader);
                        break;
                    case ShapeTypes.Compund:
                        entry.Shape = DeserializeCompound(reader);
                        break;
                    case ShapeTypes.MobileMesh:
                        entry.Shape = DeserializeMobileMesh(reader);
                        break;
                    default:
                        throw new NotImplementedException(string.Format("Cannot serialize a physics entity of type ID \"{0}\"", type));
                }

                entries.Add(entry);
            }

            return new CompoundShape(entries);
        }
    }
}
