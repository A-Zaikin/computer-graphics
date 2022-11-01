using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public class Prismatoid : Polyhedron
    {
        public Prismatoid(Vector2[] surface1, Vector2[] surface2, Vector3 color = default)
            : base(CreateShape(surface1, surface2), color)
        {
        }

        private static Shape CreateShape(Vector2[] surface1, Vector2[] surface2)
        {
            var shape = new Shape(surface1.Length + surface2.Length);
            for (var i = 0; i < surface1.Length; i++)
            {
                shape.Points[i] = new Vector3(surface1[i].X, 1, surface1[i].Y);
            }
            for (var j = 0; j < surface2.Length; j++)
            {
                shape.Points[surface1.Length + j] = new Vector3(surface2[j].X, -1, surface2[j].Y);
            }

            var polyhedronSides = new bool[surface1.Length, surface2.Length];
            var indexCount = 0;
            for (var i = 0; i < surface1.Length; i++)
            {
                for (var j = 0; j < surface2.Length; j++)
                {
                    if (IsVectorPointingInsidePolygon(surface1, i, surface2[j])
                        && IsVectorPointingInsidePolygon(surface2, j, surface1[i]))
                    {
                        continue;
                    }
                    polyhedronSides[i, j] = true;
                    indexCount += 4;
                }
            }

            // resolve quads
            for (var i = 0; i < surface1.Length; i++)
            {
                for (var j = 0; j < surface2.Length; j++)
                {
                    if (polyhedronSides[i, j]
                        && polyhedronSides[(i + 1) % surface1.Length, j]
                        && polyhedronSides[i, (j + 1) % surface2.Length]
                        && polyhedronSides[(i + 1) % surface1.Length, (j + 1) % surface2.Length])
                    {
                        var line1 = surface2[(j + 1) % surface2.Length] - surface1[i];
                        var distance1 = Distance(surface1[i], line1, surface1[(i + 1) % surface1.Length]);
                        var line2 = surface2[j] - surface1[(i + 1) % surface1.Length];
                        var distance2 = Distance(surface1[(i + 1) % surface1.Length], line2, surface1[i]);
                        if (distance1 > distance2)
                        {
                            polyhedronSides[(i + 1) % surface1.Length, j] = false;
                        }
                        else
                        {
                            polyhedronSides[i, (j + 1) % surface2.Length] = false;
                        }
                    }
                }
            }

            for (var i = 0; i < surface1.Length; i++)
            {
                for (var j = 0; j < surface2.Length; j++)
                {
                    if (polyhedronSides[i, j] && polyhedronSides[i, (j + 1) % surface2.Length])
                    {
                        shape.Indices.AddRange(new int[] { i, surface1.Length + j,
                            surface1.Length + (j + 1) % surface2.Length });
                    }
                }
            }

            for (var j = 0; j < surface2.Length; j++)
            {
                for (var i = 0; i < surface1.Length; i++)
                {
                    if (polyhedronSides[i, j] && polyhedronSides[(i + 1) % surface1.Length, j])
                    {
                        shape.Indices.AddRange(new int[] { surface1.Length + j, (i + 1) % surface1.Length, i });
                    }
                }
            }

            for (var i = 1; i < surface1.Length - 1; i++)
            {
                shape.Indices.AddRange(new int[] { 0, i, (i + 1) % surface1.Length });
            }
            for (var j = 1; j < surface2.Length - 1; j++)
            {
                shape.Indices.AddRange(new int[] { surface1.Length,
                    surface1.Length + (j + 1) % surface2.Length, surface1.Length + j });
            }

            return shape;


            bool IsVectorPointingInsidePolygon(Vector2[] surface, int currentPointIndex, Vector2 otherPolygonPoint)
            {
                var previousPoint = surface[currentPointIndex == 0
                    ? surface.Length - 1
                    : currentPointIndex - 1];
                var nextPoint = surface[(currentPointIndex + 1) % surface.Length];

                var currentPoint = surface[currentPointIndex];
                var polygonAngle = GetCcwAngle(previousPoint - currentPoint, nextPoint - currentPoint);
                var otherSurfaceAngle = GetCcwAngle(previousPoint - currentPoint, otherPolygonPoint - currentPoint);
                return otherSurfaceAngle > 0 && otherSurfaceAngle < polygonAngle;
            }


            float GetCcwAngle(Vector2 p1, Vector2 p2)
            {
                var dot = p1.X * p2.X + p1.Y * p2.Y;
                var det = p1.X * p2.Y - p1.Y * p2.X;
                return (float)MathHelper.Atan2(det, dot); //atan2(sin, cos)
            }


            float Distance(Vector2 lineStart, Vector2 lineDirection, Vector2 point)
            {
                var numerator = MathHelper.Abs(lineDirection.Y * point.X
                    - lineDirection.X * point.Y
                    - lineDirection.Y * lineStart.X
                    + lineDirection.X * lineStart.Y);
                var denominator = MathHelper.Sqrt(point.X * point.X + point.Y * point.Y);
                return (float)(numerator / denominator);
            }

            //IEnumerable<int> TriangulateSurface(Vector2[] surface)
            //{
            //    var indices = new List<int>();
            //    var list = new LinkedList<Vector2>(surface);
            //    var vertex = list.First.Next;
            //    while(vertex != list.Last)
            //    {
            //        var hasPointInside = false;
            //        for (var j = 0; j < list.Count; j++)
            //        {
            //            if (IsPointInTriangle(list.ElementAt(j),
            //                vertex.Previous.Value, vertex.Value, vertex.Next.Value))
            //            {
            //                hasPointInside = true;
            //                break;
            //            }
            //        }
            //        if (hasPointInside)
            //        {
            //            continue;
            //        }
            //        indices.AddRange(new int[] { vertex.Previous.Value, vertex.Next.Value, vertex.Value });
            //        var nextVertex = vertex.Next;
            //        list.Remove(vertex);
            //        vertex = nextVertex;
            //    }


            //    bool IsPointInTriangle(Vector2 point, Vector2 vertex1, Vector2 vertex2, Vector2 vertex3)
            //    {
            //        var d1 = Sign(point, vertex1, vertex2);
            //        var d2 = Sign(point, vertex2, vertex3);
            //        var d3 = Sign(point, vertex3, vertex1);

            //        var isNegative = (d1 < 0) || (d2 < 0) || (d3 < 0);
            //        var isPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

            //        return !(isNegative && isPositive);


            //        float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
            //        {
            //            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
            //        }
            //    }
            //}
        }
    }
}
