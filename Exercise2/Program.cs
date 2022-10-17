using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exercise2
{
    public static class Program
    {
        public static List<Polygon> Polygons;
        private static Stopwatch timer = new();
        private static Window window;

        public static float Time => timer.ElapsedMilliseconds / 1000f;

        private static void Main()
        {
            CreatePolygons();

            timer.Start();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1600, 600),
                Title = "Exercise 2",
                Flags = ContextFlags.ForwardCompatible,
            };

            window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
            window.Dispose();
        }

        private static void CreatePolygons()
        {
            Polygons = new() {
                new RegularPolygon(new Vector2(150 - 500, 150), 75, 3, -MathF.PI / 6, new Vector3(0.7f, 1, 0.1f)),
                new RegularPolygon(new Vector2(-150 - 500, -150), 75, 4, MathF.PI / 4, new Vector3(0.1f, 0.4f, 1)),
                new RegularPolygon(new Vector2(150 - 500, -150), 75, 50, color: new Vector3(1, 0.3f, 0.2f)),
                new RegularPolygon(new Vector2(-150 - 500, 150), 75, 6, color: new Vector3(0.1f, 1, 1)),
            };

            // Triangle
            Polygons[0].Animations.Add(new Animation(AnimationType.Scale,
                () => Matrix4.CreateScale((MathF.Sin(Time * 3) + MathF.Sin(Time * 5)) / 4f + 0.6f)));

            // Square
            Polygons[1].Animations.Add(new Animation(AnimationType.Rotation,
                () => Matrix4.CreateRotationX(Time * 2)));
            Polygons[1].Animations.Add(new Animation(AnimationType.Rotation,
                () => Matrix4.CreateRotationY(Time * 11 / 5)));

            // Circle
            Polygons[2].Animations.Add(new Animation(AnimationType.Scale,
                () => Matrix4.CreateScale((MathF.Cos(Time * 2.4f) + 1.2f) / 2, 1, 1)));
            Polygons[2].Animations.Add(new Animation(AnimationType.Scale,
                () => Matrix4.CreateScale(1, (MathF.Sin(Time) + 1.2f) / 2, 1)));

            // Hexagon
            Polygons[3].Animations.Add(new Animation(AnimationType.Rotation,
                () => Matrix4.CreateRotationZ(Time * 2)));

            // Eight-figure
            var movingPolygon = new Polygon(new Vector2[]
            {
                new Vector2(250, 50),
                new Vector2(200, 75),
                new Vector2(210, 45),
                new Vector2(225, 100),
                new Vector2(275, 45),
            }, color: new Vector3(1f, 0.3f, 1f));
            movingPolygon.Scale *= Matrix4.CreateScale(2);
            movingPolygon.Translation *= Matrix4.CreateTranslation(0, 100, 0);
            movingPolygon.Animations.Add(new Animation(AnimationType.Translation,
                () => Matrix4.CreateTranslation(MathF.Cos(Time * 2) * 300, MathF.Sin(Time * 4) * 75, 0)));
            Polygons.Add(movingPolygon);

            // Sin function
            var sinPolygon = new Polygon(new Vector2[]
            {
                new Vector2(-100, -50),
                new Vector2(-100, 50),
                new Vector2(0, 100),
                new Vector2(100, 50),
                new Vector2(100, -50),
            }, color: new Vector3(0, 0.6f, 0.7f));
            sinPolygon.Scale *= Matrix4.CreateScale(0.7f);
            sinPolygon.Translation *= Matrix4.CreateTranslation(300, -120, 0);
            sinPolygon.Animations.Add(new Animation(AnimationType.Translation,
                () => Matrix4.CreateTranslation(
                    400 * MathF.Abs((Time - 1) % 4 - 2) - 400,
                    MathF.Sin(Time * 7) * 100, 0)));
            Polygons.Add(sinPolygon);

            // Bouncing
            var position = new Vector2(300, -50);
            var velocity = new Vector2(1, 0.5f);
            var bouncingPolygon = new RegularPolygon(Vector2.Zero, 80, 9, color: new Vector3(1f, 1f, 0.2f));
            bouncingPolygon.Animations.Add(new Animation(AnimationType.Translation, () =>
            {
                var borderPadding = 80;
                if (position.X > window.Size.X / 2 - borderPadding || position.X < -window.Size.X / 2 + borderPadding)
                {
                    velocity.X *= -1;
                }
                if (position.Y > window.Size.Y / 2 - borderPadding || position.Y < -window.Size.Y / 2 + borderPadding)
                {
                    velocity.Y *= -1;
                }
                position += velocity;
                return Matrix4.CreateTranslation(position.X, position.Y, 0);
            }));
            bouncingPolygon.Animations.Add(new Animation(AnimationType.Rotation,
                () => Matrix4.CreateRotationZ(Time * 1.5f)));
            Polygons.Add(bouncingPolygon);

            // Rotate around point
            var rotatingPolygon = new RegularPolygon(new Vector2(400, -200), 50, 50, color: new Vector3(1, 0.7f, 0.7f));
            rotatingPolygon.Scale *= Matrix4.CreateScale(0.5f, 1, 1);
            rotatingPolygon.Animations.Add(new Animation(AnimationType.Rotation,
                () => Polygon.GetRotationMatrix(Time * 3, new Vector2(0, 200))));
            Polygons.Add(rotatingPolygon);
        }
    }
}
