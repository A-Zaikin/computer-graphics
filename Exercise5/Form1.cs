using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise5
{
    public partial class Form1 : Form
    {
        Bitmap image;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            image = (Bitmap)Image.FromFile("image.png");
            pictureBox1.Image = image;
            pictureBox2.Image = GetNewImage(image,
                color => Color.FromArgb(color.A, color.R, 0, 0));
            pictureBox3.Image = GetNewImage(image,
                color => Color.FromArgb(color.A, 0, color.G, 0));
            pictureBox4.Image = GetNewImage(image,
                color => Color.FromArgb(color.A, 0, 0, color.B));
            pictureBox5.Image = GetNewImage(image,
                color => Color.FromArgb(color.A, color.A, color.A));

            pictureBox6.Image = GetNewImage(image,
                color => Color.FromArgb(color.A, 
                (color.R + color.G) / 2,
                (color.G + color.B) / 2,
                (color.B + color.R) / 2));
        }

        private Bitmap GetNewImage(Bitmap image, Func<Color, Color> function)
        {
            var newImage = new Bitmap(image.Width, image.Height);
            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var color = image.GetPixel(x, y);
                    newImage.SetPixel(x, y, function(color));
                }
            }
            return newImage;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //label1.Text = $"X: {e.X}, Y: {e.Y}";
            var color = image.GetPixel(e.X, e.Y);
            label1.Text = $"R: {color.R}\nG: {color.G}\nB: {color.B}\nA: {color.A}";
        }
    }
}
