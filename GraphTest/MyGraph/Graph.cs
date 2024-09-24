
namespace MyGraph
{
    public abstract class Graph<TVertexData, TEdgeData>
    {
        protected GraphRepresentation<TVertexData, TEdgeData> rep;

        public virtual Vertex<TVertexData, TEdgeData>[] Vertices
        {
            get
            {
                int[] indices = rep.Vertices;

                Vertex<TVertexData, TEdgeData>[] ret = new Vertex<TVertexData, TEdgeData>[indices.Length];

                foreach (int i in indices)
                    ret[i] = new Vertex<TVertexData, TEdgeData>(this, i, rep.GetVertexData(i));
                
                return ret;
            }
        }

        public abstract bool ContainsVertex(Vertex<TVertexData, TEdgeData> vertex);
        public abstract Vertex<TVertexData, TEdgeData> AddVertex(TVertexData? data);

        public virtual Vertex<TVertexData, TEdgeData> AddVertex()
        {
            return AddVertex(default(TVertexData));
        }

        public abstract void RemoveVertex(Vertex<TVertexData, TEdgeData> toRemove);
        public abstract void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data);

        public virtual void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            AddEdge(from, to, default(TEdgeData));
        }

        public abstract void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex);
        public abstract void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data);
        public abstract TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void SetEdgeData(Vertex<TVertexData, TEdgeData> index1, Vertex<TVertexData, TEdgeData> index2, TEdgeData? data);
        public abstract Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex);
    }
}