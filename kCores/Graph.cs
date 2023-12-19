using System;
using System.Collections.Generic;

namespace kCores
{
    public class Graph
    {
        private double _radius;
        private Metrics _metrics;
        private List<Vertice> _vertices = new List<Vertice>();

        public int VerticesCount => _vertices.Count;
        public int EdgesCount => CalculateEdges(_vertices);
        public int ComponentsCount => ComponentsInfo().Count;
        public int IsolatedCount => CalculateIsolated();


        public Graph(Metrics metrics, double radius = double.PositiveInfinity)
        {
            _radius = radius;
            _metrics = metrics;
        }

        // обход графа в глубину
        private void DFS(Vertice vertice, List<Vertice> component)
        {
            vertice.IsVisited = true;
            
            component.Add(vertice);
        
            foreach (Vertice neighbour in vertice.Neighbours)
            {
                if (!neighbour.IsVisited)
                {
                    DFS(neighbour, component);
                }
            }
        }


        // добавление вершины в граф
        private bool AddVertice(Vertice vertice)
        {
            // массив расстояний от новой вершины до остальных вершин
            double[] distances = new double[_vertices.Count];

            for (int i = 0; i < _vertices.Count; i++)
            {
                // вычисление расстояние с помощью метрики
                distances[i] = _metrics.Distance(vertice, _vertices[i]);

                // если новая вершина совпадает с уже существующей, добавить ее не получится
                if (distances[i] == 0d)
                {
                    return false;
                }
            }

            for (int i = 0; i < _vertices.Count; i++)
            {
                // в случае, если расстояние между вершинами меньше радиуса, создаем между ними ребро
                if (distances[i] <= _radius)
                {
                    vertice.Connect(_vertices[i]);
                }
            }

            // добавляем новую вершину в список вершин
            _vertices.Add(vertice);

            return true;
        }


        // получение информации о компонентах графа
        private List<List<Vertice>> ComponentsInfo()
        {
            List<List<Vertice>> components = new List<List<Vertice>>();

            foreach (Vertice vertice in _vertices)
            {
                vertice.IsVisited = false;
            }

            foreach (Vertice vertice in _vertices)
            {
                if (!vertice.IsVisited)
                {
                    List<Vertice> component = new List<Vertice>();

                    DFS(vertice, component);

                    components.Add(component);
                }
            }
            return components;
        }


        #region СВОЙСТВА
        // количество ребер
        public int CalculateEdges(List<Vertice> vertices)
        {
            int count = 0;

            foreach (Vertice vertice in vertices)
            {
                count += vertice.Degree;
            }

            return count / 2;
        }

        // количество изолированных вершин
        public int CalculateIsolated()
        {
            int count = 0;

            foreach(Vertice vertice in _vertices)
            {
                if (vertice.IsIsolated)
                {
                    count++;
                }
            }

            return count;
        }
        #endregion


        // генерация случайного геометрического графа
        public void Generate(int verticesCount, int boundX, int boundY)
        {
            // удаляем предыдущие вершины
            _vertices.Clear();

            for (int i = 0; i < verticesCount; i++)
            {
                bool isVertexAdded = false;

                while (!isVertexAdded)
                { 
                    Vertice vertice = new Vertice(i, Point.RandomPoint(boundX, boundY));
                    isVertexAdded = AddVertice(vertice);
                }
            }
        }


        // вычисление k-ядра графа
        public void K_Core(int k)
        {
            List<Vertice> deleted = new List<Vertice>();

            do
            {
                deleted.Clear();

                foreach (Vertice vertice in _vertices)
                {
                    if (vertice.Degree < k)
                    {
                        vertice.Isolate();
                        deleted.Add(vertice);
                    }
                }

                foreach (Vertice vertice in deleted)
                {
                    _vertices.Remove(vertice);
                }
            } while (deleted.Count > 0);

        }


        // для отладки
        public void Log()
        {
            Console.WriteLine("-- GRAPH -- ");

            Console.WriteLine($"n = {VerticesCount}");
            Console.WriteLine($"m = {EdgesCount}");
            Console.WriteLine($"r = {_radius}");

            Console.WriteLine();

            foreach (Vertice v in _vertices)
            {
                Console.WriteLine($"VERTICE [{v.ID}] | x = {v.Point.X}, y = {v.Point.Y}, deg = {v.Degree}");
            }

            List<List<Vertice>> components = ComponentsInfo();

            Console.WriteLine($"\nComponents count = {components.Count}\n");

            foreach (List<Vertice> c in components)
            {
                Console.WriteLine($"COMPONENT | n = {c.Count}, m = {CalculateEdges(c)}");
            }

            Console.WriteLine("\n");
        }
    }
}
