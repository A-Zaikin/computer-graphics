using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Exercise1
{
    public class Canvas
    {
        private PictureBox pictureBox;

        public Canvas(PictureBox pictureBox, Color backgroundColor)
        {
            this.pictureBox = pictureBox;

            var bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics = Graphics.FromImage(bmp);
            pictureBox.Image = bmp;

            Graphics.Clear(backgroundColor);
            this.pictureBox = pictureBox;
            BackgroundColor = backgroundColor;
        }

        public Graphics Graphics { get; set; }
        public Color BackgroundColor { get; set; }
        public float Width => pictureBox.Width;
        public float Height => pictureBox.Height;
    }
}
