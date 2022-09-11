using System.Drawing;

namespace Exercise1
{
    public abstract class Shape
    {
        public Shape(Canvas canvas)
        {
            Canvas = canvas;
        }

        public Canvas Canvas { get; protected set; }

        public Pen Pen { get; set; }

        public abstract void Draw();

        public abstract void Hide();
    }
}
