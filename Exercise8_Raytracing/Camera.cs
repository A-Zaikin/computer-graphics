using System;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public class Camera
    {
        public Vector3 EyePosition;
        public Vector3 LookDirection;
        public Vector2Int ScreenSize;
        public float HorizontalFov;

        public Camera(Vector3 eyePosition, Vector3 lookDirection, Vector2Int screenSize, float horizontalFov)
        {
            EyePosition = eyePosition;
            LookDirection = lookDirection;
            ScreenSize = screenSize;
            HorizontalFov = horizontalFov;
        }

        public Ray[,] CastRays()
        {
            var rays = new Ray[ScreenSize.X, ScreenSize.Y];
            var verticalFov = HorizontalFov / ScreenSize.X * ScreenSize.Y;
            var worldScreenSize = new Vector2(
                2 * MathF.Tan(HorizontalFov / 2),
                2 * MathF.Tan(verticalFov / 2));

            for (var y = 0; y < ScreenSize.Y; y++)
            {
                var relativePixelY = worldScreenSize.Y / ScreenSize.Y * (y + 0.5f) - worldScreenSize.Y / 2;
                for (var x = 0; x < ScreenSize.X; x++)
                {
                    var relativePixelPosition = new Vector3(
                        worldScreenSize.X / ScreenSize.X * (x + 0.5f) - worldScreenSize.X / 2,
                        relativePixelY,
                        1);
                    rays[x, y] = new Ray(EyePosition, Vector3.Normalize(relativePixelPosition));
                }
            }
            return rays;
        }
    }
}
