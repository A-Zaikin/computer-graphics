using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public static class PlatonicSolids
    {
        public static Polyhedron CreateTetrahedron(float scale)
        {
            var points1 = VectorHelper.MirrorOrthogonally(
                new Vector3(1, 0, 0),
                new Vector3(0, 0, -1 / MathF.Sqrt(2)), scale);
            var points2 = VectorHelper.MirrorOrthogonally(
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 1 / MathF.Sqrt(2)), scale);
            var points = points1.Concat(points2).ToArray();

            List<int> indices = new();
            foreach (var triangle in Combinations.Create(points.Select((p, i) => i).ToArray(), 3))
            {
                PolygonHelper.SortIndices(points, triangle);
                indices.AddRange(triangle);
            }

            return new Polyhedron(points, indices.ToArray());
        }

        public static Polyhedron CreateHexahedron(float scale)
        {
            return Parallelepiped.Create(scale, scale, scale);
        }

        public static Polyhedron CreateOctahedron(float scale)
        {
            var points1 = VectorHelper.MirrorOrthogonally(new Vector3(1, 0, 0), Vector3.Zero, scale);
            var points2 = VectorHelper.MirrorOrthogonally(new Vector3(0, 1, 0), Vector3.Zero, scale);
            var points3 = VectorHelper.MirrorOrthogonally(new Vector3(0, 0, 1), Vector3.Zero, scale);
            var points = points1.Concat(points2).Concat(points3).ToArray();

            List<int> indices = new();
            foreach (var triangle in Combinations.Create(points.Select((p, i) => i).ToArray(), 3))
            {
                if (Vector3.Distance(points[triangle[0]], points[triangle[1]]) > MathF.Sqrt(2) * scale * 1.001f
                    || Vector3.Distance(points[triangle[0]], points[triangle[2]]) > MathF.Sqrt(2) * scale * 1.001f
                    || Vector3.Distance(points[triangle[1]], points[triangle[2]]) > MathF.Sqrt(2) * scale * 1.001f)
                {
                    continue;
                }
                PolygonHelper.SortIndices(points, triangle);
                indices.AddRange(triangle);
            }

            return new Polyhedron(points, indices.ToArray());
        }

        public static Polyhedron CreateIcosahedron(float scale)
        {
            var f = (1 + MathF.Sqrt(5)) / 2; // golden ratio
            var points1 = VectorHelper.MirrorOrthogonally(new Vector3(0, 1, f), Vector3.Zero, scale);
            var points2 = VectorHelper.MirrorOrthogonally(new Vector3(1, f, 0), Vector3.Zero, scale);
            var points3 = VectorHelper.MirrorOrthogonally(new Vector3(f, 0, 1), Vector3.Zero, scale);
            var points = points1.Concat(points2).Concat(points3).ToArray();

            List<int> indices = new();
            foreach (var triangle in Combinations.Create(points.Select((p, i) => i).ToArray(), 3))
            {
                if (Vector3.Distance(points[triangle[0]], points[triangle[1]]) > 2 * scale * 1.001f
                    || Vector3.Distance(points[triangle[0]], points[triangle[2]]) > 2 * scale * 1.001f
                    || Vector3.Distance(points[triangle[1]], points[triangle[2]]) > 2 * scale * 1.001f)
                {
                    continue;
                }
                PolygonHelper.SortIndices(points, triangle);
                indices.AddRange(triangle);
            }

            return new Polyhedron(points, indices.ToArray());
        }

        public static Polyhedron CreateDodecahedron(float scale)
        {
            var f = (1 + MathF.Sqrt(5)) / 2; // golden ratio

            var points1 = VectorHelper.MirrorOrthogonally(new Vector3(1, 1, 1), Vector3.Zero, scale);
            var points2 = VectorHelper.MirrorOrthogonally(new Vector3(0, f, 1 / f), Vector3.Zero, scale);
            var points3 = VectorHelper.MirrorOrthogonally(new Vector3(1 / f, 0, f), Vector3.Zero, scale);
            var points4 = VectorHelper.MirrorOrthogonally(new Vector3(f, 1 / f, 0), Vector3.Zero, scale);
            var points = points1.Concat(points2).Concat(points3).Concat(points4).ToArray();

            List<int> indices = new();
            foreach (var pentagon in Combinations.Create(points.Select((p, i) => i).ToArray(), 5))
            {
                var isSide = true;
                for (var i = 0; i < 4; i++)
                {
                    for (var j = i + 1; j < 5; j++)
                    {
                        if (Vector3.Distance(points[pentagon[i]], points[pentagon[j]]) > 2 * scale * 1.001f)
                        {
                            isSide = false;
                        }
                    }
                }
                if (!isSide)
                {
                    continue;
                }

                PolygonHelper.SortIndices(points, pentagon);
                indices.AddRange(PolygonHelper.Triangulate(5).Select(i => pentagon[i]));
            }

            return new Polyhedron(points, indices.ToArray());
        }
    }
}
