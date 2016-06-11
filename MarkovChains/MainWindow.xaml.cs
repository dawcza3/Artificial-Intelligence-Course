using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;

namespace MarkovChains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public Dictionary<int, double> dictXY=new Dictionary<int, double>(); 
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            double[] data = new double[10];
            for (int i = 0; i < data.Length; i++) data[i] = Rand.Normal(0, 1);
            Variable<int> numTimes = Variable.Observed(10);
            Range time = new Range(numTimes);
            VariableArray<double> x = Variable.Array<double>(time);

            using (var block = Variable.ForEach(time))
            {
                var t = block.Index;
               // var i = t.ObservedValue;
                using (Variable.If(t == 0))
                {
                    x[t] = Variable.GaussianFromMeanAndVariance(0, 1);
                    x[t].ObservedValue = data[0];
                    //dictXY.Add(t.ObservedValue,x[t].ObservedValue);
                }
                using (Variable.If(t > 0))
                {
                    x[t] = Variable.GaussianFromMeanAndVariance(x[t - 1], 1);
                    x[t].ObservedValue = data[0];
                    //dictXY.Add(t.ObservedValue, x[t].ObservedValue);
                }
                
            }

            InferenceEngine ie = new InferenceEngine();
            Debug.WriteLine("Probability both coins are heads: " + ie.Infer(x));
            
        }

        public ObservableDataSource<Point> source1 = null;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create source         
/*            source1 = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            source1.SetXYMapping(p => p);

            /#1#/ Add the graph. Colors are not specified and chosen random
            chart.AddLineChart(source1, 2, "Data row");
#1#


            // Start computation process in second thread
            Thread simThread = new Thread(Simulation);
            simThread.IsBackground = true;
            simThread.Start();*/
        }

/*        private void Simulation()
        {
            int i = 0;
            while (true)
            {
                Point p1 = new Point(i * i, i);
                source1.AppendAsync(Dispatcher, p1);
                
                i++;
                Thread.Sleep(1000);

            }
        }*/
    }
}
