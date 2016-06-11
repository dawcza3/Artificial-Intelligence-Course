using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;

namespace LosoweKroki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chartControl.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right |
                AnchorStyles.Top |
                AnchorStyles.Left;
            label1.BackColor = Color.White;
            label2.BackColor = Color.White;
            label1.Anchor = AnchorStyles.Bottom;
            label1.Text = "Krok czasowy";
            label2.Anchor = AnchorStyles.Left;
            label2.Text = "Pozycja";
            
            // analogiczne do tego co jest na dole ?
            double[] data = new double[10];
            data[0] = Rand.Normal(0, 1);
            for (int i = 1; i < data.Length; i++)
                data[i] = Rand.Normal(data[i-1], 1);

            double[]data2=new double[10];
            data2[0] = Rand.Normal(0, 1);
            data2[1] = Rand.Normal(data2[0], 1);
            for (int i = 2; i < 10; i++)
            {
                data2[i] = Rand.Normal(data2[i - 1] + data2[i - 2], 1);
            }
           
            // z tego nie idzie nic wyciągnąć ? 
            /*
            Variable<int> numTimes = Variable.Observed(10);
            Range time = new Range(numTimes);
            VariableArray<double> x = Variable.Array<double>(time);

            using (var block = Variable.ForEach(time))
            {
                var t = block.Index;
                using (Variable.If(t == 0))
                {
                    x[t] = Variable.GaussianFromMeanAndVariance(0, 1);
                }
                using (Variable.If(t > 0))
                {
                    x[t] = Variable.GaussianFromMeanAndVariance(x[t - 1], 1);
                }

            }
*/
            chartControl.Series.Clear();
            chartControl.Titles.Add("Zależność czasu od pozycji");
            
            Series series = this.chartControl.Series.Add("Wykres 1");
            series.ChartType = SeriesChartType.Spline;
            for (int i = 0; i < 10; i++)
            {
                series.Points.AddXY(i, data[i]);
            }

            Series series2 = this.chartControl.Series.Add("Wykres 2");
            series2.ChartType = SeriesChartType.Spline;
            for (int i = 0; i < 10; i++)
            {
                series2.Points.AddXY(i, data2[i]);
            }
        }
    }
}
