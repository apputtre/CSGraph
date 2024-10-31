
namespace Graph
{
    public abstract class GraphRepresentation<TVertex>
    {
        public abstract int[] Vertices {get;}
        public abstract bool ContainsVertex(int index);
        public abstract TVertex AddVertex();
        public abstract void RemoveVertex(int index);
        public virtual void AddEdge(int index1, int index2)
        {
            AddEdge(new Edge(index1, index2));
        }
        public abstract void RemoveEdge(int index1, int index2);
        public abstract int[] GetConnections(int index);
    }

    public abstract class GraphRepresentation : GraphRepresentation<Vertex> { }
}