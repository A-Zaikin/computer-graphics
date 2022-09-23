using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise8_Raytracing
{
    public partial class Form1 : Form
    {
        private Task task;

        public Form1()
        {
            InitializeComponent();

            task = new Task(StartUpdate);
            task.Start();
        }

        private void StartUpdate()
        {
            var raytracer = new Raytracer();
            var i = 0;
            var speed = 100;

            while (true)
            {
                i++;
                raytracer.SpherePosition = new Vector3(
                    MathF.Sin(i / 200.0f * speed) * 2,
                    MathF.Cos(i / 200.0f * speed) * 2,
                    0);

                using var frame = raytracer.RenderFrame();
                var upscaledBitmap = new Bitmap(mainPictureBox.Size.Width, mainPictureBox.Size.Height);
                using (var graphics = Graphics.FromImage(upscaledBitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.DrawImage(frame, 0, 0, mainPictureBox.Size.Width, mainPictureBox.Size.Height);
                }
                Invoke(new Action(() => mainPictureBox.Image = upscaledBitmap));
                Thread.Sleep(speed);
            }
        }
    }
}
