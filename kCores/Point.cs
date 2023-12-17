using System;

namespace kCores
{
    public class Point
    {
        private static Random random = new Random();

        private double _x = 0d;
        private double _y = 0d;

        public double X { get => _x; private set { } }
        public double Y { get => _y; private set { } }

        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static Point RandomPoint(double boundX, double boundY)
        {
            double x = random.NextDouble() * boundX;
            double y = random.NextDouble() * boundY;

            return new Point(x, y);
        }
    }
}
