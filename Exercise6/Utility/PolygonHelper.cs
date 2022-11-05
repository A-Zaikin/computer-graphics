using OpenTK.Mathematics;
using System;
using System.Collections;
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

        public static IEnumerable<int> Triangulate(int length, bool reverse = false)
        {
            List<int> indices = new();
            for (LoopIndex i = new(length, 1); i < length - 1; i += 1)
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

        public static IEnumerable<int> Triangulate(IList list, bool reverse = false)
        {
            return Triangulate(list.Count, reverse);
        }

        public static bool IsPointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            if (pt == v1 || pt == v2 || pt == v3)
            {
                return false;
            }

            var d1 = Sign(pt, v1, v2);
            var d2 = Sign(pt, v2, v3);
            var d3 = Sign(pt, v3, v1);

            var hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0);
            var hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNegative && hasPositive);


            static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
            {
                return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
            }
        }

        public static void Sort(Vector2[] points)
        {
            var center = points.Aggregate((a, b) => a + b) / points.Length;
            Array.Sort(points, (p1, p2) =>
            {
                var vector1 = p1 - center;
                var angle1 = (MathF.Atan2(vector1.X, vector1.Y) + MathF.PI * 2) % (MathF.PI * 2);
                var vector2 = p1 - center;
                var angle2 = (MathF.Atan2(vector2.X, vector2.Y) + MathF.PI * 2) % (MathF.PI * 2);
                return angle1.CompareTo(angle2);
            });
        }

        public static void SortIndices(Vector3[] allPoints, int[] indices)
        {
            var center = indices.Select(i => allPoints[i]).Aggregate((a, b) => a + b);
            var normal = center;
            var p = Vector3.MagnitudeMax(
                Vector3.Cross(Vector3.UnitX, normal),
                Vector3.MagnitudeMax(
                    Vector3.Cross(Vector3.UnitY, normal),
                    Vector3.Cross(Vector3.UnitZ, normal)));
            var q = Vector3.Cross(normal, p);
            Array.Sort(indices, (i1, i2) => ComputeAngle(allPoints[i1]).CompareTo(ComputeAngle(allPoints[i2])));


            float ComputeAngle(Vector3 point)
            {
                var t = Vector3.Dot(normal, Vector3.Cross(point - center, p));
                var u = Vector3.Dot(normal, Vector3.Cross(point - center, q));
                return MathF.Atan2(u, t);
            }
        }
    }
}
