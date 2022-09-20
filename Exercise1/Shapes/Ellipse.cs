using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    public class Ellipse : Shape
    {
        public Ellipse(Canvas canvas, Rectangle rectangle, Pen pen = null, SolidBrush brush = null) : base(canvas)
        {
            Pen = pen;
            Brush = brush;
            Rectangle = rectangle;
        }

        public Rectangle Rectangle { get; set; }
        public SolidBrush Brush { get; set; }

        public override void Draw()
        {
            if (Brush != null)
            {
                Canvas.Graphics.FillEllipse(Brush, Rectangle);
            }
            if (Pen != null)
            {
                Canvas.Graphics.DrawEllipse(Pen, Rectangle);
            }
        }

        public override void Hide()
        {
            if (Brush != null)
            {
                Canvas.Graphics.FillEllipse(new SolidBrush(Canvas.BackgroundColor), Rectangle);
            }
            if (Pen != null)
            {
                Canvas.Graphics.DrawEllipse(new Pen(Canvas.BackgroundColor, Pen.Width), Rectangle);
            }
        }
    }
}
