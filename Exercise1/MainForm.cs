using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;

namespace Exercise1
{
    public partial class MainForm : Form
    {
        private List<Shape> shapes = new List<Shape>();
        private Canvas canvas;

        public MainForm()
        {
            InitializeComponent();
            canvas = new Canvas(pictureBox, Color.Aqua);
            CreateStartingShapes();
        }

        private void CreateStartingShapes()
        {
            var line = new Line(canvas, 5,
                new PointF(50, 50), new PointF(canvas.Width - 50, canvas.Height - 50),
                Color.Red, DashStyle.Dash,
                startCap: LineCap.Round,
                endCap: LineCap.Custom, customEndCap: new AdjustableArrowCap(10, 10));
            shapes.Add(line);
            line.Draw();

            var ellipse = new Ellipse(canvas, new Rectangle(100, 100, 200, 300),
                new Pen(Color.Green, 15), new SolidBrush(Color.YellowGreen));
            shapes.Add(ellipse);
            ellipse.Draw();

            var circle = new Circle(canvas, new Point(canvas.Width - 190, 100), 75, new Pen(Color.DarkBlue, 5));
            shapes.Add(circle);
            circle.Draw();

            var points = new Point[]
            {
                new Point(30, canvas.Height - 30),
                new Point(140, canvas.Height - 140),
                new Point(200, canvas.Height - 50)
            };
            var polygon = new Polygon(canvas, new Pen(Color.Yellow, 10), points);
            shapes.Add(polygon);
            polygon.Draw();

            var regularPolygon = new RegularPolygon(canvas, new Pen(Color.DarkMagenta, 12),
                new Point(canvas.Width - 250, canvas.Height - 250), 70, 7);
            shapes.Add(regularPolygon);
            regularPolygon.Draw();
        }


        private void RedrawAllShapes()
        {
            foreach (var shape in shapes)
            {
                shape.Draw();
            }
            pictureBox.Invalidate();
        }

        private void showAllButton_Click(object sender, System.EventArgs e)
        {
            RedrawAllShapes();
        }

        private void hideAllButton_Click(object sender, System.EventArgs e)
        {
            canvas.Clear();
            pictureBox.Invalidate();
        }

        private void drawCircleButton_Click(object sender, System.EventArgs e)
        {
            var center = new Point(
                int.Parse(circleCenterXTextBox.Text),
                int.Parse(circleCenterYTextBox.Text));
            var radius = int.Parse(circleRadiusTextBox.Text);
            var lineWidth = int.Parse(circleWidthTextBox.Text);
            var circle = new Circle(canvas, center, radius, new Pen(Color.DarkBlue, lineWidth));
            shapes.Add(circle);
            circle.Draw();
            pictureBox.Invalidate();
        }

        private void hideLastCircleButton_Click(object sender, System.EventArgs e)
        {
            var circles = shapes.Where(shape => shape is Circle).ToArray();
            if (circles.Length > 0)
            {
                var lastCircle = circles.Last();
                shapes.Remove(lastCircle);
                lastCircle.Hide();
                pictureBox.Invalidate();
            }
            RedrawAllShapes();
        }

        private void drawPolygonButton_Click(object sender, System.EventArgs e)
        {
            var center = new Point(
                int.Parse(polygonCenterXTextBox.Text),
                int.Parse(polygonCenterYTextBox.Text));
            var radius = int.Parse(polygonRadiusTextBox.Text);
            var lineWidth = int.Parse(polygonWidthTextBox.Text);
            var edges = int.Parse(polygonEdgesTextBox.Text);
            if (edges < 2)
            {
                return;
            }
            var polygon = new RegularPolygon(canvas, new Pen(Color.DarkBlue, lineWidth), center, radius, edges);
            shapes.Add(polygon);
            polygon.Draw();
            pictureBox.Invalidate();
        }

        private void hideLastPolygonButton_Click(object sender, System.EventArgs e)
        {
            var regularPolygons = shapes.Where(shape => shape is RegularPolygon).ToArray();
            if (regularPolygons.Length > 0)
            {
                var lastPolygon = regularPolygons.Last();
                shapes.Remove(lastPolygon);
                lastPolygon.Hide();
                pictureBox.Invalidate();
            }
            RedrawAllShapes();
        }
    }
}
