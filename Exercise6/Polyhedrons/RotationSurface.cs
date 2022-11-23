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
            int sectionCount, int sectionVertexCount, float offsetAngle = 0)
        {
            var points = new Vector3[sectionVertexCount * sectionCount];
            List<int> indices = new();

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

                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + j, i * sectionVertexCount + (j + 1).Value});
                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + (j + 1).Value, (i - 1) * sectionVertexCount + (j + 1).Value });
                }
            }
            return new Polyhedron(points, indices.ToArray());
        }

        public static Polyhedron CreateHelix(float radius, float thickness,
            int sectionsPerRotation, int sectionVertexCount,
            float height, int rotationCount, float offsetAngle = 0)
        {
            var sectionCount = sectionsPerRotation * rotationCount;
            var points = new Vector3[sectionVertexCount * sectionCount];
            List<int> indices = new();

            var polygon = new Vector3[sectionVertexCount];
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

                    if (i == 0)
                    {
                        polygon[j] = pointPosition;
                        continue;
                    }

                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + j, i * sectionVertexCount + (j + 1).Value});
                    indices.AddRange(new int[] { (i - 1) * sectionVertexCount + j,
                            i * sectionVertexCount + (j + 1).Value, (i - 1) * sectionVertexCount + (j + 1).Value });
                }
            }

            var totalIndices = indices
                .Concat(PolygonHelper.Triangulate(polygon))
                .Concat(PolygonHelper.Triangulate(polygon, true)
                    .Select(i => i + (sectionCount - 1) * sectionVertexCount))
                .ToArray();
            return new Polyhedron(points, totalIndices);
        }

        public static Polyhedron CreateSphere(float radius, int sectionCount, int sideVertexCount)
        {
            var points = new Vector3[sideVertexCount * sectionCount + 2];
            List<int> indices = new();

            points[0] = Vector3.UnitY * radius;
            points[1] = -Vector3.UnitY * radius;
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
            return new Polyhedron(points, indices.Select(i => i + 2).ToArray());
        }
    }
}
