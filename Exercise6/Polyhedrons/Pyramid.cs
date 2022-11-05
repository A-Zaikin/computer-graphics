using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public static class Pyramid
    {
        public static Polyhedron Create(Vector2[] surface, float height, Vector2 apex = default)
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
    }
}
