using System;
using System.IO;

namespace kCores
{
    public class Program
    {
        private static readonly EuclidianMetrics metrics = new EuclidianMetrics();
        
        private static readonly string dir = AppDomain.CurrentDomain.BaseDirectory + "/data/";

        private static readonly int boundX = 10;
        private static readonly int boundY = 10;

        static void Main(string[] args)
        {
            
            if (!File.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }


            // PropertyRelationOfK("vertices", 50, Range(0, 10, 1), Range(1.0, 3.0, 0.5), g => g.VerticesCount);
            PropertyRelationOfR("edges", 50, Range(0, 10, 1), Range(1.0, 3.0, 0.5), g => g.EdgesCount);
            // PropertyRelationOfR("components", 50, Range(1, 6, 1), Range(0, 3.0, 0.2), g => g.ComponentsCount);
            
            
            // PropertyRelationOfK("components", 200, Range(1, 10, 1), Range(0.3, 2.4, 0.3), g => g.ComponentsCount);
            // PropertyRelationOfK("vertices", 100, Range(1, 10, 1), Range(0.3, 2.4, 0.3), g => g.VerticesCount);

        }

        static double[] Range(double min, double max, double step)
        {
            double[] values = new double[(int)((max - min) / step) + 1];

            int i = 0;

            for (double k = min; k < max + step; k += step, i++)
            {
                values[i] = k;
            }

            return values;
        }

        static int[] Range(int min, int max, int step)
        {
            int[] values = new int[(max - min) / step + 1];

            int i = 0;

            for (int k = min; k <= max; k += step, i++)
            {
                values[i] = k;
            }

            return values;
        }

        static void PropertyRelationOfR(string property, int n, int[] k, double[] r, Func<Graph, int> propertyFunc)
        {
            Console.WriteLine($"{property} relation of r");

            string filePath = dir + $"{property}_r_n{n}.csv";

            string result = "r;";

            for (int i = 0; i < k.Length; i++)
            {
                result += $"k = {k[i]};";
            }

            result += '\n';

            for (int i = 0; i < r.Length; i++)
            {
                Console.WriteLine($"r = {r[i]}");

                result += $"{r[i]};";

                for (int j = 0; j < k.Length; j++)
                {
                    result += $"{PropertyAverageValue(n, k[j], r[i], propertyFunc)};";
                }

                result += '\n';
            }

            File.WriteAllText(filePath, result);
        }

        static void PropertyRelationOfK(string property, int n, int[] k, double[] r, Func<Graph, int> propertyFunc)
        {
            Console.WriteLine($"{property} relation of k");

            string filePath = dir + $"{property}_k_n{n}.csv";

            string result = "k;";

            for (int i = 0; i < r.Length; i++)
            {
                result += $"r = {r[i]};";
            }

            result += '\n';

            for (int i = 0; i < k.Length; i++)
            {
                Console.WriteLine($"k = {k[i]}");

                result += $"{k[i]};";

                for (int j = 0; j < r.Length; j++)
                {
                    result += $"{PropertyAverageValue(n, k[i], r[j], propertyFunc)};";
                }

                result += '\n';
            }

            File.WriteAllText(filePath, result);
        }

        static double PropertyAverageValue(int n, int k, double r, Func<Graph, int> propertyFunc)
        {
            int exp = 1000;
            double sum = 0;
            
            Graph g = new Graph(metrics, r);

            for (int e = 0; e < exp; e++)
            {
                g.Generate(n, boundX, boundY);
                g.K_Core(k);

                sum += propertyFunc(g);
            }


            return Math.Round(sum / exp);
        }
    }
}

