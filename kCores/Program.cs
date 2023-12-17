using System;
using System.IO;

namespace kCores
{
    public class Program
    {
        private static readonly EuclidianMetrics metrics = new EuclidianMetrics();

        private static readonly string dir = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly double exp = 1000d;

        private static  readonly double rMin = 2.0d;
        private static readonly double rMax = 8.0d;
        private static readonly double rStep = 0.5d;

        private static readonly int boundX = 10;
        private static readonly int boundY = 10;

        static void Main(string[] args)
        {
            nCycle(100, 20, 60, 10);
        }


        static void nCycle(int n, int kMin, int kMax, int kStep)
        { 
            string filePath = dir + $"n{n}_n.csv";

            string result = "r;";

            for (int k = kMin; k <= kMax; k += kStep)
            {
                result += $"k={k};";
            }

            result += "\n";

            for (double r = rMin; r <= rMax; r += rStep)
            {
                Console.WriteLine(r);

                result += $"{r};";

                double sum = 0;

                for (int k = kMin; k <= kMax; k += kStep)
                {
                    for (int e = 0; e < exp; e++)
                    {
                        Graph g = new Graph(metrics, r);
                        g.Generate(n, boundX, boundY);
                        g.K_Core(k);

                        sum += g.VerticesCount;
                    }

                    sum /= exp;

                    result += $"{Math.Floor(sum)};";
                }

                result += "\n";
            }

            File.WriteAllText(filePath, result);
        }
    }
}
