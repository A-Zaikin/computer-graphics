using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public class Polyhedron
    {
        public readonly Vector3[] Points;
        public readonly float[] Vertices;
        public readonly int[] Indices;

        public int VertexArrayObject;

        public Vector3 Color = new(0.2f, 0.5f, 0.1f);

        public Vector3 Translation;
        public Vector3 Rotation;
        public Vector3 Scale = new(1, 1, 1);

        public event Action Animations;

        public Polyhedron(Vector3[] points, int[] indices)
        {
            Points = points;
            Vertices = Points.SelectMany(point => new float[3] { point.X, point.Y, point.Z }).ToArray();
            Indices = indices;
        }

        public Polyhedron(Part[] parts)
        {
            var points = new List<Vector3>();
            var indices = new List<int>();
            var offset = 0;
            for (var i = 0; i < parts.Length; i++)
            {
                points.AddRange(parts[i].GetPoints(out var count));
                indices.AddRange(parts[i].GetIndices().Select(index => index + offset));
                offset += count;
            }

            Points = points.ToArray();
            Indices = indices.ToArray();
            Vertices = Points.SelectMany(point => new float[3] { point.X, point.Y, point.Z }).ToArray();
        }

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.CreateScale(Scale);

            transform *= Matrix4.CreateRotationX(Rotation.X);
            transform *= Matrix4.CreateRotationY(Rotation.Y);
            transform *= Matrix4.CreateRotationZ(Rotation.Z);

            transform *= Matrix4.CreateTranslation(Translation);

            return transform;
        }

        public void Update()
        {
            Animations?.Invoke();
        }
    }
}
