using System;
using System.IO;

namespace kCores
{
    public class Program
    {
        private static readonly EuclidianMetrics metrics = new EuclidianMetrics();

        
        private static readonly string dir = AppDomain.CurrentDomain.BaseDirectory + "/data/";

        private static readonly double exp = 1000d;

        private static  readonly double rMin = 2.0d;
        private static readonly double rMax = 8.0d;
        private static readonly double rStep = 0.5d;

        private static readonly int boundX = 10;
        private static readonly int boundY = 10;

        static void Main(string[] args)
        {
            
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Cycle("vertices", 50, 10, 30, 10, g => g.VerticesCount);
            Cycle("edges", 50, 10, 30, 10, g => g.EdgesCount);
            Cycle("components", 50, 10, 30, 10, g => g.ComponentsCount);
            Cycle("isolated", 50, 10, 30, 10, g => g.CalculateIsolated());

            Cycle("vertices", 100, 20, 60, 10, g => g.VerticesCount);
            Cycle("edges", 100, 20, 60, 10, g => g.EdgesCount);
            Cycle("components", 100, 20, 60, 10, g => g.ComponentsCount);
            Cycle("isolated", 100, 20, 60, 10, g => g.CalculateIsolated());

            Cycle("vertices", 200, 50, 150, 40, g => g.VerticesCount);
            Cycle("edges", 200, 50, 150, 40, g => g.EdgesCount);
            Cycle("components", 200, 50, 150, 40, g => g.ComponentsCount);
            Cycle("isolated", 200, 50, 150, 40, g => g.CalculateIsolated());
        }


        static void Cycle(string property, int n, int kMin, int kMax, int kStep, Func<Graph, int> propertyFunc)
        {
            Console.WriteLine(property);

            string filePath = dir + $"n{n}_{property}.csv";

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

                        sum += propertyFunc(g);
                    }

                    sum /= exp;

                    result += $"{Math.Round(sum)};";
                }

                result += "\n";
            }

            File.WriteAllText(filePath, result);
        }
    }
}
