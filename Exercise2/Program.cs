using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace Exercise2
{
    public static class Program
    {
        public static List<Polygon> Polygons;

        private static void Main()
        {
            Polygons = new() {
                new RegularPolygon(new Vector2(0.5f, -0.5f), 0.3f, 3, -MathF.PI / 6, color: new Vector3(0.7f, 1, 0.1f)),
                new RegularPolygon(new Vector2(-0.5f, -0.5f), 0.3f, 4, MathF.PI / 4, color: new Vector3(0, 0.2f, 0.9f)),
                new RegularPolygon(new Vector2(0.5f, 0.5f), 0.3f, 50, color: new Vector3(1, 0.3f, 0.2f)),
                new RegularPolygon(new Vector2(-0.5f, 0.5f), 0.3f, 6, color: new Vector3(0.1f, 1, 1))
            };

            //Polygons[0].Scale = Matrix4.CreateScale(0.5f);

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(600, 600),
                Title = "Exercise 2",
                Flags = ContextFlags.ForwardCompatible,
            };

            using var window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}
