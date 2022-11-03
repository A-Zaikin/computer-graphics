using OpenTK.Mathematics;
using System;
using System.Linq;

namespace Exercise6
{
    public static class VectorHelper
    {
        public static float GetCcwAngle(Vector2 p1, Vector2 p2)
        {
            var dot = p1.X * p2.X + p1.Y * p2.Y;
            var det = p1.X * p2.Y - p1.Y * p2.X;
            return (float)MathHelper.Atan2(det, dot); //atan2(sin, cos)
        }

        public static float Distance(Vector2 lineStart, Vector2 linePoint, Vector2 point)
        {
            var lineDirection = linePoint - lineStart;
            var numerator = MathHelper.Abs(lineDirection.Y * point.X
                - lineDirection.X * point.Y
                - lineDirection.Y * lineStart.X
                + lineDirection.X * lineStart.Y);
            var denominator = MathHelper.Sqrt(point.X * point.X + point.Y * point.Y);
            return (float)(numerator / denominator);
        }

        public static Vector2 Rotate(Vector2 p, float angle)
        {
            return new Vector2(
                p.X * MathF.Cos(angle) - p.Y * MathF.Sin(angle),
                p.X * MathF.Sin(angle) + p.Y * MathF.Cos(angle));
        }
    }
}
