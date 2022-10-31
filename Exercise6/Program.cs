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
        public static List<Shape> Shapes;
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
            Shapes = new();
            var square = new Shape(new Vector3[] {
                new Vector3(0, 50, 0),
                new Vector3(30, 0, 10),
                new Vector3(10, 0, 30),
                new Vector3(-15, 0, -15),
                new Vector3(30, 0, 10),
            }, new Vector3(0.8f, 0.1f, 0.4f));
            square.Scale *= Matrix4.CreateScale(6);
            square.Animations.Add(new Animation(AnimationType.Rotation,
                () => Matrix4.CreateRotationX(Time) * Matrix4.CreateRotationY(Time / 2.1f)));
            Shapes.Add(square);
        }
    }
}
