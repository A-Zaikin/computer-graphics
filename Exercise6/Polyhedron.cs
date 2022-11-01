using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise6
{
    public class Polyhedron
    {
        public readonly Vector3[] Points;
        public readonly int[] Indices;

        public readonly Vector3 Center;

        public int VertexArrayObject;

        public Vector3 Color;
        public Matrix4 Translation = Matrix4.CreateTranslation(Vector3.Zero);
        public Matrix4 Rotation = Matrix4.Identity;
        public Matrix4 Scale = Matrix4.CreateScale(1);

        public List<Animation> Animations = new();

        public Polyhedron(Vector3[] points, int[] indices, Vector3 color = default)
        {
            Points = points;
            Indices = indices;
            Center = Points.Aggregate((current, point) => current += point) / Points.Length;

            Color = color == default
                ? new Vector3(0.2f, 0.5f, 0.1f)
                : color;
        }

        protected Polyhedron(Shape shape, Vector3 color = default)
            : this(shape.Points, shape.Indices.ToArray(), color) { }

        //public static Matrix4 GetRotationMatrix(float angle, Vector3 point)
        //{
        //    return Matrix4.CreateTranslation(-new Vector3(point))
        //        * Matrix4.CreateRotationZ(angle)
        //        * Matrix4.CreateTranslation(new Vector3(point));
        //}

        public float[] GetVertices()
        {
            return Points.SelectMany(point => new float[3] { point.X, point.Y, point.Z }).ToArray();
        }

        public Matrix4 GetTransform()
        {
            var center3D = new Vector3(Center);
            var transform = Matrix4.CreateTranslation(-center3D);

            transform *= Scale;
            foreach (var animation in Animations.Where(animation => animation.Type == AnimationType.Scale))
            {
                transform *= animation.Transformation();
            }

            transform *= Rotation;
            foreach (var animation in Animations.Where(animation => animation.Type == AnimationType.Rotation))
            {
                transform *= animation.Transformation();
            }

            transform *= Translation;
            foreach (var animation in Animations.Where(animation => animation.Type == AnimationType.Translation))
            {
                transform *= animation.Transformation();
            }

            transform *= Matrix4.CreateTranslation(center3D);
            return transform;
        }

        public void AddAnimation(AnimationType type, Func<Matrix4> tranformation)
        {
            Animations.Add(new Animation { Type = type, Transformation = tranformation });
        }

        public class Animation
        {
            public AnimationType Type;
            public Func<Matrix4> Transformation;
        }

        public class Shape
        {
            public Vector3[] Points;
            public List<int> Indices;

            public Shape(int pointCount)
            {
                Points = new Vector3[pointCount];
                Indices = new();
            }
        }
    }

    public enum AnimationType
    {
        Translation,
        Rotation,
        Scale
    }
}
