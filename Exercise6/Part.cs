using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise6
{
    public abstract class Part
    {
        public abstract IEnumerable<Vector3> GetPoints(out int count);
        public abstract IEnumerable<int> GetIndices();
    }

    public class Side : Part
    {
        public SimplePolygon Plane1;
        public SimplePolygon Plane2;

        private readonly List<int> indices = new();

        public Side(SimplePolygon plane1, SimplePolygon plane2)
        {
            Plane1 = plane1;
            Plane2 = plane2;
        }

        public void AddTriangleIndices(params LoopIndex[] triangleIndices)
        {
            foreach (var index in triangleIndices)
            {
                if (index.Array == Plane1.Points)
                {
                    indices.Add(index);
                }
                else if (index.Array == Plane2.Points)
                {
                    indices.Add(index.Value + Plane1.Points.Length);
                }
                else
                {
                    throw new ArgumentException("LoopIndex refers to a different side array.");
                }
            }
        }

        public override IEnumerable<Vector3> GetPoints(out int count)
        {
            count = 0;
            return Array.Empty<Vector3>();
        }

        public override IEnumerable<int> GetIndices()
        {
            return indices;
        }
    }

    public class SimplePolygon : Part
    {
        public readonly Vector3[] Points;
        private readonly List<int> indices = new();

        public SimplePolygon(Vector3[] points)
        {
            Points = points;
            for (LoopIndex i = new(Points, 1); i < Points.Length - 1; i += 1)
            {
                indices.AddRange(new int[] { 0, i, i + 1 });
            }
        }

        public override IEnumerable<Vector3> GetPoints(out int count)
        {
            count = Points.Length;
            return Points;
        }

        public override IEnumerable<int> GetIndices()
        {
            return indices;
        }
    }
}
