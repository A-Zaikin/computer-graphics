using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Exercise8_Raytracing
{
    public static class BitmapUtilities
    {
        public static void DrawBitmap(Bitmap source, Bitmap destination, PointF point)
        {
            using var g = Graphics.FromImage(destination);
            g.CompositingMode = CompositingMode.SourceOver;
            var x = point.X - source.Width / 2;
            var y = point.Y - source.Height / 2;
            g.DrawImage(source, new PointF(x, y));
        }

        public static Bitmap UpscaleBitmap(Bitmap original, Size newSize)
        {
            var upscaledBitmap = new Bitmap(newSize.Width, newSize.Height);
            using var graphics = Graphics.FromImage(upscaledBitmap);
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.DrawImage(original, 0, 0, newSize.Width, newSize.Height);
            return upscaledBitmap;
        }
    }
}
