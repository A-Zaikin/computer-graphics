using OpenTK.Mathematics;
using System;

namespace Exercise6
{
    public class Polyhedron
    {
        public Vector3[] Points;
        public int[] Indices;
        public Vector3[] Normals;
        public float[] BufferData;

        public bool IsRound;

        public int VertexArrayObject;

        public Vector3 Color = new(0.2f, 0.5f, 0.1f);

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = new(1, 1, 1);

        public event Action Animations;

        public Polyhedron(Vector3[] points, int[] indices, Vector3[] normals = null, bool round = false)
        {
            Points = points;
            Indices = indices;
            Normals = normals;
            IsRound = round;
            if (Normals == null)
            {
                Normals = new Vector3[Points.Length];
                for (var i = 0; i < Normals.Length; i++)
                    Normals[i] = Vector3.UnitZ;
            }

            var bufferDataVectors = new Vector3[Points.Length * 2];
            for (var i = 0; i < bufferDataVectors.Length; i += 2)
            {
                bufferDataVectors[i] = Points[i / 2];
                bufferDataVectors[i + 1] = Normals[i / 2];
            }
            BufferData = bufferDataVectors.ToVertices();
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
