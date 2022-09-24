using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Raytracer
    {
        public List<Sphere> Spheres;
        public Camera Camera;
        public Light Light;

        public Raytracer()
        {
            Spheres = new()
            {
                new Sphere(1, new Vector3(), Color.Red),
                new Sphere(0.7f, new Vector3(), Color.Yellow),
                new Sphere(99998, new Vector3(0, -100000, 0), Color.ForestGreen)
            };

            Camera = new Camera(
                new Vector3(0, 2, -7),
                Vectors3.Forward,
                new Size(512, 512),
                90);

            Light = new Light(new Vector3(5, 2, 1), 2);
            Spheres.Add(Light.Sphere);
        }

        public Bitmap RenderFrame()
        {
            var frame = new Bitmap(Camera.ScreenSize.Width, Camera.ScreenSize.Height);

            var rays = Camera.CastRays();
            for (var y = 0; y < Camera.ScreenSize.Height; y++)
            {
                for (var x = 0; x < Camera.ScreenSize.Width; x++)
                {
                    var bitmapY = Camera.ScreenSize.Height - y - 1;
                    if (IsAnyObjectHit(rays[x, y], out var hit))
                    {
                        if (hit.Object.IsLight)
                        {
                            frame.SetPixel(x, bitmapY, hit.Object.Color);
                        }
                        else
                        {
                            var color = ApplyBrightness(hit.Object.Color, GetBrightness(hit));
                            frame.SetPixel(x, bitmapY, color);
                        }
                    }
                    else
                    {
                        frame.SetPixel(x, bitmapY, Color.Blue);
                    }
                }
            }

            return frame;
        }

        private Color ApplyBrightness(Color color, float brightness)
        {
            var ratio = 0.1f + brightness;
            return Color.FromArgb(255,
                (int)Math.Clamp(color.R * ratio, 0, 255),
                (int)Math.Clamp(color.G * ratio, 0, 255),
                (int)Math.Clamp(color.B * ratio, 0, 255));
        }

        private bool IsAnyObjectHit(Ray ray, out Hit hit)
        {
            var hits = new List<Hit>();
            var didRayHitAnything = false;
            foreach (var sphere in Spheres)
            {
                if (sphere.IsHit(ray, hits))
                {
                    didRayHitAnything = true;
                }
            }

            if (didRayHitAnything)
            {
                hit = FindClosestHit(hits);
                return true;
            }
            hit = new Hit();
            return false;
        }

        private Hit FindClosestHit(List<Hit> hits)
        {
            var closestHit = hits[0];
            for (var i = 1; i < hits.Count; i++)
            {
                if (hits[i].LocationOnRay < closestHit.LocationOnRay)
                {
                    closestHit = hits[i];
                }
            }
            return closestHit;
        }

        private float GetBrightness(Hit hit)
        {
            var hitPoint = hit.GetHitPoint();
            var vectorToLight = Light.Position - hitPoint;
            var distanceToLight = vectorToLight.Length();
            var rayToLight = new Ray(hitPoint, Vector3.Normalize(vectorToLight));
            if (IsAnyObjectHit(rayToLight, out var newHit) 
                && newHit.LocationOnRay < distanceToLight
                && !newHit.Object.IsLight)
            {
                return 0;
            }

            var brightness = Light.Brightness;
            if (distanceToLight > 1)
            {
                brightness /= distanceToLight;
            }
            return brightness;
        }
    }

    public struct Hit
    {
        public float LocationOnRay;
        public Ray Ray;
        public Sphere Object;

        public Hit(float locationOnRay, Ray ray, Sphere hitObject)
        {
            LocationOnRay = locationOnRay;
            Ray = ray;
            Object = hitObject;
        }

        public Vector3 GetHitPoint() => Ray.Origin + Ray.Direction * LocationOnRay;
    }
}
