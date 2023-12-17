namespace kCores
{
    public abstract class Metrics
    {
        public abstract double Distance(Point p1, Point p2);
        
        public double Distance(Vertice v1, Vertice v2)
        {
            return Distance(v1.Point, v2.Point);    
        }
    }
}
