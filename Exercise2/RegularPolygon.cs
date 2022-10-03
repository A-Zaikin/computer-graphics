using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise2
{
    public class RegularPolygon : Polygon
    {
        public float Radius;

        private Vector2 center;

        public RegularPolygon(Vector2 center, float radius, int vertexCount, float angleOffset = 0)
        {
            VertexCount = vertexCount;
            this.center = center;
            Radius = radius;

            Points = new Vector2[VertexCount];
            for (var i = 0; i < VertexCount; i++)
            {
                Points[i] = new Vector2(
                    center.X + Radius * MathF.Cos(2 * MathF.PI * i / VertexCount + angleOffset),
                    center.Y + Radius * MathF.Sin(2 * MathF.PI * i / VertexCount + angleOffset));
            }
        }

        public override Vector2 GetCenter() => center;
    }
}
