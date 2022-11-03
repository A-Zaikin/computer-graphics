using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exercise6
{
    public static class Program
    {
        public static List<Polyhedron> Polyhedrons;
        private static Stopwatch timer = new();
        private static Window window;

        public static float Time => timer.ElapsedMilliseconds / 1000f;

        private static void Main()
        {
            CreatePolygons();

            timer.Start();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(500, 500),
                Title = "Exercise 6",
                Flags = ContextFlags.ForwardCompatible,
            };

            window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
            window.Dispose();
        }

        private static void CreatePolygons()
        {
            Polyhedrons = new();

            var cylinder = new Prismatoid(
                Polygon.CreateRegular(Vector2.Zero, 1, 3),
                Polygon.CreateRegular(Vector2.Zero, 1, 5));
            cylinder.Color = new Vector3(0, 1, 0);
            cylinder.Animations += () => cylinder.Rotation = new Vector3(Time / 1.7f, Time / 1.9f, Time / 2.1f);
            Polyhedrons.Add(cylinder);
        }
    }
}
