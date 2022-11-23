using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public static class ComplexShape
    {
        public static Polyhedron[] CreateHelix(float radius, float thickness,
            int sectionsPerRotation, int sectionVertexCount,
            float height, int rotationCount, float offsetAngle = 0, bool round = false)
        {
            var sectionCount = sectionsPerRotation * rotationCount;
            var points = new Vector3[sectionVertexCount * sectionCount];
            List<int> indices = new();
            var normals = new Vector3[points.Length];

            var firstPlane = new Vector3[sectionVertexCount];
            var lastPlane = new Vector3[sectionVertexCount];
            for (var i = 0; i < sectionCount; i++)
            {
                var angle = 2 * MathF.PI * -i / sectionCount;
                for (LoopIndex j = new(sectionVertexCount); !j.HasLooped; j += 1)
                {
                    var pointPosition = new Vector3(
                        radius + thickness * MathF.Cos(2 * MathF.PI * -j / sectionVertexCount + offsetAngle),
                        -height / 2 + thickness * MathF.Sin(2 * MathF.PI * -j / sectionVertexCount + offsetAngle),
                        0);

                    pointPosition.Xz = VectorHelper.Rotate(pointPosition.Xz, -angle * rotationCount);
                    pointPosition.Y += i * height / sectionCount;
                    points[i * sectionVertexCount + j] = pointPosition;

                    var center = new Vector3(radius, -height / 2 + i * height / sectionCount, 0);
                    center.Xz = VectorHelper.Rotate(center.Xz, -angle * rotationCount);
                    normals[i * sectionVertexCount + j] = (pointPosition - center).Normalized();

                    if (i == 0)
                    {
                        firstPlane[j] = pointPosition;
                        continue;
                    }
                    if (i == sectionCount - 1)
                    {
                        lastPlane[j] = pointPosition;
                    }


                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + j, i * sectionVertexCount + (j + 1).Value});
                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + (j + 1).Value, (i - 1) * sectionVertexCount + (j + 1).Value });
                }
            }

            var planeIndices = PolygonHelper.Triangulate(firstPlane)
                .Concat(PolygonHelper.Triangulate(lastPlane, true)
                    .Select(i => i + firstPlane.Length))
                .ToArray();

            return new Polyhedron[2]
            {
                new Polyhedron(points, indices.ToArray(), normals, round),
                new Polyhedron(firstPlane.Concat(lastPlane).ToArray(), planeIndices, null, false),
            };
        }

        public static Polyhedron[] CreateCylinder(int vertexCount, float topRadius, float bottomRadius,
            float height, bool round = false)
        {
            List<int> indices = new();
            var normals = new Vector3[vertexCount * 2];

            var topPlane = PolygonHelper.CreateRegular(Vector2.Zero, topRadius, vertexCount)
                .Select(vec => new Vector3(vec.X, 0, vec.Y)).ToArray();
            var bottomPlane = PolygonHelper.CreateRegular(Vector2.Zero, bottomRadius, vertexCount)
                .Select(vec => new Vector3(vec.X, 0, vec.Y)).ToArray();

            var coneAngle = MathF.Atan((bottomRadius - topRadius) / height);

            for (var i = 0; i < vertexCount; i++)
            {
                var normal = bottomPlane[i].Normalized(); 
                if (bottomRadius != topRadius)
                {
                    normal *= MathF.Cos(coneAngle);
                    normal += Vector3.UnitY * MathF.Sin(coneAngle);
                }
                normals[i] = normal;
                normals[i + vertexCount] = normal;
            }

            for (LoopIndex i = new(vertexCount); !i.HasLooped; i += 1)
            {
                indices.AddRange(new int[] { i - 1, vertexCount + i, i });
                indices.AddRange(new int[] { vertexCount + i, i - 1, vertexCount + (i - 1) });
            }

            for (var i = 0; i < vertexCount; i++)
            {
                topPlane[i].Y = height / 2;
                bottomPlane[i].Y = -height / 2;
            }
            var points = topPlane.Concat(bottomPlane).ToArray();
            var planeIndices = PolygonHelper.Triangulate(topPlane)
                .Concat(PolygonHelper.Triangulate(bottomPlane, true).Select(i => i + vertexCount)).ToArray();
            return new Polyhedron[2]
            {
                new Polyhedron(points, planeIndices, null, false),
                new Polyhedron(points, indices.ToArray(), normals, round),
            };
        }
    }
}
