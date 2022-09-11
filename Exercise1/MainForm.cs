using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;
using System.Drawing.Imaging;

namespace Exercise1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Draw()
        {
            var canvas = new Canvas(pictureBox, Color.Aqua);
            var line = new Line(canvas, 5,
                new PointF(50, 50), new PointF(canvas.Width - 50, canvas.Height - 50),
                Color.Red, DashStyle.Dash,
                startCap: LineCap.Round,
                endCap: LineCap.Custom, customEndCap: new AdjustableArrowCap(10, 10));
            line.Draw();
            //var json = JsonConvert.SerializeObject(line, Formatting.Indented);
            //sceneSetupTextBox.Text = json;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }
    }
}
