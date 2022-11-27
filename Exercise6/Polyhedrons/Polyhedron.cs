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

        public Material Material;

        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale = Vector3.One;
        public Vector3 PostScaleRotation;

        public Vector3 WorldRotation;

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
            var transform = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(Rotation));

            transform *= Matrix4.CreateScale(Scale);

            transform *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(PostScaleRotation));

            //var rotatedPosition = Position;
            //rotatedPosition.Yz = VectorHelper.Rotate(Position.Yz, Rotation.X);
            //rotatedPosition.Xz = VectorHelper.Rotate(Position.Xz, Rotation.Y);
            //rotatedPosition.Xy = VectorHelper.Rotate(Position.Xy, Rotation.Z);
            transform *= Matrix4.CreateTranslation(Position);

            return transform;
        }

        public Matrix4 GetWorldRotation()
        {
            return Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(WorldRotation));
        }

        public void Update()
        {
            Animations?.Invoke();
        }
    }
}
