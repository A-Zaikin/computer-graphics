using System;
using System.Drawing;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Sphere
    {
        public float Radius;
        public Vector3 Position;
        public Color Color;

        public Sphere(float radius, Vector3 position, Color color)
        {
            Radius = radius;
            Position = position;
            Color = color;
        }

        // https://www.scratchapixel.com/images/upload/ray-simple-shapes/raysphereisect1.png
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
        public bool IsHit(Ray ray, out Vector3[] hitPoints)
        {
            hitPoints = new Vector3[2];

            var L = Position - ray.Origin;
            var tca = Vector3.Dot(L, ray.Direction);
            var lLength = L.Length();
            var dSquared = lLength * lLength - tca * tca;
            if (dSquared > Radius * Radius)
            {
                return false;
            }
            var thc = MathF.Sqrt(Radius * Radius - dSquared);
            var t0 = tca - thc;
            var t1 = tca + thc;

            hitPoints[0] = ray.Origin + ray.Direction * t0;
            hitPoints[1] = ray.Origin + ray.Direction * t1;
            return true;
        }

    }
}
