using System;

namespace kCores
{
    public class EuclidianMetrics : Metrics
    {
        public override double Distance(Point p1, Point p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            return Math.Sqrt(x * x + y * y);
        }
    }
}
