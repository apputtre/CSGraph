using System.Collections.Generic;

namespace Graph
{
    public class Graph
    {
        public virtual IEnumerable<Vertex> Vertices => throw new NotImplementedException();
        public virtual IEnumerable<Edge> Edges => throw new NotImplementedException();

        protected GraphRepresentation rep;

        public Graph(bool sparse = true)
        {
            if (sparse)
                rep = new AdjacencyList();
            else
                throw new NotImplementedException();
        }

        public bool ContainsVertex(Vertex vertex)
        {
            return rep.ContainsVertex(vertex);
        }

        public virtual Vertex NewVertex()
        {
            return rep.NewVertex();
        }

        public void RemoveVertex(Vertex toRemove)
        {
            rep.RemoveVertex(toRemove);
        }

        public virtual bool ContainsEdge(Vertex from, Vertex to)
        {
            return rep.AreConnected(from, to) || rep.AreConnected(to, from);
        }

        public virtual void AddEdge(Vertex from, Vertex to)
        {
            AddEdge(from, to);
            AddEdge(to, from);
        }

        public virtual void RemoveEdge(Vertex from, Vertex to)
        {
            rep.RemoveEdge(from, to);

            if (rep.AreConnected(to, from))
                rep.RemoveEdge(to, from);
        }
        public virtual Edge GetEdge(Vertex from, Vertex to)
        {
            return rep.GetEdge(from, to);
        }

        public virtual Vertex[] GetNeighbors(Vertex vertex)
        {
            return rep.GetConnected(vertex);
        }

        public virtual Vertex GetVertex(int index)
        {
            return rep.GetVertex(index);
        }
    }

    public class Graph<V, E> : Graph
    {
        public override IEnumerable<Vertex<V, E>> Vertices => throw new NotImplementedException();
        public override IEnumerable<Edge<V, E>> Edges => throw new NotImplementedException();

        protected GraphRepresentation<V, E> rep;

        public Graph(bool sparse = true)
        {
            if (sparse)
                rep = new AdjacencyList<V, E>(this);
            else
                throw new NotImplementedException();
        }

        public override Vertex<V, E> NewVertex()
        {
            return rep.NewVertex();
        }

        public override bool ContainsEdge(Vertex from, Vertex to)
        {
            return rep.AreConnected(from, to) || rep.AreConnected(to, from);
        }

        public override void AddEdge(Vertex from, Vertex to)
        {
            AddEdge(from, to);
            AddEdge(to, from);
        }

        public void AddEdge(Vertex<V, E> from, Vertex<V, E> to, E? data)
        {
            rep.AddEdge(from, to, data);
        }

        public override void RemoveEdge(Vertex from, Vertex to)
        {
            rep.RemoveEdge(from, to);

            if (rep.AreConnected(to, from))
                rep.RemoveEdge(to, from);
        }
        public override Edge<V, E> GetEdge(Vertex from, Vertex to)
        {
            return rep.GetEdge(from, to);
        }
        public override Vertex[] GetNeighbors(Vertex vertex)
        {
            return rep.GetConnected(vertex);
        }

        public override Vertex<V, E> GetVertex(int index)
        {
            return rep.GetVertex(index);
        }
    }

    /*
    public class Digraph<V, E> : Graph<V, E>
    {
        public override void AddEdge(Vertex from, Vertex to)
        {
            rep.AddEdge(from, to);
        }

        public override void RemoveEdge(Vertex from, Vertex to)
        {
            rep.RemoveEdge(from, to);
        }
    }
    */
}