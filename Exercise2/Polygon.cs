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

        public Polygon(params float[] vertices)
        {
            VertexCount = vertices.Length / 3;
            Points = new Vector2[VertexCount];

            for(var i = 0; i < VertexCount; i++)
            {
                Points[i] = new Vector2(vertices[i*3], vertices[i*3 + 1]);
            }
        }

        public float[] GetVertices()
        {
            return Points.SelectMany(point => new float[3] { point.X, point.Y, 0f }).ToArray();
        }

        public void Rotate(float angle, Vector2 point)
        {
            var center = GetCenter();
            for (var i = 0; i < Points.Length; i++)
            {
                var distanceToCenter = center - Points[i];
                Points[i].X += distanceToCenter.Length * MathF.Cos(angle);
                Points[i].Y += distanceToCenter.Length * MathF.Sin(angle);
            }
        }

        public void Move(Vector2 vector)
        {
            Points = Points.Select(point => point + vector).ToArray();
        }

        public virtual Vector2 GetCenter()
        {
            var center = Points.Aggregate((current, point) => current += point);
            return center / Points.Length;
        }
    }
}
