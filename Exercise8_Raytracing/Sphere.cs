using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Sphere
    {
        public float Radius;
        public Vector3 Position;
        public Color Color;
        public bool IsLight;

        public Sphere(float radius, Vector3 position, Color color)
        {
            Radius = radius;
            Position = position;
            Color = color;
        }

        // https://www.scratchapixel.com/images/upload/ray-simple-shapes/raysphereisect1.png
        // https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
        public bool IsHit(Ray ray, List<Hit> hitPoints)
        {
            var L = Position - ray.Origin;
            var tca = Vector3.Dot(L, ray.Direction);
            if (tca < 0)
            {
                return false;
            }
            var lLength = L.Length();
            var dSquared = lLength * lLength - tca * tca;
            if (dSquared > Radius * Radius)
            {
                return false;
            }
            var thc = MathF.Sqrt(Radius * Radius - dSquared);

            hitPoints.Add(new Hit(tca - thc, ray, this));
            hitPoints.Add(new Hit(tca + thc, ray, this));
            return true;
        }
    }
}
