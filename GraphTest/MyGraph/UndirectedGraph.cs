using System;
using System.Collections.Generic;
using System.Collections;

namespace Graph
{
    public class UndirectedGraph<TVertexData, TEdgeData> : Graph<TVertexData, TEdgeData>
    {
        public UndirectedGraph(bool sparse = true)
        {
            if (sparse)
                rep = new AdjacencyList<TVertexData, TEdgeData>();
            else
                rep = new AdjacencyMatrix<TVertexData, TEdgeData>();
        }

        public UndirectedGraph(Graph<TVertexData, TEdgeData> other, bool sparse = true) : this(sparse)
        {
            foreach (var vertex in other.Vertices)
                AddVertex(vertex);
            
            foreach (var vertex in other.Vertices)
                foreach (var neighbor in vertex.Neighbors)
                    rep.AddEdge(vertex.Index, neighbor.Index, other.GetEdgeData(vertex, neighbor));
        }

        public UndirectedGraph(ICollection<Vertex<TVertexData, TEdgeData>> vertices, bool sparse = true) : this(sparse)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);
        }

        public override bool ContainsVertex(Vertex<TVertexData, TEdgeData> vertex)
        {
            return vertex.GetParentGraph() == this && rep.ContainsVertex(vertex.Index);
        }

        public override Vertex<TVertexData, TEdgeData> AddVertex(TVertexData? data)
        {
            int idx = rep.AddVertex(data);
            Vertex<TVertexData, TEdgeData> vertex = new(this, idx, data);
            return vertex;
        }

        public override Vertex<TVertexData, TEdgeData> AddVertex(Vertex<TVertexData, TEdgeData> other)
        {
            int idx = rep.AddVertex(other.Data);
            Vertex<TVertexData, TEdgeData> vertex = new(this, idx, other.Data);
            return vertex;
        }

        public override void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data)
        {
            rep.AddEdge(from.Index, to.Index, data);
            rep.AddEdge(to.Index, from.Index, data);
        }

        public override void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            rep.RemoveEdge(from.Index, to.Index);
            rep.RemoveEdge(to.Index, from.Index);
        }

        public override TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            return rep.GetEdgeData(from.Index, to.Index);
        }

        public override void SetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data)
        {
            rep.SetEdgeData(from.Index, to.Index, data);
            rep.SetEdgeData(to.Index, from.Index, data);
        }

        public override Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex)
        {
            int[] connections = rep.GetConnections(vertex.Index);

            Vertex<TVertexData, TEdgeData>[] neighbors = new Vertex<TVertexData, TEdgeData>[connections.Length];

            for(int i = 0; i < connections.Length; ++i)
                neighbors[i] = new Vertex<TVertexData, TEdgeData>(this, connections[i], rep.GetVertexData(connections[i]));

            return neighbors;
        }

/*
        public NeighborSet this[Vertex v]
        {
            get
            {
                if (!ContainsVertex(v))
                    throw new ArgumentOutOfRangeException();

                return new NeighborSet(this, v);
            }
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            return new GraphEnumerator(vertices);
        }
        */

        public override void RemoveVertex(Vertex<TVertexData, TEdgeData> toRemove)
        {
            rep.RemoveVertex(toRemove.Index);
        }

        public override TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex)
        {
            return rep.GetVertexData(vertex.Index);
        }

        public override void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data)
        {
            rep.SetVertexData(vertex.Index, data);
        }

        public override Vertex<TVertexData, TEdgeData>[] GetVertices(TVertexData data)
        {
            List<Vertex<TVertexData, TEdgeData>> ret = new();

            foreach (int idx in rep.Vertices)
                if (rep.GetVertexData(idx).Equals(data))
                    ret.Add(new Vertex<TVertexData, TEdgeData>(this, idx, data));

            return ret.ToArray();
        }

        public override bool ContainsEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            return Array.IndexOf(rep.GetConnections(from.Index), to.Index) != -1 || Array.IndexOf(rep.GetConnections(to.Index), from.Index) != -1;
        }

        public override Edge<TVertexData, TEdgeData> GetEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            if (!ContainsVertex(from) || !ContainsVertex(to))
                throw new IndexOutOfRangeException();

            return new Edge<TVertexData, TEdgeData>(from, to, GetEdgeData(from, to));
        }

        /*
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return ((IEnumerable)(this)).GetEnumerator();
                }
                */

        /*
                public class GraphEnumerator : IEnumerator<Vertex>
                {
                    private readonly Vertex[] vertices;
                    int position = -1;

                    public GraphEnumerator(List<Vertex> vertices)
                    {
                        this.vertices = vertices.ToArray();
                    }

                    public bool MoveNext()
                    {
                        return (++position < vertices.Length);
                    }

                    public void Reset()
                    {
                        position = -1;
                    }

                    Vertex IEnumerator<Vertex>.Current
                    {
                        get
                        {
                            try
                            {
                                return vertices[position];
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new InvalidOperationException();
                            }
                        }
                    }

                    public object Current
                    {
                        get => Current;
                    }

                    void IDisposable.Dispose()
                    {
                        // nothing to dispose of!
                    }
                }
                */

    }
}