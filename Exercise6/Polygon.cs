using OpenTK.Mathematics;
using System;

namespace Exercise6
{
    public class Polygon
    {
        public readonly Vector2[] Points;

        public Polygon(params Vector2[] points)
        {
            Points = points;
        }

        public static Vector2[] CreateRegular(Vector2 center, float radius,
            int vertexCount, float angleOffset = 0)
        {
            var points = new Vector2[vertexCount];
            for (var i = 0; i < vertexCount; i++)
            {
                points[i] = new Vector2(
                    center.X + radius * MathF.Cos(2 * MathF.PI * -i / vertexCount + angleOffset),
                    center.Y + radius * MathF.Sin(2 * MathF.PI * -i / vertexCount + angleOffset));
            }
            return points;
        }
    }
}
