
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Graph
{
    public abstract class GraphRepresentation
    {
        public abstract IEnumerable<Vertex> Vertices { get; }
        public abstract IEnumerable<Edge> Edges { get; }

        public abstract Vertex NewVertex();
        public abstract bool ContainsVertex(Vertex vertex);
        public abstract void RemoveVertex(Vertex vertex);
        public abstract void AddEdge(Vertex v1, Vertex v2);
        public abstract Edge? GetEdge(Vertex v1, Vertex v2);
        public abstract Vertex[] GetConnected(Vertex v);
        public abstract bool AreConnected(Vertex v1, Vertex v2);
        public abstract void RemoveEdge(Vertex v1, Vertex v2);
        public abstract Vertex GetVertex(int index);
    }

    public abstract class GraphRepresentation<V, E> : GraphRepresentation
    {
        public abstract override IEnumerable<Vertex<V, E>> Vertices { get; }
        public abstract override IEnumerable<Edge<V, E>> Edges { get; }

        public abstract override Vertex<V, E> NewVertex();
        public abstract void AddEdge(Vertex v1, Vertex v2, E? edgeData);
        public abstract override Edge<V, E>? GetEdge(Vertex v1, Vertex v2);
        public abstract override Vertex<V, E> GetVertex(int index);
    }

    public class AdjacencyList : GraphRepresentation
    {
        public override IEnumerable<Vertex> Vertices => throw new NotImplementedException();
        public override IEnumerable<Edge> Edges => throw new NotImplementedException();

        private List<VertexEntry> _vertices = new();
        private int _nextIdx;
        private Graph _parent;

        public AdjacencyList(Graph parent)
        {
            _parent = parent;
        }

        private record VertexEntry
        {
            public Vertex? Vertex { get; set; }
            public List<Vertex> Neighbors { get; }

            public VertexEntry(Vertex vertex)
            {
                Vertex = vertex;
                Neighbors = new();
            }
        }

        public override bool ContainsVertex(Vertex vertex)
        {
            if (vertex.Index < _vertices.Count)
                return _vertices[vertex.Index].Vertex == vertex;

            return false;
        }

        public override Vertex NewVertex()
        {
            Vertex? vertex = null;

            if (_nextIdx >= _vertices.Count)
                _vertices.Append(new VertexEntry(null));

            vertex = new Vertex(_parent, _nextIdx);
            _vertices[_nextIdx].Vertex = vertex;

            _nextIdx = GetNextFreeIndex();

            return vertex;
        }

        public override void AddEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            _vertices[v1.Index].Neighbors.Add(v2);
        }

        public override Edge? GetEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            if (AreConnected(v1, v2))
                return new Edge(v1, v2);

            return null;
        }

        public override void RemoveEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            foreach (Vertex neighbor in _vertices[v1.Index].Neighbors)
                if (neighbor == v2)
                {
                    _vertices[v1.Index].Neighbors.Remove(neighbor);
                    return;
                }
        }

        public override void RemoveVertex(Vertex vertex)
        {
            if (!ContainsVertex(vertex))
                return;

            foreach (Vertex neighbor in _vertices[vertex.Index].Neighbors)
                RemoveEdge(vertex, neighbor);

            _vertices[vertex.Index] = null;

            if (vertex.Index < _nextIdx)
                _nextIdx = vertex.Index;
        }

        public override Vertex[] GetConnected(Vertex v)
        {
            if (!ContainsVertex(v))
                throw new IndexOutOfRangeException();

            List<Vertex> ret = new();

            foreach (Vertex neighbor in _vertices[v.Index].Neighbors)
                ret.Add(neighbor);

            return ret.ToArray();
        }

        private int GetNextFreeIndex()
        {
            for (int i = _nextIdx; i < _vertices.Count; ++i)
                if (_vertices[_nextIdx].Vertex == null)
                    return i;

            return _vertices.Count;
        }

        public override Vertex GetVertex(int index)
        {
            if (index >= _vertices.Count)
                throw new IndexOutOfRangeException();

            if (_vertices[index] == null)
                return null;

            return _vertices[index].Vertex;
        }
    }

    public class AdjacencyList<V, E> : GraphRepresentation<V, E>
    {
        public override IEnumerable<Vertex<V, E>> Vertices => throw new NotImplementedException();
        public override IEnumerable<Edge<V, E>> Edges => throw new NotImplementedException();

        private Graph<V, E>? _parent;

        public AdjacencyList(Graph<V, E>? parent) : base(parent)
        {
            _parent = parent;
        }

        private record NeighborEntry
        {
            public Vertex<V, E>? Vertex { get; set; }
            public E? EdgeData { get; set; }

            public NeighborEntry(Vertex<V, E> vertex, E? edgeData = default(E))
            {
                Vertex = vertex;
                EdgeData = edgeData;
            }
        }

        private record VertexEntry
        {
            public Vertex<V, E>? Vertex { get; set; }
            public List<NeighborEntry> Neighbors { get; }

            public VertexEntry(Vertex<V, E> vertex)
            {
                Vertex = vertex;
                Neighbors = new();
            }
        }

        private List<VertexEntry> _vertices = new();
        private int _nextIdx = 0;

        public override bool ContainsVertex(Vertex vertex)
        {
            if (vertex.Index < _vertices.Count)
                return _vertices[vertex.Index].Vertex == vertex;

            return false;
        }

        public override Vertex<V, E> NewVertex()
        {
            Vertex<V, E>? vertex = null;

            if (_nextIdx >= _vertices.Count)
                _vertices.Append(new VertexEntry(null));
            
            vertex = new Vertex<V, E>(this, _nextIdx);
            _vertices[_nextIdx].Vertex = vertex;

            _nextIdx = GetNextFreeIndex();

            return vertex;
        }

        public override void AddEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            _vertices[v1.Index].Neighbors.Add(new NeighborEntry(GetVertex(v2.Index)));
        }

        public override void AddEdge(Vertex v1, Vertex v2, E? edgeData)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            _vertices[v1.Index].Neighbors.Add(new NeighborEntry(GetVertex(v2.Index), edgeData));
        }

        public override void RemoveEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            foreach (NeighborEntry entry in _vertices[v1.Index].Neighbors)
                if (entry.Vertex == v2)
                {
                    _vertices[v1.Index].Neighbors.Remove(entry);
                    return;
                }
        }

        public override void RemoveVertex(Vertex vertex)
        {
            if (!ContainsVertex(vertex))
                return;

            foreach (NeighborEntry entry in _vertices[vertex.Index].Neighbors)
                RemoveEdge(vertex, entry.Vertex);

            _vertices[vertex.Index] = null;

            if (vertex.Index < _nextIdx)
                _nextIdx = vertex.Index;
        }

        public override Vertex<V, E>[] GetConnected(Vertex v)
        {
            if (!ContainsVertex(v))
                throw new IndexOutOfRangeException();

            List<Vertex<V, E>> ret = new();

            foreach (NeighborEntry entry in _vertices[v.Index].Neighbors)
                ret.Add(entry.Vertex);

            return ret.ToArray();
        }

        /*
        public override List<Edge<V, E>> GetEdges(Vertex<V, E> vertex)
        {
            if (!ContainsVertex(vertex))
                throw new IndexOutOfRangeException();

            List<Edge<V, E>> ret = new();

            foreach (NeighborEntry entry in _vertices[vertex.Index].Neighbors)
                ret.Add(new Edge<V, E>(vertex, entry.Vertex, entry.EdgeData));

            return ret;
        }
        */

        private int GetNextFreeIndex()
        {
            for (int i = _nextIdx; i < _vertices.Count; ++i)
                if (_vertices[_nextIdx].Vertex == null)
                    return i;

            return _vertices.Count;
        }

        public override Vertex<V, E> GetVertex(int index)
        {
            if (index >= _vertices.Count)
                throw new IndexOutOfRangeException();

            if (_vertices[index] == null)
                return null;

            return _vertices[index].Vertex;
        }

        public override Edge<V, E>? GetEdge(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            if (AreConnected(v1, v2))
                return new Edge<V, E>(GetVertex(v1.Index), GetVertex(v2.Index), _vertices[v1.Index].Neighbors.Find(entry => entry.Vertex == v2).EdgeData);

            return null;
        }

        public override bool AreConnected(Vertex v1, Vertex v2)
        {
            if (!ContainsVertex(v1) || !ContainsVertex(v2))
                throw new IndexOutOfRangeException();

            if (_vertices[v1.Index].Neighbors.Find(entry => entry.Vertex == v2) != null)
                return true;

            return false;
        }
    }
}