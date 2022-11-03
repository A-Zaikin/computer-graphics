using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public static class PolygonHelper
    {
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

        public static bool IsVectorPointingInsidePolygon(Vector2[] surface, int surfacePointIndex, Vector2 otherPoint)
        {
            var previousPoint = surface[surfacePointIndex == 0
                ? surface.Length - 1
                : surfacePointIndex - 1];
            var nextPoint = surface[(surfacePointIndex + 1) % surface.Length];

            var currentPoint = surface[surfacePointIndex];
            var polygonAngle = VectorHelper.GetCcwAngle(previousPoint - currentPoint, nextPoint - currentPoint);
            var otherSurfaceAngle = VectorHelper.GetCcwAngle(previousPoint - currentPoint,
                otherPoint - currentPoint);
            return otherSurfaceAngle > 0 && otherSurfaceAngle < polygonAngle;
        }

        public static float[] ToVertices(this Vector3[] points)
        {
            return points.SelectMany(point => new float[3] { point.X, point.Y, point.Z }).ToArray();
        }

        public static IEnumerable<int> TriangulateIndices(Vector3[] points, bool reverse = false)
        {
            List<int> indices = new();
            for (LoopIndex i = new(points, 1); i < points.Length - 1; i += 1)
            {
                if (reverse)
                {
                    indices.AddRange(new int[] { 0, i + 1, i });
                }
                else
                {
                    indices.AddRange(new int[] { 0, i, i + 1 });
                }
            }
            return indices;
        }
    }
}
