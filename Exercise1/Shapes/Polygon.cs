using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Exercise1
{
    public class Polygon : Shape
    {
        public Polygon(Canvas canvas, Pen pen, IEnumerable<Point> points) : base(canvas)
        {
            Points = points.ToArray();
            Pen = pen;
        }

        public Point[] Points { get; set; }

        public override void Draw()
        {
            Canvas.Graphics.DrawPolygon(Pen, Points);
        }

        public override void Hide()
        {
            Canvas.Graphics.DrawPolygon(new Pen(Canvas.BackgroundColor, Pen.Width), Points);
        }
    }
}
