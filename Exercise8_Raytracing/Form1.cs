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
        }

        private void StartUpdate()
        {
            var raytracer = new Raytracer();
            var i = 0;
            var speed = 0.075f;

            while (true)
            {
                i++;
                raytracer.Spheres[0].Position = new Vector3(
                    MathF.Sin(i * speed) * 2,
                    0,
                    MathF.Cos(i * speed) * 2);

                raytracer.Camera.EyePosition.Y = MathF.Sin(i * speed + 1) * 1.5f + 0.5f;

                var frame = raytracer.RenderFrame();
                Invoke(new Action(() => mainPictureBox.Image = frame));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            task = new Task(StartUpdate);
            task.Start();
        }
    }
}
