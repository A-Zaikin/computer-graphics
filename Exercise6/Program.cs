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
            //var apex = new Vector3(0, 2, 0);
            //var p1 = new Vector3(0, 0, 0.5f);
            //var p2 = new Vector3(0.5f, 0, -0.5f);
            //var p3 = new Vector3(-0.5f, 0, -0.5f);
            //var prism = new Polyhedron(
            //    new Vector3[] {
            //        apex, p1, p2, p3,
            //    }, 
            //    new int[] {
            //        0, 1, 2,
            //        0, 2, 3,
            //        0, 3, 1,
            //        1, 3, 2
            //    },
            //    new Vector3(1f, 0f, 0f));
            //prism.Scale *= Matrix4.CreateScale(3);
            //prism.AddAnimation(AnimationType.Rotation,
            //    () => Matrix4.CreateRotationX(Time)
            //    * Matrix4.CreateRotationY(Time / 2.1f)
            //    * Matrix4.CreateRotationZ(Time / 3.1f));
            //Polyhedrons.Add(prism);

            //var square1 = new Shape(new Vector3[] {
            //    new Vector3(-1, -1, 0),
            //    new Vector3(-1, 1, 0),
            //    new Vector3(1, 1, 0),
            //    new Vector3(1, -1, 0),
            //}, new Vector3(0.8f, 0.1f, 0.4f));
            //square1.Translation *= Matrix4.CreateTranslation(Vector3.UnitX * -2 + Vector3.UnitZ * 10);
            //Shapes.Add(square1);

            //var square2 = new Shape(new Vector3[] {
            //    new Vector3(-1, -1, 0),
            //    new Vector3(-1, 1, 0),
            //    new Vector3(1, 1, 0),
            //    new Vector3(1, -1, 0),
            //}, new Vector3(0.1f, 0.8f, 0.4f));
            //square2.Translation *= Matrix4.CreateTranslation(Vector3.UnitX * 2 + Vector3.UnitZ * 200);
            //Shapes.Add(square2);

            var frustum = new Prismatoid(
                new Vector2[]
                {
                    //new Vector2(0, 0.7f),
                    //new Vector2(0.5f, -0.3f),
                    //new Vector2(-0.5f, -0.5f),
                    new Vector2(0, 1f),
                    new Vector2(0.5f, 0.3f),
                    new Vector2(0.3f, -0.5f),
                    new Vector2(-0.3f, -0.5f),
                    new Vector2(-0.5f, 0.3f),
                },
                new Vector2[]
                {
                    new Vector2(-1, -1),
                    new Vector2(-1, 1),
                    new Vector2(1, 1),
                    new Vector2(1, -1),
                },
                new Vector3(0, 0, 1));
            frustum.AddAnimation(AnimationType.Rotation,
                () => Matrix4.CreateRotationX(Time / 2.7f)
                * Matrix4.CreateRotationY(Time / 2.1f)
                * Matrix4.CreateRotationZ(Time / 3.1f));
            Polyhedrons.Add(frustum);
        }
    }
}
