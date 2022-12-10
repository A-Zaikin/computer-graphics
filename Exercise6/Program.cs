using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            CurrentIndex = new LoopIndex(Polyhedrons, Polyhedrons.Count - 1);

            timer.Start();
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1000, 800),
                Title = "Exercise 6-7  |  WSADQE = rotate  |  ZX = zoom  |  arrows = next figure  |  " +
                    "F = flat lighting  |  "
                    + "R = poly mode  |  SPACE = auto rotation",
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
            AddBigModel();
        }

        private static void AddBigModel()
        {
            var eyeHeight = 1.4f;
            var topRadius = 2f;
            var bottomRadius = 1.5f;
            Material metal = new() { Color = Vector3.One * 0.5f, SpecularStrength = 0.8f };
            Material rubber = new() { Color = Vector3.One * 0.1f, SpecularStrength = 0.05f };

            var body = Prismatoid.CreateGeneral(
                    PolygonHelper.CreateRegular(Vector2.Zero, bottomRadius, 4),
                    PolygonHelper.CreateRegular(Vector2.Zero, topRadius, 4),
                    4);
            body.Rotation.X = MathF.PI;
            body.Rotation.Y = MathF.PI / 4;
            body.Material = new() { Color = new Vector3(1, 0.8f, 0.2f) * 0.9f, SpecularStrength = 0.1f };
            body.TextureIndex = 0;

            float t1 = 0.8f, t2 = 0.6f;
            var bodyStripe = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, bottomRadius + (topRadius - bottomRadius) * t2, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, bottomRadius + (topRadius - bottomRadius) * t1, 4),
                4 * (t1 - t2));
            bodyStripe.Rotation.X = MathF.PI;
            bodyStripe.Rotation.Y = MathF.PI / 4;
            bodyStripe.Position.Y += 0.7f;
            //bodyStripe.Scale *= 0.994f;
            bodyStripe.Material = new() { Color = Vector3.One * 0.7f, SpecularStrength = 0.1f };
            bodyStripe.TextureIndex = 1;

            var frontPanel = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 1f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 1.4f, 4),
                1.2f);
            frontPanel.Rotation.X = MathF.PI;
            frontPanel.Rotation.Y = MathF.PI / 4;
            frontPanel.Position.Y = 0.6f;
            frontPanel.Position.Z = 0.9f;
            frontPanel.Scale.X = 0.4f;
            frontPanel.Scale.Z = 0.5f;
            //frontPanel.Material = body.Material;
            //frontPanel.Material.Color *= 0.5f;
            frontPanel.Material = new() { Color = new Vector3(1f, 1f, 1) * 0.3f, SpecularStrength = 0.1f };
            frontPanel.TextureIndex = 1;

            var lowerBody = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 1.5f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 1.7f, 4),
                1);
            lowerBody.Rotation.X = MathF.PI;
            lowerBody.Rotation.Y = MathF.PI / 4;
            lowerBody.Position.Y = -1.6f;
            lowerBody.Scale.X = 0.8f;
            lowerBody.Scale.Z = 1.15f;
            lowerBody.Material = new() { Color = Vector3.One * 0.2f, SpecularStrength = 0.8f };
            lowerBody.TextureIndex = 0;

            var eyeHolder = RotationSurfaces.CreateSphere(0.5f, 20, 20);
            eyeHolder.Position = new Vector3(0, eyeHeight, 1.15f);
            eyeHolder.Material = metal;

            var eye = RotationSurfaces.CreateSphere(0.35f, 20, 20);
            eye.Position = new Vector3(0, eyeHeight, 1.4f);
            eye.Material = new() { Color = new Vector3(0, 0.5f, 0.7f), SpecularStrength = 1 };

            var wheel = RotationSurfaces.CreateTorus(0.5f, 0.4f, 50, 4, MathF.PI / 4, false);
            wheel.Position.Y = -2.4f;
            wheel.Rotation.Z = MathF.PI / 2;
            wheel.Scale.X = 1.7f;
            //wheel.Material = rubber;
            wheel.Material = new() { Color = Vector3.One, SpecularStrength = 0 };
            wheel.TextureIndex = 4;

            var wheelSuspension = RotationSurfaces.CreateTorus(0.6f, 0.2f, 30, 30, round: true);
            wheelSuspension.Position.Y = -1.7f;
            wheelSuspension.Rotation.X = MathF.PI / 2;
            wheelSuspension.Scale.Y = 1.4f;
            wheelSuspension.Material = metal;

            var armVertexCount = 15;

            var leftArm = ComplexShape.CreateCylinder(armVertexCount, 0.1f, 0.15f, 2, true);
            foreach (var poly in leftArm)
            {
                poly.Position.Y = eyeHeight - 0.7f;
                poly.Position.X = -topRadius;
                poly.Rotation.Z = MathF.PI - MathF.PI / 4;

                poly.Material = metal;
            }
            leftArm[1].TextureIndex = 5;

            var leftArmWires = ComplexShape.CreateHelix(0.1f, 0.08f, 20, 20, 2.2f, 4, 0, true);
            foreach (var poly in leftArmWires)
            {
                poly.Position.Y = eyeHeight - 0.7f;
                poly.Position.X = -topRadius;
                poly.Rotation.Z = MathF.PI - MathF.PI / 4;

                poly.Material = new() { Color = new Vector3(1, 1, 0), SpecularStrength = 0.1f };
            }

            var leftElbow = PlatonicSolid.CreateOctahedron(0.3f);
            leftElbow.Position = new Vector3(-topRadius - 0.7f, eyeHeight - 1.45f, 0);
            leftElbow.Material = metal;

            var leftForearm = ComplexShape.CreateCylinder(armVertexCount, 0.07f, 0.1f, 1.5f, true);
            foreach (var poly in leftForearm)
            {
                poly.Position.X = -topRadius - 0.4f;
                poly.Position.Y = eyeHeight - 1.8f;
                poly.Position.Z = 0.6f;

                poly.Rotation.X = -MathF.PI / 3;
                poly.Rotation.Z = -5 * MathF.PI / 6;

                poly.Material = metal;
            }
            leftForearm[1].TextureIndex = 5;

            var rightArm = ComplexShape.CreateCylinder(armVertexCount, 0.1f, 0.15f, 2, true);
            foreach (var poly in rightArm)
            {
                poly.Position.Y = eyeHeight + 0.15f;
                poly.Position.X = topRadius + 0.1f;

                poly.Rotation.Z = -MathF.PI / 2 + MathF.PI / 12;

                poly.Material = metal;
            }
            rightArm[1].TextureIndex = 5;

            var rightForearm = ComplexShape.CreateCylinder(armVertexCount, 0.07f, 0.1f, 1.5f, true);
            foreach (var poly in rightForearm)
            {
                poly.Position.X = topRadius + 1f;
                poly.Position.Y = eyeHeight + 1.1f;
                poly.Position.Z = 0.15f;

                poly.Rotation.X = MathF.PI / 12;

                poly.Material = metal;
            }
            rightForearm[1].TextureIndex = 5;

            var rightForearmWires = ComplexShape.CreateHelix(0.12f, 0.05f, 20, 20, 1.55f, 1, 0, true);
            foreach (var poly in rightForearmWires)
            {
                poly.Position.X = topRadius + 0.95f;
                poly.Position.Y = eyeHeight + 1.2f;
                poly.Position.Z = 0.15f;

                poly.Rotation.X = MathF.PI / 12;

                poly.Material = new() { Color = new Vector3(1, 0, 0), SpecularStrength = 0.1f };
            }

            var rightElbow = PlatonicSolid.CreateOctahedron(0.3f);
            rightElbow.Position = new Vector3(topRadius + 1f, eyeHeight + 0.4f, 0);
            rightElbow.Material = metal;

            var leftCap = Prismatoid.CreateParallelepiped(0.1f, 1, 1);
            leftCap.Position = new Vector3(-1.6f, 2.3f, 0);
            leftCap.Rotation.Z = MathF.PI / 5;
            leftCap.Material = metal;
            leftCap.TextureIndex = 0;

            var rightCap = Prismatoid.CreateParallelepiped(0.1f, 1, 1);
            rightCap.Position = new Vector3(1.6f, 2.3f, 0);
            rightCap.Rotation.Z = -MathF.PI / 5;
            rightCap.Material = metal;
            rightCap.TextureIndex = 0;

            var antenna = ComplexShape.CreateHelix(0.2f, 0.05f, 20, 20, 2, 1, 0, true);
            foreach (var poly in antenna)
            {
                poly.Position.X = 0.5f;
                poly.Position.Y = eyeHeight + 1.5f;
                poly.Position.Z = -0.4f;

                poly.Rotation.Y = MathF.PI;

                poly.Material = metal;
            }

            var leftHandCylinder = ComplexShape.CreateCylinder(20, 0.1f, 0.1f, 0.3f, true);
            foreach (var poly in leftHandCylinder)
            {
                poly.Material = metal;
                poly.Rotation.Z = -MathF.PI / 6;

                poly.Position.X = -2;
                poly.Position.Y = -0.75f;
                poly.Position.Z = 1.2f;
            }
            leftHandCylinder[1].TextureIndex = 5;

            var leftHandTop = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.1f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.12f, 4), 1);
            leftHandTop.Material = metal;
            leftHandTop.Scale.Z = 0.25f;
            leftHandTop.Position.X = -1.6f;
            leftHandTop.Position.Y = -1f;
            leftHandTop.Position.Z = 1.2f;
            leftHandTop.PostScaleRotation.Z = MathF.PI / 3;

            var leftHandBottom = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.1f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.15f, 4), 1);
            leftHandBottom.Material = metal;
            leftHandBottom.Scale.Z = 0.25f;
            leftHandBottom.Position.X = -1.7f;
            leftHandBottom.Position.Y = -0.95f;
            leftHandBottom.Position.Z = 1.5f;
            leftHandBottom.PostScaleRotation = new Vector3(-0.3f, -0.5f, 0.9f);

            var rightHandCylinder = ComplexShape.CreateCylinder(20, 0.1f, 0.1f, 0.3f, true);
            foreach (var poly in rightHandCylinder)
            {
                poly.Material = metal;
                poly.Rotation.Z = MathF.PI / 2;

                poly.Position.X = 3;
                poly.Position.Y = 3.3f;
                poly.Position.Z = 0.35f;
            }
            rightHandCylinder[1].TextureIndex = 5;

            var rightHandTop = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.1f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.12f, 4), 1);
            rightHandTop.Material = metal;
            rightHandTop.Scale.Z = 0.25f;
            rightHandTop.Position.X = 3;
            rightHandTop.Position.Y = 3.8f;
            rightHandTop.Position.Z = 0.35f;
            rightHandTop.PostScaleRotation.X = MathF.PI;

            var rightHandBottom = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.1f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.12f, 4), 1);
            rightHandBottom.Material = metal;
            rightHandBottom.Scale.Z = 0.25f;
            rightHandBottom.Position.X = 3;
            rightHandBottom.Position.Y = 3.65f;
            rightHandBottom.Position.Z = 0.75f;
            rightHandBottom.PostScaleRotation.X = -3f / 4 * MathF.PI;

            var frontPanelTop = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.5f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.75f, 4), 0.5f);
            frontPanelTop.Material = metal;
            frontPanelTop.Rotation.Y = MathF.PI / 4;
            frontPanelTop.Scale.Z = 0.15f;
            frontPanelTop.Position.Z = 1.2f;
            frontPanelTop.Position.Y = -0.3f;
            frontPanelTop.TextureIndex = 0;

            var frontPanelTopScreen = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.35f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.55f, 4), 0.4f);
            frontPanelTopScreen.Material = new()
            { Color = new Vector3(0.7f, 0.1f, 0.1f) * 1.5f, SpecularStrength = 1f };
            frontPanelTopScreen.Rotation.Y = MathF.PI / 4;
            frontPanelTopScreen.Scale.Z = 0.15f;
            frontPanelTopScreen.Position.Z = 1.24f;
            frontPanelTopScreen.Position.Y = -0.3f;
            frontPanelTopScreen.TextureIndex = 3;

            var frontPanelBottom = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.72f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.72f, 4), 0.51f);
            frontPanelBottom.Material = metal;
            frontPanelBottom.Rotation.Y = MathF.PI / 4;
            frontPanelBottom.Scale.Z = 0.15f;
            frontPanelBottom.Position.Z = 1.2f;
            frontPanelBottom.Position.Y = -0.8f;
            frontPanelBottom.TextureIndex = 0;

            var frontPanelBottomScreen = Prismatoid.CreateGeneral(
                PolygonHelper.CreateRegular(Vector2.Zero, 0.6f, 4),
                PolygonHelper.CreateRegular(Vector2.Zero, 0.6f, 4), 0.4f);
            frontPanelBottomScreen.Material = new()
            { Color = new Vector3(0.1f, 0.3f, 0.4f) * 1.5f, SpecularStrength = 1f };
            frontPanelBottomScreen.Rotation.Y = MathF.PI / 4;
            frontPanelBottomScreen.Scale.Z = 0.15f;
            frontPanelBottomScreen.Position.Z = 1.25f;
            frontPanelBottomScreen.Position.Y = -0.8f;
            frontPanelBottomScreen.TextureIndex = 3;

            var shape = new List<Polyhedron>
            {
                body, bodyStripe, lowerBody, frontPanel,
                eye, eyeHolder, wheel, wheelSuspension,
                leftArm[0], leftArm[1], leftElbow, leftArmWires[0], leftArmWires[1],
                leftForearm[0], leftForearm[1],
                rightArm[0], rightArm[1], rightElbow,
                rightForearm[0], rightForearm[1], rightForearmWires[0], rightForearmWires[1],
                leftCap, rightCap, antenna[0], antenna[1],
                leftHandCylinder[0], leftHandCylinder[1], leftHandTop, leftHandBottom,
                rightHandCylinder[0], rightHandCylinder[1], rightHandTop, rightHandBottom,
                frontPanelTop, frontPanelTopScreen,
                frontPanelBottom, frontPanelBottomScreen
            };

            for (var i = 0; i < 9; i++)
            {
                var panel = Prismatoid.CreateGeneral(
                    PolygonHelper.CreateRegular(Vector2.Zero, 0.5f, 4),
                    PolygonHelper.CreateRegular(Vector2.Zero, 0.6f, 4),
                    0.05f);
                panel.Rotation.X = MathF.PI;
                panel.Rotation.Y = MathF.PI / 4;
                panel.Position.Y = 0.1f + i * 0.1f;
                panel.Position.Z = 1.25f + i * 0.01f;
                panel.Scale.X = 0.4f;
                panel.Scale.Z = 0.1f;
                panel.Material = new() { Color = new Vector3(1f, 1f, 1) * 0.7f, SpecularStrength = 0.1f };
                panel.TextureIndex = 1;

                shape.Add(panel);
            }

            AddPolyhedron(shape.ToArray());
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
            var torus = RotationSurfaces.CreateTorus(2, 1, 100, 50, round: true);
            AddPolyhedron(torus);

            // угловая спираль
            AddPolyhedron(ComplexShape.CreateHelix(1.5f, 0.2f, 5, 3, 7, 10));
            // спираль
            var spiral = ComplexShape.CreateHelix(1.5f, 0.5f, 50, 35, 5, 3, round: true);
            AddPolyhedron(spiral);

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
        }
    }
}
