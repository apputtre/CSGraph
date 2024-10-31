
namespace Graph
{
    public class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }

        public Edge(Vertex from, Vertex to)
        {
            From = from;
            To = to;
        }
    }

    public class Edge<TEdgeData> : Edge
    {
        private TEdgeData? data;

        public TEdgeData Data
        {
            get => data;
        }

        public Edge(Vertex from, Vertex to, TEdgeData? data) : base(from, to)
        {
            this.data = data;
        }
    }
}