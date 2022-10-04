using OpenTK.Mathematics;
using System;

namespace Exercise2
{
    public class RegularPolygon : Polygon
    {
        public float Radius;

        public RegularPolygon(Vector2 center, float radius, int vertexCount,
            float angleOffset = 0, Vector3 color = default)
            : base(ComputePoints(center, radius, vertexCount, angleOffset), color)
        {
        }

        private static Vector2[] ComputePoints(Vector2 center, float radius, 
            int vertexCount, float angleOffset)
        {
            var points = new Vector2[vertexCount];
            for (var i = 0; i < vertexCount; i++)
            {
                points[i] = new Vector2(
                    center.X + radius * MathF.Cos(2 * MathF.PI * i / vertexCount + angleOffset),
                    center.Y + radius * MathF.Sin(2 * MathF.PI * i / vertexCount + angleOffset));
            }
            return points;
        }
    }
}
