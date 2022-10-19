using System.Drawing;
using System.Windows.Forms;

namespace Exercise4_cs
{
    public partial class Form1 : Form
    {
        private Bitmap image;
        private float a = 0.7f, b = 0.3f, c = 0.5f, d = 0.3f, x0 = 1;
        private int maxDepth = 20;
        private float scale = 500;
        private int xOffset = 300;

        public Form1()
        {
            InitializeComponent();
        }

        private void aTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (float.TryParse(aTextBox.Text.Replace('.', ','), out a))
                {
                    RedrawImage();
                }
            }
        }

        private void LeafFractal(float x, float y, int depth = 0)
        {
            if (depth == maxDepth)
            {
                return;
            }
            PaintPixel(x, y);

            LeafFractal(
                a * x + b * y,
                b * x - a * y,
                depth + 1);

            LeafFractal(
                c * (x - x0) - d * y + x0,
                d * (x - x0) + c * y,
                depth + 1);
        }

        private void PaintPixel(float x, float y)
        {
            image.SetPixel(
                (int)(x * scale + mainPictureBox.Width / 2 - xOffset),
                (int)(-y * scale + mainPictureBox.Height / 2),
                Color.Green);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            RedrawImage();
        }

        private void RedrawImage()
        {
            if (image != null)
            {
                image.Dispose();
            }
            image = new Bitmap(mainPictureBox.Width, mainPictureBox.Height);
            mainPictureBox.Image = image;
            LeafFractal(1, 0);
        }
    }
}
