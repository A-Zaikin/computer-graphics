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
        public static List<Polyhedron[]> Polyhedrons = new();
        public static LoopIndex CurrentIndex;
        private static Stopwatch timer = new();
        private static Window window;
        private static Random random = new();

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
            AddPolyhedron(Prismatoid.CreateParallelepiped(1, 2, 3));
            // паралеллепипед (косой)
            AddPolyhedron(Prismatoid.CreateParallelepiped(1, 2, 3, new Vector2(0.5f, -0.8f)));
            // куб
            AddPolyhedron(Prismatoid.CreateParallelepiped(2, 2, 2));
            // пирамида
            AddPolyhedron(Prismatoid.CreatePyramid(PolygonHelper.CreateRegular(Vector2.Zero, 2, 4), 3));
            // усечённая пирамида
            AddPolyhedron(Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 3),
                PolygonHelper.CreateRegular(Vector2.Zero, 2, 3), 2));
            // пирамида, основанием у которой служит правильный многоугольник
            AddPolyhedron(Prismatoid.CreatePyramid(PolygonHelper.CreateRegular(Vector2.Zero, 2, 7), 3));
            // усечённая пирамида из правильных многоугольников
            AddPolyhedron(Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 1, 5),
                PolygonHelper.CreateRegular(Vector2.Zero, 2, 9), 2));

            // конус
            AddPolyhedron(ComplexShape.CreateCylinder(100, 0, 2f, 4, round: true));
            // цилиндр (правильный)
            AddPolyhedron(ComplexShape.CreateCylinder(50, 1.5f, 1.5f, 4, round: true));
            // цилиндр (общий)
            AddPolyhedron(ComplexShape.CreateCylinder(100, 0.5f, 2f, 5, round: true));
        }

        private static void CreateSurfacesOfRotation()
        {
            // гайка
            AddPolyhedron(RotationSurfaces.CreateTorus(2, 0.7f, 4, 3));
            // шайба
            AddPolyhedron(RotationSurfaces.CreateTorus(2, 0.7f, 10, 4, MathHelper.Pi / 4));
            // тор
            AddPolyhedron(RotationSurfaces.CreateTorus(2, 1, 100, 50, round: true));

            // угловая спираль
            AddPolyhedron(ComplexShape.CreateHelix(1.5f, 0.2f, 5, 3, 7, 10));
            // спираль
            AddPolyhedron(ComplexShape.CreateHelix(1.5f, 0.5f, 50, 35, 5, 3, round: true));

            // сферы
            AddPolyhedron(RotationSurfaces.CreateSphere(2, 25, 15));
            AddPolyhedron(RotationSurfaces.CreateSphereFromTriangles(2));
            AddPolyhedron(RotationSurfaces.CreateSphere(2, 100, 100));
        }

        private static void CreatePlatonicSolids()
        {
            AddPolyhedron(PlatonicSolid.CreateTetrahedron(2));
            AddPolyhedron(PlatonicSolid.CreateHexahedron(2.5f));
            AddPolyhedron(PlatonicSolid.CreateOctahedron(2));
            AddPolyhedron(PlatonicSolid.CreateDodecahedron(1));
            AddPolyhedron(PlatonicSolid.CreateIcosahedron(1));
        }

        private static void AddPolyhedron(params Polyhedron[] shape)
        {
            Polyhedrons.Add(shape);
            var randomColor = new Vector3(
                (float)random.NextDouble(),
                (float)random.NextDouble(),
                (float)random.NextDouble());
            foreach(var poly in shape)
            {
                poly.Color = randomColor.Normalized();
            }
        }
    }
}
