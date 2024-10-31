using System;

namespace Graph
{
    public class Vertex<TVertexData, TEdgeData>
    {
        private Graph<TVertexData, TEdgeData>? parent;
        private int index;
        private TVertexData? data;

        public Vertex(Graph<TVertexData, TEdgeData>? parent = null, int index = -1, TVertexData? data = default(TVertexData))
        {
            this.parent = parent;
            this.index = index;
            this.data = data;
        }

        public int Index
        {
            get => index;
        }

        public Vertex<TVertexData, TEdgeData>[] Neighbors
        {
            get
            {
                if (parent == null)
                    throw new InvalidOperationException();
                
                return parent.GetNeighbors(this);
            }
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
}