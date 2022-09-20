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
            
            this.pictureBox = pictureBox;
            BackgroundColor = backgroundColor;

            Clear();
        }

        public Graphics Graphics { get; set; }
        public Color BackgroundColor { get; set; }
        public int Width => pictureBox.Width;
        public int Height => pictureBox.Height;

        public void Clear()
        {
            Graphics.Clear(BackgroundColor);
        }
    }
}
