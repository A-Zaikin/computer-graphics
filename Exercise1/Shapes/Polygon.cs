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
            Canvas.Graphics.FillPolygon(new SolidBrush(Pen.Color), Points);
            Canvas.Graphics.DrawString("Polygon", new Font("Arial", 16),
                new SolidBrush(Color.Black), FindCenterPoint());
        }

        public override void Hide()
        {
            Canvas.Graphics.DrawPolygon(new Pen(Canvas.BackgroundColor, Pen.Width), Points);
            Canvas.Graphics.FillPolygon(new SolidBrush(Canvas.BackgroundColor), Points);
            //Canvas.Graphics.DrawString("Polygon", new Font("Arial", 16),
            //    new SolidBrush(Canvas.BackgroundColor), FindCenterPoint());
        }

        private Point FindCenterPoint()
        {
            var centerPoint = new Point(0, 0);
            foreach(var point in Points)
            {
                centerPoint.X += point.X;
                centerPoint.Y += point.Y;
            }
            centerPoint.X /= Points.Length;
            centerPoint.Y /= Points.Length;

            centerPoint.X -= 40;
            centerPoint.Y -= 20;

            return centerPoint;
        }
    }
}
