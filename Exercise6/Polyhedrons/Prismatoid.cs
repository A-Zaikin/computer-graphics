using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public static class Prismatoid
    {
        public static Polyhedron CreateGeneral(Vector2[] smallSurface, Vector2[] bigSurface, float height)
        {
            // add connection if a point from one polygon can see a point from other polygon
            var planeConnections = new bool[smallSurface.Length, bigSurface.Length];
            for (var i = 0; i < smallSurface.Length; i++)
            {
                for (var j = 0; j < bigSurface.Length; j++)
                {
                    if (PolygonHelper.IsVectorPointingInsidePolygon(smallSurface, i, bigSurface[j])
                        && PolygonHelper.IsVectorPointingInsidePolygon(bigSurface, j, smallSurface[i]))
                    {
                        continue;
                    }
                    planeConnections[i, j] = true;
                }
            }

            // choose the furthest of the two diagonals of each quad
            var planeConnectionsCopy = planeConnections.Clone() as bool[,];
            for (LoopIndex i = new(smallSurface); !i.HasLooped; i += 1)
            {
                for (LoopIndex j = new(bigSurface); !j.HasLooped; j += 1)
                {
                    if (planeConnectionsCopy[i, j]
                        && planeConnectionsCopy[i + 1, j]
                        && planeConnectionsCopy[i, j + 1]
                        && planeConnectionsCopy[i + 1, j + 1])
                    {
                        var side1 = smallSurface[i + 1] - smallSurface[i];
                        var side2 = bigSurface[j + 1] - bigSurface[j];
                        if (Vector2.Dot(side1, side2) < 0)
                        {
                            continue;
                        }

                        var distance1 = VectorHelper.Distance(smallSurface[i],
                            bigSurface[j + 1], smallSurface[i + 1]);
                        var distance2 = VectorHelper.Distance(smallSurface[i + 1],
                            bigSurface[j], smallSurface[i]);

                        if (distance1 > distance2)
                        {
                            planeConnections[i + 1, j] = false;
                        }
                        else
                        {
                            planeConnections[i, j + 1] = false;
                        }
                    }
                }
            }

            var plane1 = smallSurface.Select(p => new Vector3(p.X, height / 2, p.Y)).ToArray();
            var plane2 = bigSurface.Select(p => new Vector3(p.X, -height / 2, p.Y)).ToArray();
            List<int> sideIndices = new();

            // add indicies of connected points from two polygons
            for (LoopIndex i = new(plane1); !i.HasLooped; i += 1)
            {
                for (LoopIndex j = new(plane2); !j.HasLooped; j += 1)
                {
                    if (planeConnections[i, j] && planeConnections[i, j + 1])
                    {
                        if (!smallSurface.Any(point => PolygonHelper.IsPointInTriangle(point,
                                smallSurface[i], bigSurface[j], bigSurface[j + 1])))
                        {
                            sideIndices.AddRange(new int[] { i, plane1.Length + j.Value,
                                plane1.Length + (j + 1).Value });
                        }
                    }
                    if (planeConnections[i, j] && planeConnections[i + 1, j])
                    {
                        if (!smallSurface.Any(point => PolygonHelper.IsPointInTriangle(point,
                                bigSurface[j], smallSurface[i], smallSurface[i + 1])))
                        {
                            sideIndices.AddRange(new int[] { plane1.Length + j.Value, i + 1, i });
                        }
                    }
                }
            }

            var points = plane1.Concat(plane2).ToArray();
            var indices = PolygonHelper.Triangulate(plane1)
                .Concat(PolygonHelper.Triangulate(plane2, true)
                    .Select(index => index + plane1.Length))
                .Concat(sideIndices)
                .ToArray();
            return new Polyhedron(points, indices);
        }

        public static Polyhedron CreatePyramid(Vector2[] surface, float height, Vector2 apex = default)
        {
            var plane = surface.Select(p => new Vector3(p.X, -height / 2, p.Y)).ToArray();
            List<int> sideIndices = new();

            // add indicies of connected points
            for (LoopIndex i = new(plane); !i.HasLooped; i += 1)
            {
                sideIndices.AddRange(new int[] { 0, i.Value + 1, (i + 1).Value + 1 });
            }

            var points = plane.Prepend(new Vector3(apex.X, height / 2, apex.Y)).ToArray();
            var indices = PolygonHelper.Triangulate(plane, true)
                .Select(index => index + 1)
                .Concat(sideIndices).ToArray();
            return new Polyhedron(points, indices);
        }

        public static Polyhedron CreateParallelepiped(float widthX, float widthZ, float height,
            Vector2 topOffset = default)
        {
            var plane1 = new Vector2[]
            {
                new Vector2(-widthX / 2, -widthZ / 2),
                new Vector2(-widthX / 2, widthZ / 2),
                new Vector2(widthX / 2, widthZ / 2),
                new Vector2(widthX / 2, -widthZ / 2),
            };
            var plane2 = plane1.Clone() as Vector2[];
            if (topOffset != default)
            {
                plane1 = plane1.Select(vertex => vertex + topOffset).ToArray();
            }
            return CreateGeneral(plane1, plane2, height);
        }
    }
}
