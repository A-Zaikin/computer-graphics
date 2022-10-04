using OpenTK.Mathematics;
using System;
using System.Linq;

namespace Exercise2
{
    public class Polygon
    {
        public int VertexCount;
        public Vector2[] Points;
        public int VertexArrayObject;
        public Vector3 Color;
        //public Matrix4 Scale;

        public Polygon(Vector2[] points, Vector3 color = default)
        {
            VertexCount = points.Length;
            Points = points;
            Color = color == default 
                ? new Vector3(0.2f, 0.5f, 0.1f) 
                : color;
            //Scale = Matrix4.Identity;
        }

        public float[] GetVertices()
        {
            return Points.SelectMany(point => new float[3] { point.X, point.Y, 0f }).ToArray();
        }

        public virtual Vector2 GetCenter()
        {
            var center = Points.Aggregate((current, point) => current += point);
            return center / Points.Length;
        }
    }
}
