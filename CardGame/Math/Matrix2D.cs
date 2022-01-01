using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    // 3x3 transformation matrix for 2D points.
    // Heavily inspired by:
    // https://github.com/craftworkgames/MonoGame.Extended/blob/develop/src/cs/MonoGame.Extended/Math/Matrix2.cs
    public class Matrix2D
    {
        // X Row Rotation/Scale
        public float M11; // X
        public float M12; // Y

        // Y Row Rotation/Scale.
        public float M21; // x.
        public float M22; // y.
        // M23 == 0 always leave out too save memory

        // Translation Row for moving.
        public float M31; // x.
        public float M32; // y.
        // M33 == 1 always leave out too save memory

        public static Matrix2D Identity { get; } = new Matrix2D(1, 0, 0, 1, 0, 0);

        public Vector2 Translation { get { return new Vector2(M31, M32); } }

        public float Rotation { get { return (float)Math.Atan2(M21, M11); } }

        public Vector2 Scale
        {
            get
            {
                // I think this shoulb be M11 * M11 + M12 * M12 etc as where row vectors?
                return new Vector2((float)Math.Sqrt(M11 * M11 + M21 * M21), (float)Math.Sqrt(M12 * M12 + M22 * M22));
            }
        }

        public Matrix2D(float m11, float m12, float m21, float m22, float m31, float m32)
        {
            M11 = m11;
            M12 = m12;

            M21 = m21;
            M22 = m22;

            M31 = m31;
            M32 = m32;
        }

        public Matrix2D CreateTransform(Vector2 position, float rotation, Vector2 scale)
        {
            return CreateTranslation(position) * CreateRotation(rotation) * CreateScale(scale);
        }

        public float Determinant()
        {
            return M11 * M22 - M12 * M21;
        }

        public Vector2 Transform(Vector2 vector)
        {
            Vector2 result;
            Transform(vector, out result);
            return result;
        }

        public void Transform(Vector2 vector, out Vector2 product)
        {
            product.X = vector.X * M11 + vector.Y * M21 + M31;
            product.Y = vector.X * M12 + vector.Y * M22 + M32;
        }

        public void Transform(float x, float y, out Vector2 product)
        {
            product.X = x * M11 + y * M21 + M31;
            product.Y = x * M12 + y * M22 + M32;
        }

        public void Transform(float x, float y, ref Vector3 product)
        {
            product.X = x * M11 + y * M21 + M31;
            product.Y = x * M12 + y * M22 + M32;
        }

        public static Matrix2D CreateRotation(float radians)
        {
            Matrix2D product;
            CreateRotation(radians, out product);
            return product;
        }

        public static void CreateRotation(float radians, out Matrix2D product)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            product = new Matrix2D(cos, sin, -sin, cos, 0, 0);
        }

        public static Matrix2D CreateScale(float scale)
        {
            Matrix2D product;
            CreateScale(scale, out product);
            return product;
        }

        public static void CreateScale(float scale, out Matrix2D product)
        {
            product = new Matrix2D(scale, 0, 0, scale, 0, 0);
        }

        public static Matrix2D CreateScale(float x, float y)
        {
            Matrix2D product;
            CreateScale(x, y, out product);
            return product;
        }

        public static void CreateScale(float x, float y, out Matrix2D product)
        {
            product = new Matrix2D(x, 0, 0, y, 0, 0);
        }

        public static Matrix2D CreateScale(Vector2 scale)
        {
            Matrix2D product;
            CreateScale(scale, out product);
            return product;
        }

        public static void CreateScale(Vector2 scale, out Matrix2D product)
        {
            product = new Matrix2D(scale.X, 0, 0, scale.Y, 0, 0);
        }

        public static Matrix2D CreateTranslation(float x, float y)
        {
            Matrix2D product;
            CreateTranslation(x, y, out product);
            return product;
        }

        public static void CreateTranslation(float x, float y, out Matrix2D product)
        {
            product = new Matrix2D(0, 0, 0, 0, x, y);
        }

        public static Matrix2D CreateTranslation(Vector2 position)
        {
            Matrix2D product;
            CreateTranslation(position, out product);
            return product;
        }

        public static void CreateTranslation(Vector2 position, out Matrix2D product)
        {
            product = new Matrix2D(1, 0, 0, 1, position.X, position.Y);
        }

        public static Matrix2D operator *(Matrix2D a, Matrix2D b)
        {
            float m11 = a.M11 * b.M11 + a.M12 * b.M21;
            float m12 = a.M11 * b.M12 + a.M12 * b.M22;

            float m21 = a.M21 * b.M11 + a.M22 * b.M21;
            float m22 = a.M21 * b.M12 + a.M22 * b.M22;

            float m31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31;
            float m32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32;

            Matrix2D product = new Matrix2D(m11, m12,
                                            m21, m22,
                                            m31, m32);

            return product;
        }

        public static Matrix2D operator *(Matrix2D matrix, float scalar)
        {
            Matrix2D product = new Matrix2D(
            matrix.M11 * scalar,
            matrix.M12 * scalar,
            matrix.M21 * scalar,
            matrix.M22 * scalar,
            matrix.M31 * scalar,
            matrix.M32 * scalar);

            return product;
        }

        public static Matrix2D operator +(Matrix2D matrix1, Matrix2D matrix2)
        {
            Matrix2D matrix = new Matrix2D(
            matrix1.M11 + matrix2.M11,
            matrix1.M12 + matrix2.M12,
            matrix1.M21 + matrix2.M21,
            matrix1.M22 + matrix2.M22,
            matrix1.M31 + matrix2.M31,
            matrix1.M32 + matrix2.M32);

            return matrix;
        }

        public static Matrix2D Invert(Matrix2D matrix)
        {
            Matrix2D product;
            Invert(ref matrix, out product);
            return product;
        }

        public static void Invert(ref Matrix2D matrix, out Matrix2D result)
        {
            // Take matrix of minors, M11, M12, M21, M22 just get single value as
            // 0 cancels the - bracket term out i.e M11 = (M22 * M33(1.0f)) - (M23(0.0f) * M32) == M22
            // Note we can ignore M31 and M32 as there value comes from M13 and M23 instead! due to Adjoint!
            // So we save Memory

            // With Minors now do the cofactors, i.e put negatives in front of M12, M12, M32
            // Now calculate adjoint, basically swap M12 with M22, M31 with M13 etc
            // Finally divide by Determinant

            // Below all steps are performned in 1 go
            float det = 1.0f / matrix.Determinant();

            result = new Matrix2D
            (
                 matrix.M22 * det,
                -matrix.M12 * det,
                -matrix.M21 * det,
                 matrix.M11 * det,
                 (matrix.M32 * matrix.M21 - matrix.M31 * matrix.M22) * det,
                -(matrix.M32 * matrix.M11 - matrix.M31 * matrix.M12) * det
            );
        }

        public static bool operator ==(Matrix2D a, Matrix2D b)
        {
            return (a.M11 == b.M11) && (a.M12 == b.M12) && (a.M21 == b.M21) &&
                   (a.M22 == b.M22) && (a.M31 == b.M31) && (a.M32 == b.M32);
        }

        public static bool operator !=(Matrix2D a, Matrix2D b)
        {
            return (a.M11 != b.M11) || (a.M12 != b.M12) || (a.M21 != b.M21) ||
                   (a.M22 != b.M22) || (a.M31 != b.M31) || (a.M32 != b.M32);
        }

        public static implicit operator Matrix(Matrix2D matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, 0, 0, matrix.M21, matrix.M22,
                              0, 0, 0, 0, 1, 0, matrix.M31, matrix.M32, 0, 1);
        }

        // The Following Functions must be implemented otherwise C# cries
        public bool Equals(Matrix2D matrix)
        {
            return Equals(ref matrix);
        }

        public override bool Equals(object obj)
        {
            return obj is Matrix2D && Equals((Matrix2D)obj);
        }

        public bool Equals(ref Matrix2D matrix)
        {
            return (M11 == matrix.M11) && (M12 == matrix.M12) && (M21 == matrix.M21) && (M22 == matrix.M22) &&
                   (M31 == matrix.M31) && (M32 == matrix.M32);
        }

        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + M21.GetHashCode() +
                   M22.GetHashCode() + M31.GetHashCode() + M32.GetHashCode();
        }

        public override string ToString()
        {
            return $"{{M11:{M11} M12:{M12}}} {{M21:{M21} M22:{M22}}} {{M31:{M31} M32:{M32}}}";
        }
    }
}
