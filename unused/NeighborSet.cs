using System.Collections.ObjectModel;
using System.Collections;

namespace MyGraph
{
    public class NeighborSet : ICollection<Vertex>
    {
        private UndirectedGraph parent_graph;
        private Vertex parent_vertex;

        public NeighborSet(UndirectedGraph parent_graph, Vertex parent_vertex)
        {
            this.parent_graph = parent_graph;
            this.parent_vertex = parent_vertex;
        }

        public int Count
        {
            get => parent_graph.GetNeighbors(parent_vertex).Count;
        }

        public bool IsReadOnly { get => false; }

        public void Add(Vertex v)
        {
            parent_graph.AddEdge(parent_vertex, v);
        }

        public bool Remove(Vertex v)
        {
            if (parent_graph.GetNeighbors(parent_vertex).Contains(v))
            {
                parent_graph.RemoveEdge(parent_vertex, v);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            foreach (Vertex v in parent_graph.GetNeighbors(parent_vertex))
                parent_graph.RemoveEdge(parent_vertex, v);
        }

        public bool Contains(Vertex v)
        {
            return parent_graph.GetNeighbors(parent_vertex).Contains(v);
        }

        public void CopyTo(Vertex[] arr, int num)
        {
            List<Vertex> neighbors = parent_graph.GetNeighbors(parent_vertex);

            for (int i = 0; i < num; ++i)
                arr[i] = neighbors[i];
        }

        public int this[Vertex neighbor]
        {
            get => parent_graph.GetWeight(parent_vertex, neighbor);

            // TODO: find out why set accessor is not called
            set
            {
                parent_graph.SetWeight(parent_vertex, neighbor, value);
            } 
        }

        IEnumerator<Vertex> IEnumerable<Vertex>.GetEnumerator()
        {
            return new NeighborSetEnumerator(parent_graph.GetNeighbors(parent_vertex));
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<Vertex>)this).GetEnumerator();
        }

        public class NeighborSetEnumerator : IEnumerator<Vertex>
        {
            private Vertex[] vertices;
            private int position = -1;

            public NeighborSetEnumerator(List<Vertex> vertices)
            {
                this.vertices = vertices.ToArray();
            }

            public bool MoveNext()
            {
                ++position;
                return position < vertices.Length;
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
                        throw (new InvalidOperationException());
                    }
                }
            }

            public object Current
            {
                get => ((IEnumerator<Vertex>)(this)).Current;
            }

            void IDisposable.Dispose()
            {
                // nothing to dispose of!
            }
        }
    }
}
