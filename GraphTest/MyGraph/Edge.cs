
namespace Graph
{
    public class Edge<TVertexData, TEdgeData>
    {
        private TEdgeData? data;

        public TEdgeData Data
        {
            get => data;
        }
        public Vertex<TVertexData, TEdgeData> From {get; set;}
        public Vertex<TVertexData, TEdgeData> To {get; set;}

        public Edge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data)
        {
            From = from;
            To = to;
            this.data = data;
        }
    }
}