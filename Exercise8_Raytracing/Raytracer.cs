using System.Drawing;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Raytracer
    {
        public Vector3 SpherePosition = new();

        public Bitmap RenderFrame()
        {
            var screenSize = new Vector2Int(512, 512);
            var frame = new Bitmap(screenSize.X, screenSize.Y);

            //time++;
            //var spherePosition = new Vector3(
            //    MathF.Sin(time / 2.0f) * 2,
            //    MathF.Cos(time / 2.0f) * 2,
            //    0);
            var sphere = new Sphere(1, SpherePosition, Color.Red);
            var camera = new Camera(
                new Vector3(0, 0, -5),
                Vectors3.Forward,
                screenSize,
                90);
            var rays = camera.CastRays();
            for (var y = 0; y < screenSize.Y; y++)
            {
                for (var x = 0; x < screenSize.X; x++)
                {
                    if (sphere.IsHit(rays[x, y], out var hitPoints))
                    {
                        frame.SetPixel(x, y, sphere.Color);
                    }
                    else
                    {
                        frame.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return frame;
        }
    }
}
