using System;

namespace AlgorytmGenetycznyFunMinMax
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
            
        }
        public double X { get; set; }
        public double Y { get; set; }

        public double GetDistanceFromPosition(double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - X), 2) + Math.Pow((y2 - Y), 2));
        }
    }
}