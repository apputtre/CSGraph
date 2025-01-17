using System.Collections.Generic;
using System.Collections;
using Graph.Implementation;

namespace Graph.Implementation
{
    public class Vertex<TConnection>
        where TConnection : Connection, new()
    {
        public List<TConnection> Neighbors {get;}

        public Vertex()
        {
            Neighbors = new();
        }

        public Vertex(List<TConnection> neighbors)
        {
            Neighbors = neighbors;
        }

        public void RemoveConnection(int vertex)
        {
            Neighbors.RemoveAt(Neighbors.FindIndex(c => c.To == vertex));
        }

        public TConnection GetConnection(int vertex)
        {
            int idx = Neighbors.FindIndex(c => c.To == vertex);

            if (idx == -1)
                throw new Exception("Nonexistant connection");
            
            return Neighbors[idx];
        }

        public bool TryGetConnection(int vertex, out TConnection connection)
        {
            int idx = Neighbors.FindIndex(c => c.To == vertex);
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
            return Neighbors.FindIndex(c => c.To == vertex) != -1;
        }
    }

    public class Vertex<V, TConnection> : Vertex<TConnection>
        where TConnection : Connection, new()
    {
        public V Data {get; set;}

        public Vertex() : base()
        {
            Data = default;
        }

        public Vertex(V data, List<TConnection> neighbors) : base(neighbors)
        {
            Data = data;
        }
    }

    public class Connection
    {
        public int To {get; set;}

        public Connection()
        {
            To = default;
        }

        public Connection(int to)
        {
            To = to;
        }
    }

    public class Connection<E> : Connection
    {
        public E Data {get; set;}

        public Connection() : base()
        {
            Data = default;
        }

        public Connection(int to, E data) : base(to)
        {
            Data = data;
        }
    }

    public abstract class EdgeEnumeratorImp<TVertex, TConnection, TEdge> : IEnumerator<TEdge>
        where TVertex : Vertex<TConnection>
        where TConnection : Connection, new()
    {
        protected List<TVertex?> vertices;
        protected IEnumerator<int> vEnum;
        protected int numEdges;
        protected int idx = -1;
        protected int[] neighbors = Array.Empty<int>();

        public virtual TEdge Current => throw new NotImplementedException();
        object IEnumerator.Current => throw new NotImplementedException();

        public EdgeEnumeratorImp(List<TVertex?> vertices, IEnumerator<int> vEnum, int numEdges)
        {
            this.vertices = vertices;
            this.vEnum = vEnum;
            this.numEdges = numEdges;
        }

        public bool MoveNext()
        {
            if (idx == -1)
            {
                if (!vEnum.MoveNext())
                    return false;
                
                neighbors = vertices[vEnum.Current].Neighbors.ConvertAll(c => c.To).ToArray();
            }

            ++idx;

            while(idx >= neighbors.Length)
            {
                if (!vEnum.MoveNext())
                    return false;
                
                neighbors = vertices[vEnum.Current].Neighbors.ConvertAll(c => c.To).ToArray();
                idx = 0;
            }

            return true;
        }

        public void Reset()
        {
            vEnum.Reset();
            idx = -1;
            neighbors = Array.Empty<int>();
        }

        public void Dispose()
        {
            vEnum.Dispose();
        }
    }

    public class AdjacencyListImp<TVertex, TConnection>
        where TVertex : Vertex<TConnection>, new()
        where TConnection : Connection, new()
    {
        public IReadOnlyCollection<int> Vertices {get => new VertexSet(vertices, numVertices);}
        public IReadOnlyCollection<Edge<int>> Edges {get => new EdgeSet(vertices, Vertices.GetEnumerator(), numVertices);}

        public AdjacencyListImp() {}

        protected List<TVertex?> vertices = new();
        protected int numVertices = 0;
        protected int numEdges = 0;

        public int AddVertex()
        {
            int idx;

            vertices.Add(new());
            idx = vertices.Count - 1;

            ++numVertices;

            return idx;
        }

        public void RemoveVertex(int vertex)
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

        public bool ContainsVertex(int vertex)
        {
            return vertex >= 0 && vertex < vertices.Count && vertices[vertex] != null;
        }

        public void Connect(int from, int to)
        {
            TVertex vFrom = GetVertex(from);

            if (vFrom.TryGetConnection(to, out TConnection existingConnection))
            {
                vFrom.RemoveConnection(existingConnection.To);

                TConnection connection = new();
                connection.To = to;

                vFrom.Neighbors.Add(connection);
            }
            else
            {
                TConnection connection = new();
                connection.To = to;

                vFrom.Neighbors.Add(connection);
            }
            
            ++numEdges;
        }

        public void Disconnect(int from, int to)
        {
            if (!ContainsConnection(from, to))
                throw new Exception("Nonexistant edge");
            
            GetVertex(from).RemoveConnection(to);
            --numEdges;
        }

        public bool ContainsConnection(int from, int to)
        {
            if (!ContainsVertex(from) || !ContainsVertex(to))
                throw new Exception("Nonexistant vertex");
            
            return GetVertex(from).IsConnected(to);
        }

        public int[] GetNeighbors(int vertex)
        {
            if (!ContainsVertex(vertex))
                throw new Exception("Nonexistant vertex");
            
            TVertex v = GetVertex(vertex);
            
            int[] neighbors = new int[v.Neighbors.Count];

            for(int i = 0; i < v.Neighbors.Count; ++i)
                neighbors [i] = v.Neighbors[i].To;

            return neighbors;
        }

        public void ClearEdges()
        {
            foreach (TVertex v in vertices)
                v.Neighbors.Clear();
            
            numEdges = 0;
        }

        public void Clear()
        {
            vertices.Clear();

            numVertices = 0;
            numEdges = 0;
        }

        public class VertexSet : IReadOnlyCollection<int>
        {
            public int Count => numVertices;

            private int numVertices;
            private List<TVertex?> vertices;

            public VertexSet(List<TVertex?> vertices, int numVertices)
            {
                this.vertices = vertices;
                this.numVertices = numVertices;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return new VertexEnumerator(vertices, numVertices);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class VertexEnumerator : IEnumerator<int>
        {
            public int Current => idx;
            object IEnumerator.Current => idx;

            private int numVertices;
            private List<TVertex> vertices;
            private int idx = -1;

            public VertexEnumerator(List<TVertex> vertices, int numVertices)
            {
                this.vertices = vertices;
                this.numVertices = numVertices;
            }

            public bool MoveNext()
            {
                while(++idx < vertices.Count && vertices[idx] == null) {}

                return idx < vertices.Count;
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

        public class EdgeSet : IReadOnlyCollection<Edge<int>>
        {
            public virtual int Count => numEdges;

            protected readonly int numEdges;
            protected IEnumerator<int> vEnum;
            protected List<TVertex?> vertices;

            public EdgeSet(List<TVertex?> vertices, IEnumerator<int> vEnum, int numEdges)
            {
                this.vertices = vertices;
                this.vEnum = vEnum;
                this.numEdges = numEdges;
            }

            public IEnumerator<Edge<int>> GetEnumerator()
            {
                return new EdgeEnumerator(vertices, vEnum, numEdges);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class EdgeEnumerator : EdgeEnumeratorImp<TVertex, TConnection, Edge<int>>
        {
            public override Edge<int> Current => new Edge<int>(
                vEnum.Current,
                neighbors[idx]
            );

            public EdgeEnumerator(List<TVertex?> vertices, IEnumerator<int> vEnum, int numEdges) : base(vertices, vEnum, numEdges){
                Console.WriteLine(1);
            }
        }

        private void NewVertex(int idx)
        {
            if (idx < vertices.Count - 1)
                vertices[idx] = new();
            else
            {
                while(vertices.Count - 1 < idx)
                    vertices.Add(new());

                vertices[vertices.Count - 1] = new();
            }

            ++numVertices;
        }

        protected TVertex GetVertex(int idx)
        {
            if (!ContainsVertex(idx))
                throw new Exception("Nonexistant vertex");
            
            return vertices[idx];
        }

        protected TConnection GetConnection(int from, int to)
        {
            if (!ContainsConnection(from, to))
                throw new Exception("Nonexistant connection");
            
            return vertices[from].Neighbors.Find(c => c.To == to);
        }
    }
}

namespace Graph
{
    public class AdjacencyList : AdjacencyListImp<Vertex<Connection>, Connection>
    {

    }

    public class AdjacencyList<V> : AdjacencyListImp<Vertex<V, Connection>, Connection>
    {

    }

    public class WeightedAdjacencyList<E> : AdjacencyListImp<Vertex<Connection<E>>, Connection<E>>, IWeightedGraphRepresentation<E>
    {
        new public IReadOnlyCollection<Edge<int, E>> Edges => new EdgeSet(vertices, Vertices.GetEnumerator(), numEdges);

        public void Connect(int from, int to, E data)
        {
            base.Connect(from, to);
            GetConnection(from, to).Data = data;
        }

        public void SetEdgeData(int from, int to, E data)
        {
            if (!ContainsVertex(from) || !ContainsVertex(to))
                throw new Exception("Nonexistant vertex");
            
            GetConnection(from, to).Data = data;
        }

        public E GetEdgeData(int from, int to)
        {
            if (!ContainsVertex(from) || !ContainsVertex(to))
                throw new Exception("Nonexistant vertex");
            
            return GetConnection(from, to).Data;
        }

        new public class EdgeSet : IReadOnlyCollection<Edge<int, E>>
        {
            public virtual int Count => numEdges;

            protected readonly int numEdges;
            protected IEnumerator<int> vEnum;
            protected List<Vertex<Connection<E>>?> vertices;

            public EdgeSet(List<Vertex<Connection<E>>?> vertices, IEnumerator<int> vEnum, int numEdges)
            {
                this.vertices = vertices;
                this.vEnum = vEnum;
                this.numEdges = numEdges;
            }

            public IEnumerator<Edge<int, E>> GetEnumerator()
            {
                return new EdgeEnumerator(vertices, vEnum, numEdges);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        new public class EdgeEnumerator : EdgeEnumeratorImp<Vertex<Connection<E>>, Connection<E>, Edge<int, E>>
        {
            public override Edge<int, E> Current => new(
                vEnum.Current,
                neighbors[idx],
                vertices[vEnum.Current].GetConnection(neighbors[idx]).Data
            );
            
            public EdgeEnumerator(List<Vertex<Connection<E>>?> vertices, IEnumerator<int> vEnum, int numEdges) : base(vertices, vEnum, numEdges) {

                Console.WriteLine(2);
            }
        }
    }

    public class WeightedAdjacencyList : WeightedAdjacencyList<int> {}
}