using System;

namespace Graph
{
    public class Vertex
    {
        public int Index => index;
        public ICollection<Vertex> Neighbors => throw new NotImplementedException();

        protected int index;
        protected Graph? parent;

        public Vertex(Graph? parent = null, int index = -1)
        {
            this.parent = parent;
            this.index = index;
        }
    }

    public class Vertex<V, E> : Vertex
    {
        public V? Data => data;
        new public ICollection<Vertex<V, E>> Neighbors => throw new NotImplementedException();

        protected V? data;

        public Vertex(Graph<V, E>? parent = null, int index = -1)
        {
            this.parent = parent;
            this.index = index;
        }

        public Vertex(Graph<V, E>? parent, int index, V data) : this(parent, index)
        {
            this.data = data;
        }
    }

    /*

    public class Vertex<TVertexData> : Vertex
    {
        new public TVertexData? Data { get; set; }

        private TVertexData? data;

        private Vertex(Graph<Vertex<TVertexData>>? parent = null, int index = -1, TVertexData? data)
        {
            this.parent = parent;
            this.index = index;
            this.data = data;
        }

        new public static Vertex Create(Graph? parent = null, int index = -1, TVertexData? data = null)
        {
            Vertex v = new Vertex(parent, index, data);
            return v;
        }
    }

    public class Vertex<TVertexData, TEdgeData> : Vertex
    {

        private TVertexData? data;

        public Vertex(Graph? parent = null, int index = -1, TVertexData? data = default(TVertexData))
        {
            this.parent = parent;
            this.index = index;
            this.data = data;
        }

        public TVertexData? Data
        {
            get
            {
                return parent.GetVertexData(this);
            }
            set
            {
                parent.SetVertexData(this, value);
            }
        }

        public Graph<TVertexData, TEdgeData>? GetParentGraph()
        {
            return parent;
        }

        public static bool operator ==(Vertex<TVertexData, TEdgeData>? lhs, Vertex<TVertexData, TEdgeData>? rhs)
        {
            if (lhs is null || rhs is null)
                return lhs is null && rhs is null;

            return lhs.parent == rhs.parent && lhs.Index == rhs.Index;
        }

        public static bool operator !=(Vertex<TVertexData, TEdgeData>? lhs, Vertex<TVertexData, TEdgeData>? rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object? o)
        {
            if (o != null && o.GetType() == typeof(Vertex<TVertexData, TEdgeData>))
                return (Vertex<TVertexData, TEdgeData>)o == this;

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(parent, Index).GetHashCode();
        }
    }
    */
}