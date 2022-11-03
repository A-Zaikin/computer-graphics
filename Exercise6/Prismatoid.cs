using OpenTK.Mathematics;
using System.Linq;

namespace Exercise6
{
    public class Prismatoid : Polyhedron
    {
        public Prismatoid(Vector2[] surface1, Vector2[] surface2)
            : base(CreateShape(surface1, surface2)) { }

        private static Part[] CreateShape(Vector2[] surface1, Vector2[] surface2)
        {
            var plane1Points = surface1.Select(p => new Vector3(p.X, 1, p.Y)).ToArray();
            var plane1 = new SimplePolygon(plane1Points);
            var plane2Points = surface2.Select(p => new Vector3(p.X, -1, p.Y)).ToArray();
            var plane2 = new SimplePolygon(plane2Points);
            var side = new Side(plane1, plane2);
            var parts = new Part[] { plane1, plane2, side };

            var planeConnections = new bool[surface1.Length, surface2.Length];
            for (var i = 0; i < surface1.Length; i++)
            {
                for (var j = 0; j < surface2.Length; j++)
                {
                    if (IsVectorPointingInsidePolygon(surface1, i, surface2[j])
                        && IsVectorPointingInsidePolygon(surface2, j, surface1[i]))
                    {
                        continue;
                    }
                    planeConnections[i, j] = true;
                }
            }

            var planeConnectionsCopy = planeConnections.Clone() as bool[,];

            // resolve quads
            for (LoopIndex i = new(surface1); !i.HasLooped; i += 1)
            {
                for (LoopIndex j = new(surface2); !j.HasLooped; j += 1)
                {
                    if (planeConnectionsCopy[i, j]
                        && planeConnectionsCopy[i + 1, j]
                        && planeConnectionsCopy[i, j + 1]
                        && planeConnectionsCopy[i + 1, j + 1])
                    {
                        var line1 = surface2[j + 1] - surface1[i];
                        var distance1 = VectorHelper.Distance(surface1[i], line1, surface1[i + 1]);
                        var line2 = surface2[j] - surface1[i + 1];
                        var distance2 = VectorHelper.Distance(surface1[i + 1], line2, surface1[i]);
                        if (distance1 > distance2)
                        {
                            planeConnections[i + 1, j] = false;
                        }
                        else
                        {
                            planeConnections[i, j + 1] = false;
                        }
                    }
                }
            }

            for (LoopIndex i = new(plane1.Points); !i.HasLooped; i += 1)
            {
                for (LoopIndex j = new(plane2.Points); !j.HasLooped; j += 1)
                {
                    if (planeConnections[i, j] && planeConnections[i, j + 1])
                    {
                        side.AddTriangleIndices(i, j, j + 1);
                    }
                }
            }

            for (LoopIndex j = new(plane2.Points); !j.HasLooped; j += 1)
            {
                for (LoopIndex i = new(plane1.Points); !i.HasLooped; i += 1)
                {
                    if (planeConnections[i, j] && planeConnections[i + 1, j])
                    {
                        side.AddTriangleIndices(j, i + 1, i);
                    }
                }
            }

            return parts;


            static bool IsVectorPointingInsidePolygon(Vector2[] surface, int currentPointIndex, Vector2 otherPolygonPoint)
            {
                var previousPoint = surface[currentPointIndex == 0
                    ? surface.Length - 1
                    : currentPointIndex - 1];
                var nextPoint = surface[(currentPointIndex + 1) % surface.Length];

                var currentPoint = surface[currentPointIndex];
                var polygonAngle = VectorHelper.GetCcwAngle(previousPoint - currentPoint, nextPoint - currentPoint);
                var otherSurfaceAngle = VectorHelper.GetCcwAngle(previousPoint - currentPoint,
                    otherPolygonPoint - currentPoint);
                return otherSurfaceAngle > 0 && otherSurfaceAngle < polygonAngle;
            }
        }
    }
}
