using System.Collections.Generic;
using System.Collections;
using CSGraph.Implementation;

namespace CSGraph
{
    public readonly struct Edge<V>
    {
        public readonly V From {get;}
        public readonly V To {get;}

        public Edge(V from, V to)
        {
            From = from;
            To = to;
        }
    }

    public readonly struct Edge<V, E>
    {
        public readonly V From {get;}
        public readonly V To {get;}
        public readonly E Data {get;}

        public Edge(V from, V to, E data = default)
        {
            From = from;
            To = to;
            Data = data;
        }
    }

    public interface IGraph
    {
        public abstract IReadOnlyCollection<int> Vertices {get;}
        public abstract IReadOnlyCollection<Edge<int>> Edges {get;}

        public abstract int AddVertex();
        public abstract bool ContainsVertex(int vertex);
        public abstract void RemoveVertex(int vertex);
        public abstract bool ContainsEdge(int from, int to);
        public abstract void AddEdge(int from, int to);
        public abstract void RemoveEdge(int from, int to);
        public abstract Edge<int>[] GetEdges(int vertex);
        public abstract int[] GetNeighbors(int vertex);
    }

    public interface IWeightedGraph<E>
    {
        public abstract IReadOnlyCollection<int> Vertices {get;}
        public abstract IReadOnlyCollection<Edge<int, E>> Edges {get;}

        public abstract int AddVertex();
        public abstract bool ContainsVertex(int vertex);
        public abstract void RemoveVertex(int vertex);
        public abstract bool ContainsEdge(int from, int to);
        public abstract void AddEdge(int from, int to);
        public abstract void RemoveEdge(int from, int to);
        public abstract Edge<int>[] GetEdges(int vertex);
        public abstract int[] GetNeighbors(int vertex);
        public abstract void AddEdge(int from, int to, E eData);
        public abstract void SetEdgeData(int from, int to, E data);
        public abstract E GetEdgeData(int from, int to);
    }

    public interface IGraph<V>
    {
        public abstract IReadOnlyCollection<V> Vertices {get;}
        public abstract IReadOnlyCollection<Edge<V>> Edges {get;}

        public abstract void AddVertex(V vData);
        public abstract bool ContainsVertex(V vertex);
        public abstract void RemoveVertex(V vertex);
        public abstract bool ContainsEdge(V from, V to);
        public abstract void AddEdge(V from, V to);
        public abstract void RemoveEdge(V from, V to);
        public abstract Edge<V>[] GetEdges(V vertex);
        public abstract V[] GetNeighbors(V vertex);
    }

    public interface IWeightedGraph<E, V>
    {
        public abstract IReadOnlyCollection<V> Vertices {get;}
        public abstract IReadOnlyCollection<Edge<V, E>> Edges {get;}

        public abstract void AddVertex(V vData);
        public abstract bool ContainsVertex(V vertex);
        public abstract void RemoveVertex(V vertex);
        public abstract bool ContainsEdge(V from, V to);
        public abstract void RemoveEdge(V from, V to);
        public abstract Edge<V>[] GetEdges(V vertex);
        public abstract V[] GetNeighbors(V vertex);
        public abstract void AddEdge(V from, V to, E eData);
        public abstract void AddEdge(V from, V to);
        public abstract E GetEdgeData(V from, V to);
        public abstract void SetEdgeData(V from, V to, E data);
    }

    public abstract class GraphImpl<TVertex, TConnection>
        where TVertex : struct, IVertex<TConnection>
        where TConnection : struct, IConnection
    {
        protected AdjacencyList<TVertex, TConnection> adj = new();
        protected int numVertices = 0;

        protected virtual int CreateVertex(TVertex vertex)
        {
            adj.Vertices.Add(vertex);
            ++numVertices;
            return adj.Vertices.Count - 1;
        }

        protected void CreateConnection(int fromIdx, TConnection connection)
        {
            if (!TryGetVertex(fromIdx, out var fromVertex))
                throw new Exception("Nonexistant vertex");
            
            fromVertex.Connections.Add(connection);
        }

        protected void RemoveEdgeImpl(int from, int to)
        {
            if (!TryGetConnection(from, to, out TConnection c1) || !TryGetConnection(to, from, out TConnection c2))
                throw new Exception("Nonexistant edge");
            
            TryGetVertex(from, out var v1);
            TryGetVertex(to, out var v2);

            int idx = v1.Connections.FindIndex(c => c.To == to);
            v1.Connections[idx] = v1.Connections[^1];
            v1.Connections.RemoveAt(v1.Connections.Count - 1);

            idx = v2.Connections.FindIndex(c => c.To == from);
            v2.Connections[idx] = v2.Connections[^1];
            v2.Connections.RemoveAt(v2.Connections.Count - 1);
        }

        protected int[] GetNeighborsImpl(int vertex)
        {
            if (!TryGetVertex(vertex, out var v))
                throw new Exception("Nonexistant vertex");
            
            List<int> neighbors = new();

            foreach (TConnection c in v.Connections)
                if (!ContainsVertexImpl(c.To))
                    continue;
                else
                    neighbors.Add(c.To);

            return neighbors.ToArray();
        }

        protected void RemoveVertexImpl(int vertex)
        {
            if (!ContainsVertexImpl(vertex))
                throw new Exception("Nonexistant vertex");

            adj.Vertices[vertex] = null;
            --numVertices;

            if ((adj.Vertices.Count - numVertices) > 2 * numVertices)
                adj.Trim();
        }

        protected virtual bool TryGetVertex(int idx, out TVertex vertex)
        {
            vertex = new();

            if (!ContainsVertexImpl(idx))
                return false;
            
            vertex = adj.Vertices[idx].GetValueOrDefault();
            return true;
        }

        protected virtual bool ContainsVertexImpl(int vertex)
        {
            return vertex >= 0 && vertex < adj.Vertices.Count && adj.Vertices[vertex] != null;
        }

        protected virtual bool TryGetConnection(int fromIdx, int toIdx, out TConnection connection)
        {
            connection = new();

            if (!TryGetVertex(fromIdx, out TVertex fromVertex) || !TryGetVertex(toIdx, out TVertex toVertex))
                return false;
            
            int idx = fromVertex.Connections.FindIndex(c => c.To == toIdx);

            if (idx == -1)
                return false;
            
            connection = fromVertex.Connections[idx];

            return true;
        }
    }

    public class Graph : GraphImpl<Vertex<Connection>, Connection>, IGraph
    {
        public IReadOnlyCollection<int> Vertices => throw new NotImplementedException();
        public IReadOnlyCollection<Edge<int>> Edges => throw new NotImplementedException();

        public Graph() {}

        public int AddVertex()
        {
            return CreateVertex(new());
        }

        public void AddEdge(int fromIdx, int toIdx)
        {
            CreateConnection(fromIdx, new(toIdx));
            CreateConnection(toIdx, new(fromIdx));
        }

        public bool ContainsVertex(int vertex)
        {
            return ContainsVertexImpl(vertex);
        }

        public bool ContainsEdge(int from, int to)
        {
            return TryGetConnection(from, to, out var connection);
        }

        public Edge<int>[] GetEdges(int vertex)
        {
            return Array.ConvertAll(GetNeighbors(vertex), idx => new Edge<int>(vertex, idx)).ToArray();
        }

        public int[] GetNeighbors(int vertex)
        {
            return GetNeighborsImpl(vertex);
        }

        public void RemoveEdge(int from, int to)
        {
            RemoveEdgeImpl(from, to);
        }

        public void RemoveVertex(int vertex)
        {
            RemoveVertexImpl(vertex);
        }

        public void Trim()
        {
            adj.Trim();
        }
    }

    public class WeightedGraph<E> : GraphImpl<Vertex<Connection<E>>, Connection<E>>, IWeightedGraph<E>
    {
        public IReadOnlyCollection<int> Vertices => throw new NotImplementedException();
        public IReadOnlyCollection<Edge<int, E>> Edges => throw new NotImplementedException();

        public void AddEdge(int from, int to, E eData)
        {
            CreateConnection(from, new(to, eData));
            CreateConnection(to, new(from, eData));
        }

        public void AddEdge(int from, int to)
        {
            CreateConnection(from, new(to, default));
            CreateConnection(to, new(from, default));
        }

        public int AddVertex()
        {
            return CreateVertex(new());
        }

        public bool ContainsEdge(int from, int to)
        {
            return TryGetConnection(from, to, out var connection);
        }

        public bool ContainsVertex(int vertex)
        {
            return ContainsVertexImpl(vertex);
        }

        public E GetEdgeData(int from, int to)
        {
            if (!TryGetConnection(from, to, out Connection<E> c))
                throw new Exception("Nonexistant edge");
            
            return c.Data;
        }

        public Edge<int>[] GetEdges(int vertex)
        {
            return Array.ConvertAll(GetNeighbors(vertex), idx => new Edge<int>(vertex, idx)).ToArray();
        }

        public int[] GetNeighbors(int vertex)
        {
            return GetNeighborsImpl(vertex);
        }

        public void RemoveEdge(int from, int to)
        {
            RemoveEdgeImpl(from, to);
        }

        public void RemoveVertex(int vertex)
        {
            RemoveVertexImpl(vertex);
        }

        public void SetEdgeData(int from, int to, E data)
        {
            RemoveEdgeImpl(from, to);
            CreateConnection(from, new Connection<E>(to, data));
            CreateConnection(to, new Connection<E>(from, data));
        }
    }

    public class Graph<V> : GraphImpl<Vertex<V, Connection>, Connection>, IGraph<V>
    {
        public IReadOnlyCollection<V> Vertices => throw new NotImplementedException();
        public IReadOnlyCollection<Edge<V>> Edges => throw new NotImplementedException();

        protected Dictionary<V, int> indices = new();

        public void AddVertex(V vData)
        {
            int idx = CreateVertex(new(vData));
            indices[vData] = idx;
        }

        public void AddEdge(V from, V to)
        {
            if (!TryGetVertexByData(from, out var fromVertex) || !TryGetVertexByData(to, out var toVertex))
                throw new Exception("Nonexistant vertex");
            
            TryGetIndexByData(from, out int fromIdx);
            TryGetIndexByData(to, out int toIdx);
            
            CreateConnection(fromIdx, new Connection(toIdx));
            CreateConnection(toIdx, new Connection(fromIdx));
        }

        public bool ContainsVertex(V vData)
        {
            if (!indices.ContainsKey(vData))
                return false;
            
            int idx = indices[vData];
            return ContainsVertexImpl(idx);
        }

        public bool ContainsEdge(V from, V to)
        {
            return TryGetConnectionByData(from, to, out Connection c);
        }

        public V[] GetNeighbors(V vData)
        {
            if (!TryGetVertexByData(vData, out var v))
                throw new Exception("Nonexistant vertex");
            
            List<V> neighbors = new();

            foreach (Connection c in v.Connections)
                if (!ContainsVertexImpl(c.To))
                    continue;
                else
                    neighbors.Add(adj.Vertices[c.To].GetValueOrDefault().Data);

            return neighbors.ToArray();
        }

        public Edge<V>[] GetEdges(V vertex)
        {
            return Array.ConvertAll(GetNeighbors(vertex), vData => new Edge<V>(vertex, vData));
        }

        public void RemoveEdge(V from, V to)
        {
            if (!TryGetConnectionByData(from, to, out Connection c1) || !TryGetConnectionByData(to, from, out Connection c2))
                throw new Exception("Nonexistant edge");
            
            TryGetIndexByData(from, out int fromIdx);
            TryGetIndexByData(to, out int toIdx);

            RemoveEdgeImpl(fromIdx, toIdx);
        }

        public void RemoveVertex(V vertex)
        {
            if (!TryGetIndexByData(vertex, out int idx))
                throw new Exception("Nonexistant vertex");

            RemoveVertexImpl(idx);
        }

        public void Trim()
        {
            adj.Trim();
        }

        private bool TryGetVertexByData(V vData, out Vertex<V, Connection> vertex)
        {
            vertex = new();

            if (!TryGetIndexByData(vData, out int idx))
                return false;
            
            vertex = adj.Vertices[idx].GetValueOrDefault();
            return true;
        }

        private bool TryGetIndexByData(V vData, out int idx)
        {
            idx = -1;

            if (!indices.ContainsKey(vData))
                return false;
            
            idx = indices[vData];
            
            if (!ContainsVertexImpl(idx))
                return false;

            return true;
        }

        private bool TryGetConnectionByData(V from, V to, out Connection connection)
        {
            connection = new();

            if (!TryGetIndexByData(from, out int fromIdx) || !TryGetIndexByData(to, out int toIdx))
                return false;
            
            return TryGetConnection(fromIdx, toIdx, out Connection c);
        }
    }

    public class WeightedGraph<E, V> : GraphImpl<Vertex<V, Connection<E>>, Connection<E>>, IWeightedGraph<E, V>
    {
        public IReadOnlyCollection<V> Vertices => throw new NotImplementedException();
        public IReadOnlyCollection<Edge<V, E>> Edges => throw new NotImplementedException();

        Dictionary<V, int> indices = new();

        public void AddVertex(V vData)
        {
            int idx = CreateVertex(new(vData));
            indices[vData] = idx;
        }

        public void AddEdge(V from, V to, E eData)
        {
            if (!TryGetVertexByData(from, out var fromVertex) || !TryGetVertexByData(to, out var toVertex))
                throw new Exception("Nonexistant vertex");
            
            TryGetIndexByData(from, out int fromIdx);
            TryGetIndexByData(to, out int toIdx);
            
            CreateConnection(fromIdx, new Connection<E>(toIdx, eData));
            CreateConnection(toIdx, new Connection<E>(fromIdx, eData));
        }

        public void AddEdge(V from, V to)
        {
            AddEdge(from, to, default);
        }

        public bool ContainsVertex(V vData)
        {
            if (!indices.ContainsKey(vData))
                return false;
            
            int idx = indices[vData];
            return ContainsVertexImpl(idx);
        }

        public bool ContainsEdge(V from, V to)
        {
            return TryGetConnectionByData(from, to, out Connection<E> c);
        }

        public V[] GetNeighbors(V vData)
        {
            if (!TryGetVertexByData(vData, out var v))
                throw new Exception("Nonexistant vertex");
            
            List<V> neighbors = new();

            foreach (Connection<E> c in v.Connections)
                if (!ContainsVertexImpl(c.To))
                    continue;
                else
                    neighbors.Add(adj.Vertices[c.To].GetValueOrDefault().Data);

            return neighbors.ToArray();
        }

        public Edge<V>[] GetEdges(V vertex)
        {
            return Array.ConvertAll(GetNeighbors(vertex), vData => new Edge<V>(vertex, vData));
        }

        public E GetEdgeData(V from, V to)
        {
            if (!TryGetConnectionByData(from, to, out Connection<E> c))
                throw new Exception("Nonexistant edge");
            
            return c.Data;
        }

        public void SetEdgeData(V from, V to, E data)
        {
            if (!TryGetIndexByData(from, out var fromIdx) || !TryGetIndexByData(to, out var toIdx))
                throw new Exception("Nonexistant vertex");

            RemoveEdge(from, to);
            CreateConnection(fromIdx, new Connection<E>(toIdx, data));
            CreateConnection(toIdx, new Connection<E>(fromIdx, data));
        }

        public void RemoveEdge(V from, V to)
        {
            if (!TryGetConnectionByData(from, to, out Connection<E> c1) || !TryGetConnectionByData(to, from, out Connection<E> c2))
                throw new Exception("Nonexistant edge");
            
            TryGetIndexByData(from, out int fromIdx);
            TryGetIndexByData(to, out int toIdx);

            RemoveEdgeImpl(fromIdx, toIdx);
        }

        public void RemoveVertex(V vertex)
        {
            if (!TryGetIndexByData(vertex, out int idx))
                throw new Exception("Nonexistant vertex");

            RemoveVertexImpl(idx);
        }

        public void Trim()
        {
            adj.Trim();
        }

        private bool TryGetVertexByData(V vData, out Vertex<V, Connection<E>> vertex)
        {
            vertex = new();

            if (!TryGetIndexByData(vData, out int idx))
                return false;
            
            vertex = adj.Vertices[idx].GetValueOrDefault();
            return true;
        }

        private bool TryGetIndexByData(V vData, out int idx)
        {
            idx = -1;

            if (!indices.ContainsKey(vData))
                return false;
            
            idx = indices[vData];
            
            if (!ContainsVertexImpl(idx))
                return false;

            return true;
        }

        private bool TryGetConnectionByData(V from, V to, out Connection<E> connection)
        {
            connection = new();

            if (!TryGetIndexByData(from, out int fromIdx) || !TryGetIndexByData(to, out int toIdx))
                return false;
            
            return TryGetConnection(fromIdx, toIdx, out Connection<E> c);
        }
    }
}