using System.Collections.Generic;

namespace kCores
{
    public class Vertice
    {
        private Point _point;
        private List<Vertice> _neighbours = new List<Vertice>();

        public Point Point => _point;
        public List<Vertice> Neighbours => _neighbours;
        public int Degree => _neighbours.Count;
        public bool IsVisited { get; set; }

        public Vertice(Point point)
        {
            _point = point;
        }

        public void Connect(Vertice another)
        {
            another.AddNeighbour(this);
            AddNeighbour(another);
        }
        
        public void Disconnect(Vertice another)
        {
            another.RemoveNeighbour(this);
            RemoveNeighbour(another);
        }

        public void AddNeighbour(Vertice neighbour)
        {
            _neighbours.Add(neighbour);
        }

        public void RemoveNeighbour(Vertice neighbour)
        {
            _neighbours.Remove(neighbour);
        }

        public void Isolate()
        {
            foreach (Vertice neighbour in _neighbours)
            {
                neighbour.RemoveNeighbour(this);
            }

            _neighbours.Clear();
        }
    }
}
