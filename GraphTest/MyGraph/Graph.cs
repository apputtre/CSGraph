using System.Collections.Generic;

namespace Graph
{
    public abstract class Graph
    {
        public abstract IEnumerable<Vertex> Vertices { get; }
        public abstract IEnumerable<Edge> Edges { get; }

        public abstract bool ContainsVertex(Vertex vertex);
        public abstract TVertex NewVertex();
        public abstract void RemoveVertex(TVertex toRemove);
        public abstract bool ContainsEdge(TVertex from, TVertex to);
        public abstract void AddEdge(TVertex from, TVertex to, TEdgeData? data);
        public virtual void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex);
        public abstract void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data);
        public abstract TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract Edge<TVertexData, TEdgeData> GetEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void SetEdgeData(Vertex<TVertexData, TEdgeData> index1, Vertex<TVertexData, TEdgeData> index2, TEdgeData? data);
        public abstract Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex);
        public abstract Vertex<TVertexData, TEdgeData>[] GetVertices(TVertexData data);
    }

    public abstract class Graph<TVertexData, TEdgeData> : Graph
    {
        protected GraphRepresentation rep;

        new public abstract IEnumerable<Vertex<TVertexData>> Vertices { get; }
        new public abstract IEnumerable<Edge<TEdgeData>> Edges { get; }

        public abstract bool ContainsVertex(TVertex vertex);
        public abstract TVertex NewVertex();
        public abstract void RemoveVertex(TVertex toRemove);
        public abstract bool ContainsEdge(TVertex from, TVertex to);
        public abstract void AddEdge(TVertex from, TVertex to, TEdgeData? data);

        public virtual void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            AddEdge(from, to, default(TEdgeData));
        }

        public abstract void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex);
        public abstract void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data);
        public abstract TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract Edge<TVertexData, TEdgeData> GetEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void SetEdgeData(Vertex<TVertexData, TEdgeData> index1, Vertex<TVertexData, TEdgeData> index2, TEdgeData? data);
        public abstract Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex);
        public abstract Vertex<TVertexData, TEdgeData>[] GetVertices(TVertexData data);
    }

    public abstract class Graph<TVertexData, TEdgeData> : Graph
    {
        protected GraphRepresentation<TVertexData, TEdgeData> rep;

        public virtual Vertex<TVertexData, TEdgeData>[] Vertices
        {
            get
            {
                int[] indices = rep.Vertices;

                List<Vertex<TVertexData, TEdgeData>> ret = new();

                foreach (int i in indices)
                    ret.Add(new Vertex<TVertexData, TEdgeData>(this, i, rep.GetVertexData(i)));
                
                return ret.ToArray();
            }
        }

        public abstract bool ContainsVertex(Vertex<TVertexData, TEdgeData> vertex);
        public abstract Vertex<TVertexData, TEdgeData> AddVertex(TVertexData? data);

        public virtual Vertex<TVertexData, TEdgeData> AddVertex()
        {
            return AddVertex(default(TVertexData));
        }

        public abstract Vertex<TVertexData, TEdgeData> AddVertex(Vertex<TVertexData, TEdgeData> other);

        public abstract void RemoveVertex(Vertex<TVertexData, TEdgeData> toRemove);
        public abstract bool ContainsEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data);

        public virtual void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            AddEdge(from, to, default(TEdgeData));
        }

        public abstract void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex);
        public abstract void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data);
        public abstract TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract Edge<TVertexData, TEdgeData> GetEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to);
        public abstract void SetEdgeData(Vertex<TVertexData, TEdgeData> index1, Vertex<TVertexData, TEdgeData> index2, TEdgeData? data);
        public abstract Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex);
        public abstract Vertex<TVertexData, TEdgeData>[] GetVertices(TVertexData data);
    }
}