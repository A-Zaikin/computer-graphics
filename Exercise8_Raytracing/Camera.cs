using System;
using System.Drawing;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Camera
    {
        public Vector3 EyePosition;
        public Vector3 LookDirection;
        public Size ScreenSize;
        public float HorizontalFov;

        public Camera(Vector3 eyePosition, Vector3 lookDirection, Size screenSize, float horizontalFov)
        {
            EyePosition = eyePosition;
            LookDirection = lookDirection;
            ScreenSize = screenSize;
            HorizontalFov = horizontalFov;
        }

        public Ray[,] CastRays()
        {
            var rays = new Ray[ScreenSize.Width, ScreenSize.Height];
            var horizontalFovInRadians = HorizontalFov / 180 * MathF.PI;
            var verticalFovInRadians = horizontalFovInRadians / ScreenSize.Width * ScreenSize.Height;
            var worldScreenSize = new Vector2(
                2 * MathF.Tan(horizontalFovInRadians / 2),
                2 * MathF.Tan(verticalFovInRadians / 2));

            for (var y = 0; y < ScreenSize.Height; y++)
            {
                var relativePixelY = worldScreenSize.Y / ScreenSize.Height * (y + 0.5f) - worldScreenSize.Y / 2;
                for (var x = 0; x < ScreenSize.Width; x++)
                {
                    var relativePixelPosition = new Vector3(
                        worldScreenSize.X / ScreenSize.Width * (x + 0.5f) - worldScreenSize.X / 2,
                        relativePixelY,
                        1);
                    rays[x, y] = new Ray(EyePosition, Vector3.Normalize(relativePixelPosition));
                }
            }
            return rays;
        }
    }
}
