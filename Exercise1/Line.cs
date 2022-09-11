using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Exercise1
{
    public class Line : Shape
    {
        public Line(Canvas canvas, float width, PointF start, PointF end, Color color,
            DashStyle dashStyle = DashStyle.Solid,
            LineCap startCap = LineCap.Flat, LineCap endCap = LineCap.Flat, 
            CustomLineCap customStartCap = null, CustomLineCap customEndCap = null) : base(canvas)
        {
            Canvas = canvas;
            Pen = new Pen(color, width)
            {
                DashStyle = dashStyle,
                StartCap = startCap,
                EndCap = endCap,
            };

            if (customStartCap != null)
            {
                Pen.CustomStartCap = customStartCap;
            }
            if (customEndCap != null)
            {
                Pen.CustomEndCap = customEndCap;
            }

            Start = start;
            End = end;
        }

        public PointF Start { get; set; }
        public PointF End { get; set; }

        public override void Draw()
        {
            Canvas.Graphics.DrawLine(Pen, Start, End);
        }

        public override void Hide()
        {
            var backgroundPen = new Pen(Canvas.BackgroundColor, Pen.Width);
            Canvas.Graphics.DrawLine(backgroundPen, Start, End);
        }
    }
}
