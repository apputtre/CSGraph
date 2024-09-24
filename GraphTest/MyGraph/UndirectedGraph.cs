using System;
using System.Collections.Generic;
using System.Collections;

namespace MyGraph
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
                neighbors[i] = new Vertex<TVertexData, TEdgeData>(this, connections[i], rep.GetVertexData(i));

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