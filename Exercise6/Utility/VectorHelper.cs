using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
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

        public static Vector3 Abs(Vector3 vector)
        {
            return new Vector3(MathHelper.Abs(vector.X), MathHelper.Abs(vector.Y), MathHelper.Abs(vector.Z));
        }

        public static IEnumerable<Vector3> MirrorOrthogonally(Vector3 point, Vector3 translate, float scale = 1)
        {
            point = Abs(point);
            List<Vector3> points = new();
            for (var x = -1; x < MathHelper.Sign(point.X) * 2; x += 2)
                for (var y = -1; y < MathHelper.Sign(point.Y) * 2; y += 2)
                    for (var z = -1; z < MathHelper.Sign(point.Z) * 2; z += 2)
                        points.Add(new Vector3(point.X * x, point.Y * y, point.Z * z) + translate);
            return points.Select(p => p * scale);
        }
    }
}
