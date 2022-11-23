using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public class Polyhedron
    {
        public Vector3[] Points;
        public float[] Vertices;
        public int[] Indices;

        public int VertexArrayObject;

        public Vector3 Color = new(0.2f, 0.5f, 0.1f);

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = new(1, 1, 1);

        public event Action Animations;

        public Polyhedron(Vector3[] points, int[] indices)
        {
            Points = points;
            Vertices = Points.ToVertices();
            Indices = indices;
        }

        public Matrix4 GetTransform()
        {
            var transform = Matrix4.CreateScale(Scale);

            transform *= Matrix4.CreateRotationX(Rotation.X);
            transform *= Matrix4.CreateRotationY(Rotation.Y);
            transform *= Matrix4.CreateRotationZ(Rotation.Z);

            transform *= Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z));

            return transform;
        }

        public void Update()
        {
            Animations?.Invoke();
        }
    }
}
