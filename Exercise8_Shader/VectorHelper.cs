using OpenTK.Mathematics;
using System;

namespace Exercise8_Shader
{
    public static class VectorHelper
    {
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            return new Vector2(
                v.X * MathF.Cos(angle) - v.Y * MathF.Sin(angle),
                v.X * MathF.Sin(angle) + v.Y * MathF.Cos(angle));
        }
    }
}
