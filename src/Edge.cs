
namespace Graph
{
    public class Edge
    {
        public virtual Vertex From => parent.GetVertex(from);
        public virtual Vertex To => parent.GetVertex(to);

        protected int from;
        protected int to;
        protected Graph? parent;

        public Edge(Graph? parent, int from = -1, int to = -1)
        {
            this.parent = parent;
            this.from = from;
            this.to = to;
        }

        public Edge(Graph parent, Vertex from, Vertex to) : this(parent, from.Index, to.Index) { }
    }

    public class Edge<V, E>
    {
        public Vertex<V, E> From => parent.GetVertex(from);
        public Vertex<V, E> To => parent.GetVertex(to);

        public E? Data { get; set; }

        protected int from;
        protected int to;
        protected Graph<V, E>? parent;

        public Edge(Graph<V, E>? parent = null, int from = -1, int to = -1)
        {
            this.parent = parent;
            this.from = from;
            this.to = to;
        }

        public Edge(Graph<V, E>? parent, Vertex<V, E> from, Vertex<V, E> to, E data) : this(parent, from.Index, to.Index)
        {
            Data = data;
        }
    }
}