using BEPUutilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justice.Tools
{
    /// <summary>
    /// Utility class for serializing and deserializing BEPU physics' basic structures
    /// </summary>
    public static class BEPUSerializers
    {
        #region Vectors

        /// <summary>
        /// Serializes a 2-Dimensional vector to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Vector2 vector, BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
        }

        /// <summary>
        /// Deserializes a 2-Dimensional vector from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Vector2 DeserializeVector2(BinaryReader reader)
        {
            Vector2 result;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 3-Dimensional vector to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Vector3 vector, BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        /// <summary>
        /// Deserializes a 3-Dimensional vector from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Vector3 DeserializeVector3(BinaryReader reader)
        {
            Vector3 result;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();
            result.Z = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 4-Dimensional vector to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Vector4 vector, BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
            writer.Write(vector.W);
        }

        /// <summary>
        /// Deserializes a 4-Dimensional vector from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Vector4 DeserializeVector4(BinaryReader reader)
        {
            Vector4 result;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();
            result.Z = reader.ReadSingle();
            result.W = reader.ReadSingle();

            return result;
        }

        #endregion

        #region Matrices and Transformations

        /// <summary>
        /// Serializes a Quaternion to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Quaternion value, BinaryWriter writer)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }

        /// <summary>
        /// Deserializes a Quaternion from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Quaternion DeserializeQuaternion(BinaryReader reader)
        {
            Quaternion result;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();
            result.Z = reader.ReadSingle();
            result.W = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 3x3 matrix to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Matrix3x3 matrix, BinaryWriter writer)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);
            writer.Write(matrix.M13);

            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
            writer.Write(matrix.M23);

            writer.Write(matrix.M31);
            writer.Write(matrix.M32);
            writer.Write(matrix.M33);
        }

        /// <summary>
        /// Deserializes a 3x3 matrix from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Matrix3x3 DeserializeMatrix3x3(BinaryReader reader)
        {
            Matrix3x3 result;

            result.M11 = reader.ReadSingle();
            result.M12 = reader.ReadSingle();
            result.M13 = reader.ReadSingle();

            result.M21 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();
            result.M23 = reader.ReadSingle();

            result.M31 = reader.ReadSingle();
            result.M32 = reader.ReadSingle();
            result.M33 = reader.ReadSingle();

            return result;
        }
        
        /// <summary>
        /// Serializes a 4x4 matrix to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Matrix matrix, BinaryWriter writer)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);
            writer.Write(matrix.M13);
            writer.Write(matrix.M14);

            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
            writer.Write(matrix.M23);
            writer.Write(matrix.M24);

            writer.Write(matrix.M31);
            writer.Write(matrix.M32);
            writer.Write(matrix.M33);
            writer.Write(matrix.M34);

            writer.Write(matrix.M41);
            writer.Write(matrix.M42);
            writer.Write(matrix.M43);
            writer.Write(matrix.M44);
        }

        /// <summary>
        /// Deserializes a 4x4 matrix from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Matrix DeserializeMatrix4x4(BinaryReader reader)
        {
            Matrix result;

            result.M11 = reader.ReadSingle();
            result.M12 = reader.ReadSingle();
            result.M13 = reader.ReadSingle();
            result.M14 = reader.ReadSingle();

            result.M21 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();
            result.M23 = reader.ReadSingle();
            result.M24 = reader.ReadSingle();

            result.M31 = reader.ReadSingle();
            result.M32 = reader.ReadSingle();
            result.M33 = reader.ReadSingle();
            result.M34 = reader.ReadSingle();

            result.M41 = reader.ReadSingle();
            result.M42 = reader.ReadSingle();
            result.M43 = reader.ReadSingle();
            result.M44 = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 2x2 matrix to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Matrix2x2 matrix, BinaryWriter writer)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);

            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
        }

        /// <summary>
        /// Deserializes a 2x2 matrix from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Matrix2x2 DeserializeMatrix2x2(BinaryReader reader)
        {
            Matrix2x2 result;

            result.M11 = reader.ReadSingle();
            result.M12 = reader.ReadSingle();

            result.M21 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 2x3 matrix to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Matrix2x3 matrix, BinaryWriter writer)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);
            writer.Write(matrix.M13);

            writer.Write(matrix.M21);
            writer.Write(matrix.M22);
            writer.Write(matrix.M23);
        }

        /// <summary>
        /// Deserializes a 2x3 matrix from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Matrix2x3 DeserializeMatrix2x3(BinaryReader reader)
        {
            Matrix2x3 result;

            result.M11 = reader.ReadSingle();
            result.M12 = reader.ReadSingle();
            result.M13 = reader.ReadSingle();

            result.M21 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();
            result.M23 = reader.ReadSingle();

            return result;
        }

        /// <summary>
        /// Serializes a 3x2 matrix to the binary stream
        /// </summary>
        /// <param name="vector">The value to serialize</param>
        /// <param name="writer">The binary stream to write to</param>
        public static void Serialize(this Matrix3x2 matrix, BinaryWriter writer)
        {
            writer.Write(matrix.M11);
            writer.Write(matrix.M12);

            writer.Write(matrix.M21);
            writer.Write(matrix.M22);

            writer.Write(matrix.M31);
            writer.Write(matrix.M32);
        }

        /// <summary>
        /// Deserializes a 3x2 matrix from the binary stream
        /// </summary>
        /// <param name="reader">The binary stream to read from</param>
        /// <returns>The deserialized value</returns>
        public static Matrix3x2 DeserializeMatrix3x2(BinaryReader reader)
        {
            Matrix3x2 result;

            result.M11 = reader.ReadSingle();
            result.M12 = reader.ReadSingle();

            result.M21 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();

            result.M31 = reader.ReadSingle();
            result.M32 = reader.ReadSingle();

            return result;
        }

        #endregion
    }
}
