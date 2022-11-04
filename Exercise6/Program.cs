﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exercise6
{
    public static class Program
    {
        public static List<Polyhedron> Polyhedrons = new();
        public static LoopIndex CurrentIndex;
        private static Stopwatch timer = new();
        private static Window window;

        public static float Time => timer.ElapsedMilliseconds / 1000f;

        private static void Main()
        {
            CreatePolygons();
            CurrentIndex = new LoopIndex(Polyhedrons);

            timer.Start();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 800),
                Title = "Exercise 6",
                Flags = ContextFlags.ForwardCompatible,
            };

            window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
            window.Dispose();
        }

        private static void CreatePolygons()
        {
            
            
            // паралеллепипед
            Polyhedrons.Add(Parallelepiped.Create(1, 2, 3));
            // куб
            Polyhedrons.Add(Parallelepiped.Create(2, 2, 2));
            // пирамида
            Polyhedrons.Add(Pyramid.Create(PolygonHelper.CreateRegular(Vector2.Zero, 2, 4), 3));
            // усечённая пирамида
            Polyhedrons.Add(Prismatoid.Create(
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 3),
                PolygonHelper.CreateRegular(Vector2.Zero, 2, 3), 2));
            // пирамида, основанием у которой служит правильный многоугольник
            Polyhedrons.Add(Pyramid.Create(PolygonHelper.CreateRegular(Vector2.Zero, 2, 7), 3));
            // усечённая пирамида из правильных многоугольников
            Polyhedrons.Add(Prismatoid.Create(
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 5),
                PolygonHelper.CreateRegular(Vector2.Zero, 2, 9), 2));
            // конус
            Polyhedrons.Add(Pyramid.Create(PolygonHelper.CreateRegular(Vector2.Zero, 2, 50), 3));
            // цилиндр (правильный)
            Polyhedrons.Add(Prismatoid.Create(
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 50),
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 50), 3));
            // цилиндр (общий)
            Polyhedrons.Add(Prismatoid.Create(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.3f, 30),
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 50), 3));

            //Polyhedrons.Add(RotationSurface.CreateSphere(3, 50, 50));

            // гайка
            Polyhedrons.Add(RotationSurface.CreateTorus(2, 0.7f, 4, 3));
            // шайба
            Polyhedrons.Add(RotationSurface.CreateTorus(2, 0.7f, 100, 4, MathHelper.Pi / 4));
            // тор
            Polyhedrons.Add(RotationSurface.CreateTorus(2, 1, 100, 50));

            // угловая спираль
            Polyhedrons.Add(RotationSurface.CreateHelix(1.5f, 0.2f, 5, 3, 7, 10));
            // спираль
            Polyhedrons.Add(RotationSurface.CreateHelix(1.5f, 0.5f, 50, 50, 5, 3));
        }
    }
}
