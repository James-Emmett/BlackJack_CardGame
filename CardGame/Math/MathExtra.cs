using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public static class MathExtra
    { 
        /// <summary>
        /// Angle Between two Vectors
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float Angle(Vector2 A, Vector2 B)
        {
            return (float)Math.Atan2(MathF.Abs(A.X * B.Y - B.X * A.Y), Vector2.Dot(A, B));
        }

        /// <summary>
        /// Angle you would have to rotate from A's direction to face B's tip
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static float AngleDiffernce(Vector2 A, Vector2 B)
        {
            return (float)Math.Atan2(B.Y - A.Y, B.X - A.X);
        }

        public static Vector2 PointInCircle(float radius)
        {
            float r = MathF.Sqrt((float)Engine.Random.NextDouble()) * radius;
            float a = (float)Engine.Random.NextDouble() * MathHelper.TwoPi;

            return new Vector2(MathF.Cos(a) * r, MathF.Sin(a) * r);
        }

        public static Vector2 PointOnCircle(float radius)
        {
            float a = (float)Engine.Random.NextDouble() * MathHelper.TwoPi;

            return new Vector2(MathF.Cos(a) * radius, MathF.Sin(a) * radius);
        }
    }
}
