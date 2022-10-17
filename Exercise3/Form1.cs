using org.matheval;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Exercise3
{
    public partial class Form1 : Form
    {
        private readonly MarkerStyle[] customMarkerStyle = new MarkerStyle[]
        {
            MarkerStyle.Triangle,
            MarkerStyle.Star5,
            MarkerStyle.Square,
            MarkerStyle.Cross,
        };

        private readonly SeriesChartType[] customChartTypes = new SeriesChartType[]
        {
            SeriesChartType.Line,
            SeriesChartType.Point,
            SeriesChartType.StepLine,
            SeriesChartType.Bar,
            SeriesChartType.BoxPlot,
        };

        private int currentChartType;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart()
        {
            mainChart.Series.Clear();
            mainChart.ChartAreas[0].AxisX.Minimum = double.Parse(xMinTextBox.Text.Replace('.', ','));
            mainChart.ChartAreas[0].AxisX.Maximum = double.Parse(xMaxTextBox.Text.Replace('.', ','));
            var xSpan = mainChart.ChartAreas[0].AxisX.Maximum - mainChart.ChartAreas[0].AxisX.Minimum;
            mainChart.ChartAreas[0].AxisY.Minimum = double.Parse(yMinTextBox.Text.Replace('.', ','));
            mainChart.ChartAreas[0].AxisY.Maximum = double.Parse(yMaxTextBox.Text.Replace('.', ','));

            for (var i = 0; i < functionTextBox.Lines.Length; i++)
            {
                var line = functionTextBox.Lines[i].Trim();

                var series = new Series(line)
                {
                    ChartType = customChartTypes[currentChartType],
                    MarkerStep = (int)(xSpan),
                    MarkerStyle = customMarkerStyle[i],
                    MarkerSize = 10,
                };

                var expression = new Expression(line);
                var step = double.Parse(stepTextBox.Text.Replace('.', ','));
                for (var x = mainChart.ChartAreas[0].AxisX.Minimum;
                    x < mainChart.ChartAreas[0].AxisX.Maximum; x += step)
                {
                    expression.Bind("x", x);
                    var y = expression.Eval<double>();
                    series.Points.AddXY(x, y);
                }

                mainChart.Series.Add(series);
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void changeTypeButton_Click(object sender, EventArgs e)
        {
            currentChartType++;
            currentChartType %= customChartTypes.Length;
            UpdateChart();
        }
    }
}
