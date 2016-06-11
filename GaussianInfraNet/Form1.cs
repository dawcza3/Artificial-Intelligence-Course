using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;

namespace GaussianInfraNet
{
    public partial class Form1 : Form
    {
        const float wxmin = -5.1f;
        const float wymin = -0.2f;
        const float wxmax = -wxmin;
        const float wymax = 1.1f;
        const float wwid = wxmax - wxmin;
        const float whgt = wymax - wymin;

        public Form1()
        {
            InitializeComponent();
        }

        private float mean;
        private float precision;
        private float mean2;
        private float precision2;
        private Bitmap bm;

        private void DrawChart()
        {
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                // Define the mapping from world
                // coordinates onto the PictureBox.
                RectangleF world = new RectangleF(wxmin, wymin, wwid, whgt);
                PointF[] device_points =
                {
                    new PointF(0, picGraph.ClientSize.Height),
                    new PointF(picGraph.ClientSize.Width, picGraph.ClientSize.Height),
                    new PointF(0, 0),
                };
                System.Drawing.Drawing2D.Matrix transform = new System.Drawing.Drawing2D.Matrix(world, device_points);


                using (Pen pen = new Pen(Color.Blue, 0))
                {
                    using (Font font = new Font("Arial", 8))
                    {
                        InitializeChart(gr,transform,pen,font);
                        pen.Color = Color.Yellow;
                        DrawData(gr,transform,pen,precision,mean);
                        pen.Color = Color.Blue;
                        DrawData(gr, transform, pen, precision2, mean2);
                    }
                }

                picGraph.Image = bm;
            }
        }

        private void DrawData(Graphics gr, System.Drawing.Drawing2D.Matrix transform,Pen pen,float precision,float mean)
        {
            // Draw the curve.
            gr.Transform = transform;
            List<PointF> points = new List<PointF>();
            float one_over_2pi =
                (float)(1.0 / (precision * Math.Sqrt(2 * Math.PI)));

            float var = precision * precision;
            float dx = (wxmax - wxmin) / picGraph.ClientSize.Width;
            for (float x = wxmin; x <= wxmax; x += dx)
            {
                float y = F(x, one_over_2pi, mean, precision, var);
                points.Add(new PointF(x, y));
            }
            // Sample data from standard Gaussian
            double[] data = new double[100];
            for (int i = 0; i < data.Length; i++) data[i] = Rand.Normal(0, 1);

            for (int i = 0; i < data.Length; i++)
            {
                Variable<double> x = Variable.GaussianFromMeanAndPrecision(mean, precision).Named("x" + i);
                x.ObservedValue = data[i];
                var a = x;
            }
            gr.DrawLines(pen, points.ToArray());
        }

        private void InitializeChart(Graphics gr, System.Drawing.Drawing2D.Matrix transform,Pen pen,Font font)
        {
            Variable<double> xx = Variable.GaussianFromMeanAndVariance(10, 5).Named("xx");
            Variable.ConstrainTrue(xx > 4.0);
            InferenceEngine engine = new InferenceEngine();
            engine.Algorithm = new ExpectationPropagation();
            //Console.WriteLine("Dist over x=" + engine.Infer(xx));
            MessageBox.Show("Dist over x=" + engine.Infer(xx));
            // Draw the X axis.
            gr.Transform = transform;
            pen.Color = Color.Black;
            gr.DrawLine(pen, wxmin, 0, wxmax, 0);
            for (int x = (int)wxmin; x <= wxmax; x++)
            {
                gr.DrawLine(pen, x, -0.05f, x, 0.05f);
                gr.DrawLine(pen, x + 0.25f, -0.025f, x + 0.25f, 0.025f);
                gr.DrawLine(pen, x + 0.50f, -0.025f, x + 0.50f, 0.025f);
                gr.DrawLine(pen, x + 0.75f, -0.025f, x + 0.75f, 0.025f);
            }

            // Label the X axis.
            gr.Transform = new System.Drawing.Drawing2D.Matrix();
            gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            List<PointF> ints = new List<PointF>();
            for (int x = (int)wxmin; x <= wxmax; x++)
                ints.Add(new PointF(x, -0.07f));
            PointF[] ints_array = ints.ToArray();
            transform.TransformPoints(ints_array);

            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Near;
                int index = 0;
                for (int x = (int)wxmin; x <= wxmax; x++)
                {
                    gr.DrawString(x.ToString(), font, Brushes.Black,
                        ints_array[index++], sf);
                }
            }

            // Draw the Y axis.
            gr.Transform = transform;
            pen.Color = Color.Black;
            gr.DrawLine(pen, 0, wymin, 0, wymax);
            for (int y = (int)wymin; y <= wymax; y++)
            {
                gr.DrawLine(pen, -0.2f, y, 0.2f, y);
                gr.DrawLine(pen, -0.1f, y + 0.25f, 0.1f, y + 0.25f);
                gr.DrawLine(pen, -0.1f, y + 0.50f, 0.1f, y + 0.50f);
                gr.DrawLine(pen, -0.1f, y + 0.75f, 0.1f, y + 0.75f);
            }

            // Label the Y axis.
            gr.Transform = new System.Drawing.Drawing2D.Matrix();
            gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            ints = new List<PointF>();
            for (float y = 0.25f; y < 1.01; y += 0.25f)
                ints.Add(new PointF(0.2f, y));
            ints_array = ints.ToArray();
            transform.TransformPoints(ints_array);

            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                int index = 0;
                foreach (float y in new float[] { 0.25f, 0.5f, 0.75f, 1.0f })
                {
                    gr.DrawString(y.ToString("0.00"), font, Brushes.Black,
                        ints_array[index++], sf);
                }
            }

        }

        private bool LoadParameters()
        {
            if (txtMean.Text.Length != 0 && txtStdDev.Text.Length != 0 &&
                textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
            {
                mean = float.Parse(txtMean.Text.Replace(".", ","));
                precision = float.Parse(txtStdDev.Text.Replace(".", ","));
                mean2 = float.Parse(textBox1.Text.Replace(".", ","));
                precision2 = float.Parse(textBox2.Text.Replace(".", ","));
                // Make a bitmap.
                bm = new Bitmap(picGraph.ClientSize.Width, picGraph.ClientSize.Height);
                return true;
            }
            return false;
        }

        private void BtnDraw_Click(object sender, System.EventArgs e)
        {
            if(!LoadParameters()) return;
            DrawChart();
        }

        // The normal distribution function.
        private float F(float x, float one_over_2pi, float mean, float stddev, float var)
        {
            return (float)(one_over_2pi * Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
        }

    }
}