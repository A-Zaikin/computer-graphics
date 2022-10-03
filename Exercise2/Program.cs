using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise2
{
    public static class Program
    {
        public static List<Polygon> Polygons;

        private static Task update;

        private static void Main()
        {
            Polygons = new() {
                new RegularPolygon(new Vector2(0.5f, -0.5f), 0.3f, 3, -MathF.PI / 6),
                new RegularPolygon(new Vector2(-0.5f, -0.5f), 0.3f, 4, MathF.PI / 4),
                new RegularPolygon(new Vector2(0.5f, 0.5f), 0.3f, 50),
                new RegularPolygon(new Vector2(-0.5f, 0.5f), 0.3f, 6)
            };

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(600, 600),
                Title = "Exercise 2",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            //update = Task.Run(Update);

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }

        private static void Update()
        {
            while (true)
            {
                Polygons[0].Move(new Vector2(0.0001f, -0.0001f));
                Thread.Sleep(16);
            }
        }
    }
}
