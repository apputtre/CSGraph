using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    public abstract class GraphRepresentation<V, E>
    {
        public abstract IReadOnlyCollection<int> Vertices { get; }
        public abstract IReadOnlyCollection<Edge<int, E>> Edges { get; }

        public abstract int AddVertex(V vData);
        public abstract void RemoveVertex(int idx);
        public abstract void Connect(int v1, int v2, E eData);
        public abstract void Disconnect(int v1, int v2);
        public abstract void SetVertexData(int idx, V vData);
        public abstract V GetVertexData(int idx);
        public abstract void SetEdgeData(int v1, int v2, E eData);
        public abstract E GetEdgeData(int v1, int v2);
        public abstract bool ContainsVertex(int v);
        public abstract bool ContainsConnection(int v1, int v2);
        public abstract bool TryGetIndex(V vData, out int idx);
        public abstract int GetIndex(V vData);
        public abstract int[] GetNeighbors(int idx);
        public abstract void ClearEdges();
        public abstract void Clear();
    }

    public class AdjacencyList<V, E> : GraphRepresentation<V, E>
    {
        public override VertexSet Vertices {get => new VertexSet(this);}
        public override EdgeSet Edges {get => new EdgeSet(this);}

        public AdjacencyList() {}

        private struct Vertex
        {
            public readonly V VertexData {get;}
            public readonly List<Connection> Neighbors {get;}

            public Vertex(V data)
            {
                VertexData = data;
                Neighbors = new();
            }

            public Vertex(V data, List<Connection> neighbors)
            {
                VertexData = data;
                Neighbors = neighbors;
            }

            public void RemoveConnection(int vertex)
            {
                Neighbors.RemoveAt(Neighbors.FindIndex(c => c.Vertex == vertex));
            }

            public Connection GetConnection(int vertex)
            {
                int idx = Neighbors.FindIndex(c => c.Vertex == vertex);

                if (idx == -1)
                    throw new Exception("Nonexistant connection");
                
                return Neighbors[idx];
            }

            public bool TryGetConnection(int vertex, out Connection connection)
            {
                int idx = Neighbors.FindIndex(c => c.Vertex == vertex);
                if (idx == -1)
                {
                    connection = new();
                    return false;
                }

                connection = Neighbors[idx];

                return true;
            }

            public bool IsConnected(int vertex)
            {
                return Neighbors.FindIndex(c => c.Vertex == vertex) != -1;
            }
        }

        private struct Connection
        {
            public readonly int Vertex {get;}
            public readonly E Data {get;}

            public Connection(int vertex, E data = default(E))
            {
                Vertex = vertex;
                Data = data;
            }
        }

        private List<Vertex?> vertices = new();
        private Dictionary<V, int> indices = new();
        private PriorityQueue<int, int> freeIndices = new();
        private int numVertices = 0;
        private int numEdges = 0;

        public AdjacencyList(AdjacencyList<V, E> other)
        {
            foreach (int v in other.Vertices)
                AddVertex(other.GetVertexData(v));
            
            foreach (Edge<int, E> edge in other.Edges)
                Connect(edge.From, edge.To, edge.Data);
        }

        public override int AddVertex(V vData)
        {
            if (TryGetIndex(vData, out int i))
                throw new Exception("Duplicate vertex");

            int idx;

            if (freeIndices.Count > 0)
            {
                idx = freeIndices.Dequeue();
                vertices[idx] = new(vData);
            }
            else
            {
                vertices.Add(new(vData));
                idx = vertices.Count - 1;
            }

            indices[vData] = idx;
            ++numVertices;

            return idx;
        }

        public override void RemoveVertex(int vertex)
        {
            if (!ContainsVertex(vertex))
                throw new Exception("Nonexistant vertex");
            
            foreach (int n in GetNeighbors(vertex))
                GetVertex(n).RemoveConnection(vertex);
            
            vertices[vertex] = null;
            freeIndices.Enqueue(vertex, vertex);
            --numVertices;
        }

        public override bool ContainsVertex(int vertex)
        {
            return vertex >= 0 && vertex < vertices.Count && vertices[vertex] != null;
        }

        public override void Connect(int from, int to, E data = default(E))
        {
            Vertex vFrom = GetVertex(from);

            if (vFrom.TryGetConnection(to, out Connection existingConnection))
            {
                vFrom.RemoveConnection(existingConnection.Vertex);
                vFrom.Neighbors.Add(new Connection(to, data));
            }
            else
                vFrom.Neighbors.Add(new Connection(to, data));
            
            ++numEdges;
        }

        public override void Disconnect(int from, int to)
        {
            if (!ContainsConnection(from, to))
                throw new Exception("Nonexistant edge");
            
            GetVertex(from).RemoveConnection(to);
            --numEdges;
        }

        public override bool ContainsConnection(int from, int to)
        {
            if (!ContainsVertex(from) || !ContainsVertex(to))
                throw new Exception("Nonexistant vertex");
            
            return GetVertex(from).IsConnected(to);
        }

        public override void SetEdgeData(int from, int to, E data = default(E))
        {
            Disconnect(from, to);
            Connect(from, to, data);
        }

        public override E GetEdgeData(int from, int to)
        {
            if (!ContainsConnection(from, to))
                throw new Exception("Nonexistant edge");
            
            return GetVertex(from).GetConnection(to).Data;
        }

        public override void SetVertexData(int vertex, V data)
        {
            if (!ContainsVertex(vertex))
                throw new Exception("Nonexistant vertex");
            
            if (TryGetIndex(data, out int idx))
                throw new Exception("Duplicate vertex");
            
            Vertex existingVertex = GetVertex(vertex);

            vertices[vertex] = new Vertex(data, existingVertex.Neighbors);
        }

        public override V GetVertexData(int vertex)
        {
            if (!ContainsVertex(vertex))
                throw new Exception("Nonexistant vertex");

            return GetVertex(vertex).VertexData;
        }

        public override int[] GetNeighbors(int vertex)
        {
            if (!ContainsVertex(vertex))
                throw new Exception("Nonexistant vertex");
            
            Vertex v = GetVertex(vertex);
            
            int[] neighbors = new int[v.Neighbors.Count];

            for(int i = 0; i < v.Neighbors.Count; ++i)
                neighbors [i] = v.Neighbors[i].Vertex;

            return neighbors;
        }

        public override bool TryGetIndex(V vData, out int idx)
        {
            if (!indices.TryGetValue(vData, out idx))
                return false;

            if (vertices[idx] == null)
                return false;
            
            return true;
        }

        public override int GetIndex(V vData)
        {
            int idx;
            if (!indices.TryGetValue(vData, out idx))
                throw new Exception("Nonexistant vertex");
            
            return idx;
        }

        public override void ClearEdges()
        {
            foreach (Vertex v in vertices)
                v.Neighbors.Clear();
        }

        public override void Clear()
        {
            vertices.Clear();
            indices.Clear();
            freeIndices.Clear();
        }

        public class VertexSet : IReadOnlyCollection<int>
        {
            public int Count => adj.numVertices;

            private AdjacencyList<V, E> adj;

            public VertexSet(AdjacencyList<V, E> adj)
            {
                this.adj = adj;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return new VertexEnumerator(adj);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator) new VertexEnumerator(adj);
            }
        }

        public class VertexEnumerator : IEnumerator<int>
        {
            object IEnumerator.Current => idx;
            int IEnumerator<int>.Current => idx;

            private AdjacencyList<V, E> adj;
            private int idx = -1;

            public VertexEnumerator(AdjacencyList<V, E> adj)
            {
                this.adj = adj;
            }

            public bool MoveNext()
            {
                while(!adj.ContainsVertex(++idx) && idx < adj.vertices.Count) {}

                return idx < adj.vertices.Count;
            }

            public void Reset()
            {
                idx = -1;
            }

            public void Dispose()
            {
                return;
            }
        }

        public class EdgeSet : IReadOnlyCollection<Edge<int, E>>
        {
            public virtual int Count => adj.numEdges;

            protected AdjacencyList<V, E> adj;

            public EdgeSet(AdjacencyList<V, E> adj)
            {
                this.adj = adj;
            }

            public IEnumerator<Edge<int, E>> GetEnumerator()
            {
                return new EdgeEnumerator(adj);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator) new EdgeEnumerator(adj);
            }
        }

        public class EdgeEnumerator : IEnumerator<Edge<int, E>>
        {
            Edge<int, E> IEnumerator<Edge<int, E>>.Current => new(
                vertices.Current,
                neighbors[idx],
                adj.GetEdgeData(vertices.Current, neighbors[idx])
            );
            
            public object Current => (object) ((IEnumerator<Edge<V, E>>) this).Current;

            private AdjacencyList<V, E> adj;
            private IEnumerator<int> vertices;
            private int idx = -1;
            private int[] neighbors = Array.Empty<int>();

            public EdgeEnumerator(AdjacencyList<V, E> adj)
            {
                this.adj = adj;
                vertices = adj.Vertices.GetEnumerator();
            }

            public bool MoveNext()
            {
                if (idx == -1)
                {
                    if (!vertices.MoveNext())
                        return false;
                    
                    neighbors = adj.GetNeighbors(vertices.Current);
                }

                ++idx;

                while(idx >= neighbors.Length)
                {
                    if (!vertices.MoveNext())
                        return false;
                    
                    neighbors = adj.GetNeighbors(vertices.Current);
                    idx = 0;
                }

                return true;
            }

            public void Reset()
            {
                vertices.Reset();
                idx = -1;
                neighbors = Array.Empty<int>();
            }

            public void Dispose()
            {
                vertices.Dispose();
            }
        }

        private void NewVertex(int idx, V vData)
        {
            if (idx < vertices.Count - 1)
                vertices[idx] = new(vData);
            else
            {
                while(vertices.Count - 1 < idx)
                {
                    vertices.Add(new(vData));
                    freeIndices.Enqueue(vertices.Count - 1, vertices.Count - 1);
                }

                vertices[vertices.Count - 1] = new(vData);
            }

            ++numVertices;
        }

        private Vertex GetVertex(int idx)
        {
            if (!ContainsVertex(idx))
                throw new Exception("Nonexistant vertex");
            
            return (Vertex) vertices[idx];
        }
    }
}