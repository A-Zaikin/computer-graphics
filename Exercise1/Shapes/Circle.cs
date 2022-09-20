using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    public class Circle : Ellipse
    {
        public Circle(Canvas canvas, Point center, double radius,
            Pen pen = null, SolidBrush brush = null) : base(canvas, CircleToRectangle(center, radius), pen, brush)
        {
        }

        private static Rectangle CircleToRectangle(Point center, double radius)
        {
            var side = (int)(2 * radius / Math.Sqrt(2));
            var size = new Size(side, side);
            return new Rectangle(center, size);
        }
    }
}
