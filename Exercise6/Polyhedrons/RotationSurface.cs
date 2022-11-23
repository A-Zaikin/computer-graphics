using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    public static class RotationSurfaces
    {
        public static Polyhedron CreateTorus(float radius, float thickness,
            int sectionCount, int sectionVertexCount, float offsetAngle = 0, bool round = false)
        {
            var points = new Vector3[sectionVertexCount * sectionCount];
            List<int> indices = new();
            var normals = new Vector3[points.Length];

            for (LoopIndex i = new(sectionCount); !i.HasLooped; i += 1)
            {
                var angle = MathHelper.Pi * 2 * -i / sectionCount;
                for (LoopIndex j = new(sectionVertexCount); !j.HasLooped; j += 1)
                {
                    var pointPosition = new Vector3(
                        radius + thickness * MathF.Cos(2 * MathF.PI * -j / sectionVertexCount + offsetAngle),
                        thickness * MathF.Sin(2 * MathF.PI * -j / sectionVertexCount + offsetAngle), 0);

                    pointPosition.Xz = VectorHelper.Rotate(pointPosition.Xz, -angle);
                    points[i * sectionVertexCount + j] = pointPosition;

                    var center = new Vector3(radius, 0, 0);
                    center.Xz = VectorHelper.Rotate(center.Xz, -angle);
                    normals[i * sectionVertexCount + j] = (pointPosition - center).Normalized();

                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + j, i * sectionVertexCount + (j + 1).Value});
                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + (j + 1).Value, (i - 1) * sectionVertexCount + (j + 1).Value });
                }
            }
            return new Polyhedron(points, indices.ToArray(), normals, round);
        }

        public static Polyhedron CreateSphere(float radius, int sectionCount, int sideVertexCount)
        {
            var points = new Vector3[sideVertexCount * sectionCount + 2];
            List<int> indices = new();
            var normals = new Vector3[points.Length];

            points[0] = Vector3.UnitY * radius;
            normals[0] = Vector3.UnitY;
            points[1] = -Vector3.UnitY * radius;
            normals[1] = -Vector3.UnitY;
            for (LoopIndex i = new(sectionCount); !i.HasLooped; i += 1)
            {
                var angle = MathF.PI * 2 * -i / sectionCount;
                for (var j = 0; j < sideVertexCount; j += 1)
                {
                    var pointPosition = new Vector3(
                        radius * MathF.Cos(MathF.PI / 2 + MathF.PI * -(j + 1) / (sideVertexCount + 1)),
                        radius * MathF.Sin(MathF.PI / 2 + MathF.PI * -(j + 1) / (sideVertexCount + 1)), 0);

                    pointPosition.Xz = VectorHelper.Rotate(pointPosition.Xz, -angle);
                    points[i * sideVertexCount + j + 2] = pointPosition;
                    normals[i * sideVertexCount + j + 2] = pointPosition.Normalized();

                    if (j == 0)
                    {
                        indices.AddRange(new int[] { (i - 1) * sideVertexCount + j,
                            -2, i * sideVertexCount + j });
                    }

                    if (j == sideVertexCount - 1)
                    {
                        indices.AddRange(new int[] { (i - 1) * sideVertexCount + j,
                            i * sideVertexCount + j, -1 });
                    }
                    else
                    {
                        indices.AddRange(new int[] { (i - 1) * sideVertexCount + j,
                            i * sideVertexCount + j, i * sideVertexCount + j + 1 });
                        indices.AddRange(new int[] { (i - 1) * sideVertexCount + j,
                            i * sideVertexCount + j + 1, (i - 1) * sideVertexCount + j + 1 });
                    }
                }
            }
            return new Polyhedron(points, indices.Select(i => i + 2).ToArray(), normals, true);
        }

        public static Polyhedron CreateSphereFromTriangles(float radius)
        {
            var f = (1 + MathF.Sqrt(5)) / 2; // golden ratio
            var points1 = VectorHelper.MirrorOrthogonally(new Vector3(0, 1, f), Vector3.Zero);
            var points2 = VectorHelper.MirrorOrthogonally(new Vector3(1, f, 0), Vector3.Zero);
            var points3 = VectorHelper.MirrorOrthogonally(new Vector3(f, 0, 1), Vector3.Zero);
            var points = points1.Concat(points2).Concat(points3).ToArray();
            List<int> indices = new();

            List<Vector3> subdividedPoints = new();
            foreach (var triangle in Combinations.Create(points.Select((p, i) => i).ToArray(), 3))
            {
                if (Vector3.Distance(points[triangle[0]], points[triangle[1]]) > 2 * 1.001f
                    || Vector3.Distance(points[triangle[0]], points[triangle[2]]) > 2 * 1.001f
                    || Vector3.Distance(points[triangle[1]], points[triangle[2]]) > 2 * 1.001f)
                {
                    continue;
                }
                //PolygonHelper.SortIndices(points, triangle);
                //indices.AddRange(triangle);
                subdividedPoints.Add((points[triangle[0]] + points[triangle[1]]) / 2);
                subdividedPoints.Add((points[triangle[0]] + points[triangle[2]]) / 2);
                subdividedPoints.Add((points[triangle[1]] + points[triangle[2]]) / 2);
            }
            points = points.Concat(subdividedPoints).ToArray();

            List<Vector3> subdividedPoints2 = new();
            foreach (var triangle in Combinations.Create(points.Select((p, i) => i).ToArray(), 3))
            {
                if (Vector3.Distance(points[triangle[0]], points[triangle[1]]) > 1 * 1.001f
                    || Vector3.Distance(points[triangle[0]], points[triangle[2]]) > 1 * 1.001f
                    || Vector3.Distance(points[triangle[1]], points[triangle[2]]) > 1 * 1.001f)
                {
                    continue;
                }
                PolygonHelper.SortIndices(points, triangle);

                subdividedPoints2.Add((points[triangle[0]] + points[triangle[1]]) / 2);
                subdividedPoints2.Add((points[triangle[1]] + points[triangle[2]]) / 2);
                subdividedPoints2.Add((points[triangle[0]] + points[triangle[2]]) / 2);
                var n2 = subdividedPoints2.Count - 1 + points.Length;
                var n1 = n2 - 1;
                var n0 = n1 - 1;
                indices.AddRange(new int[]
                {
                    triangle[0], n0, n2,
                    n0, triangle[1], n1,
                    n2, n1, triangle[2],
                    n0, n1, n2,
                });
            }
            points = points.Concat(subdividedPoints2).ToArray();

            var normals = points.Select(point => point.Normalized()).ToArray();
            points = normals.Select(point => point * radius).ToArray();

            return new Polyhedron(points, indices.ToArray(), normals, true);
        }
    }
}
