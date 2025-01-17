using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    /*
    public class WeightedAdjacencyList<E> : WeightedGraphRepresentation<E>
    {
        public override VertexSet Vertices {get => new VertexSet(this);}
        public override EdgeSet Edges {get => new EdgeSet(this);}

        public WeightedAdjacencyList() {}

        private struct Vertex
        {
            public readonly List<Connection> Neighbors {get;}

            public Vertex()
            {
                Neighbors = new();
            }

            public Vertex(List<Connection> neighbors)
            {
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
        private int numVertices = 0;
        private int numEdges = 0;

        public WeightedAdjacencyList(WeightedAdjacencyList<E> other)
        {
            throw new NotImplementedException();
        }

        public override int AddVertex()
        {
            int idx;

            vertices.Add(new());
            idx = vertices.Count - 1;

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
            --numVertices;

            if (vertices.Count < 2 * (vertices.Count - numVertices))
            {
                // TODO: Reallocate
            }
        }

        public override bool ContainsVertex(int vertex)
        {
            return vertex >= 0 && vertex < vertices.Count && vertices[vertex] != null;
        }

        public override void Connect(int from, int to, E data = default)
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

        public override void SetEdgeData(int from, int to, E data = default)
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

        public override void ClearEdges()
        {
            foreach (Vertex v in vertices)
                v.Neighbors.Clear();
            
            numEdges = 0;
        }

        public override void Clear()
        {
            vertices.Clear();
            numEdges = 0;
            numVertices = 0;
        }

        public class VertexSet : IReadOnlyCollection<int>
        {
            public int Count => adj.numVertices;

            private WeightedAdjacencyList<E> adj;

            public VertexSet(WeightedAdjacencyList<E> adj)
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

            private WeightedAdjacencyList<E> adj;
            private int idx = -1;

            public VertexEnumerator(WeightedAdjacencyList<E> adj)
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

            protected WeightedAdjacencyList<E> adj;

            public EdgeSet(WeightedAdjacencyList<E> adj)
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
            
            public object Current => (object) ((IEnumerator<Edge<int, E>>) this).Current;

            private WeightedAdjacencyList<E> adj;
            private IEnumerator<int> vertices;
            private int idx = -1;
            private int[] neighbors = Array.Empty<int>();

            public EdgeEnumerator(WeightedAdjacencyList<E> adj)
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

        private void NewVertex(int idx)
        {
            if (idx < vertices.Count - 1)
                vertices[idx] = new();
            else
            {
                while(vertices.Count - 1 < idx)
                {
                    vertices.Add(new());
                }

                vertices[vertices.Count - 1] = new();
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
    */

    public class WeightedAdjacencyList<E, V> : IWeightedGraphRepresentation<E, V>
    {
        public override IReadOnlyCollection<int> Vertices {get => new VertexSet(this);}
        public override IReadOnlyCollection<Edge<int, E>> Edges {get => new EdgeSet(this);}

        public WeightedAdjacencyList() {}

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
        private int numVertices = 0;
        private int numEdges = 0;

        public WeightedAdjacencyList(WeightedAdjacencyList<E, V> other)
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

            vertices.Add(new(vData));
            idx = vertices.Count - 1;

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
            --numVertices;

            if (vertices.Count < 2 * (vertices.Count - numVertices))
            {
                // TODO: Reallocate
            }
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

        public override void SetEdgeData(int from, int to, E data = default)
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
            
            numEdges = 0;
        }

        public override void Clear()
        {
            vertices.Clear();
            indices.Clear();

            numVertices = 0;
            numEdges = 0;
        }

        public class VertexSet : IReadOnlyCollection<int>
        {
            public int Count => adj.numVertices;

            private WeightedAdjacencyList<E, V> adj;

            public VertexSet(WeightedAdjacencyList<E, V> adj)
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

            private WeightedAdjacencyList<E, V> adj;
            private int idx = -1;

            public VertexEnumerator(WeightedAdjacencyList<E, V> adj)
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

            protected WeightedAdjacencyList<E, V> adj;

            public EdgeSet(WeightedAdjacencyList<E, V> adj)
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

            private WeightedAdjacencyList<E, V> adj;
            private IEnumerator<int> vertices;
            private int idx = -1;
            private int[] neighbors = Array.Empty<int>();

            public EdgeEnumerator(WeightedAdjacencyList<E, V> adj)
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
                    vertices.Add(new(vData));

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