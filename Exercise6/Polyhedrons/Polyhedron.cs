using OpenTK.Mathematics;
using System;
using System.Linq;

namespace Exercise6
{
    public class Polyhedron
    {
        public Vector3[] Points;
        public int[] Indices;
        public Vector3[] Normals;
        public Vector2[] TextureCoordinates;
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

        public Polyhedron(Vector3[] points, int[] indices,
            Vector3[] normals = null, bool round = false, Vector2[] textureCoordinates = null)
        {
            Points = points;
            Indices = indices;
            Normals = normals;

            TextureCoordinates = textureCoordinates;
            //TextureCoordinates ??= Enumerable.Repeat(Vector2.One, Points.Length).ToArray();
            TextureCoordinates ??= new Vector2[Points.Length];

            IsRound = round;
            if (Normals == null)
            {
                Normals = new Vector3[Points.Length];
                for (var i = 0; i < Normals.Length; i++)
                    Normals[i] = Vector3.UnitZ;
            }

            BufferData = new float[Points.Length * 8];
            for (var i = 0; i < Points.Length; i++)
            {
                BufferData[i * 8] = Points[i].X;
                BufferData[i * 8 + 1] = Points[i].Y;
                BufferData[i * 8 + 2] = Points[i].Z;

                BufferData[i * 8 + 3] = Normals[i].X;
                BufferData[i * 8 + 4] = Normals[i].Y;
                BufferData[i * 8 + 5] = Normals[i].Z;

                BufferData[i * 8 + 6] = TextureCoordinates[i].X;
                BufferData[i * 8 + 7] = TextureCoordinates[i].Y;
            }
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
