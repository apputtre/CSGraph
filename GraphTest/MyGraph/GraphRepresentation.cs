
namespace MyGraph
{
    public abstract class GraphRepresentation<TVertexData, TEdgeData>
    {
        public abstract int[] Vertices {get;}

        public abstract bool ContainsVertex(int index);
        public abstract int AddVertex(TVertexData? data);

        public virtual int AddVertex()
        {
            return AddVertex(default(TVertexData));
        }

        public abstract void RemoveVertex(int index);
        public abstract void AddEdge(int index1, int index2, TEdgeData? data);

        public virtual void AddEdge(int index1, int index2)
        {
            AddEdge(index1, index2, default(TEdgeData));
        }

        public abstract void RemoveEdge(int index1, int index2);
        public abstract TVertexData? GetVertexData(int index);
        public abstract void SetVertexData(int index, TVertexData data);
        public abstract TEdgeData? GetEdgeData(int index1, int index2);
        public abstract void SetEdgeData(int index1, int index2, TEdgeData data);
        public abstract int[] GetConnections(int index);
    }
}