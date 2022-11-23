using OpenTK.Mathematics;
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
            CreatePolyhedrons();
            CurrentIndex = new LoopIndex(Polyhedrons);

            timer.Start();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 800),
                Title = "Exercise 6",
                Flags = ContextFlags.ForwardCompatible,
            };
            var gameWindowSetting = new GameWindowSettings()
            {
                RenderFrequency = 60,
                UpdateFrequency = 60,
            };
            window = new Window(gameWindowSetting, nativeWindowSettings);
            window.Run();
            window.Dispose();
        }

        private static void CreatePolyhedrons()
        {
            CreatePrismatoids();
            CreateSurfacesOfRotation();
            CreatePlatonicSolids();
        }

        private static void CreatePrismatoids()
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
                PolygonHelper.CreateRegular(Vector2.Zero, 0.5f, 30),
                PolygonHelper.CreateRegular(Vector2.Zero, 2, 50), 4));
        }

        private static void CreateSurfacesOfRotation()
        {
            // гайка
            Polyhedrons.Add(RotationSurfaces.CreateTorus(2, 0.7f, 4, 3));
            // шайба
            Polyhedrons.Add(RotationSurfaces.CreateTorus(2, 0.7f, 100, 4, MathHelper.Pi / 4));
            // тор
            Polyhedrons.Add(RotationSurfaces.CreateTorus(2, 1, 100, 50));

            // угловая спираль
            Polyhedrons.Add(RotationSurfaces.CreateHelix(1.5f, 0.2f, 5, 3, 7, 10));
            // спираль
            Polyhedrons.Add(RotationSurfaces.CreateHelix(1.5f, 0.5f, 50, 50, 5, 3));

            // сферы
            Polyhedrons.Add(RotationSurfaces.CreateSphere(2, 4, 2));
            Polyhedrons.Add(RotationSurfaces.CreateSphere(2, 7, 3));
            Polyhedrons.Add(RotationSurfaces.CreateSphere(2, 10, 10));
            Polyhedrons.Add(RotationSurfaces.CreateSphere(2, 100, 100));
        }

        private static void CreatePlatonicSolids()
        {
            Polyhedrons.Add(PlatonicSolids.CreateTetrahedron(2));
            Polyhedrons.Add(PlatonicSolids.CreateHexahedron(2.5f));
            Polyhedrons.Add(PlatonicSolids.CreateOctahedron(2));
            Polyhedrons.Add(PlatonicSolids.CreateDodecahedron(1));
            Polyhedrons.Add(PlatonicSolids.CreateIcosahedron(1));
        }
    }
}
