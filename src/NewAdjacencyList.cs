using System.Collections.Generic;
using System.Collections;
using CSGraph.Implementation;

namespace CSGraph.Implementation
{
    public interface IVertex<TConnection>
    {
        public List<TConnection> Connections {get;}
    }

    public readonly struct Vertex<TConnection> : IVertex<TConnection>
    {
        public List<TConnection> Connections {get;}

        public Vertex()
        {
            Connections = new();
        }
    }

    public readonly struct Vertex<VData, TConnection> : IVertex<TConnection>
    {
        public List<TConnection> Connections {get;}
        public VData Data {get;}

        public Vertex(VData data)
        {
            Data = data;
            Connections = new();
        }
    }

    public interface IConnection
    {
        public int To {get;}
    }

    public readonly struct Connection : IConnection
    {
        public int To {get;}

        public Connection(int to)
        {
            To = to;
        }
    }

    public readonly struct Connection<EData> : IConnection
    {
        public int To{get;}

        public EData Data {get;}

        public Connection(int to, EData data)
        {
            To = to;
            Data = data;
        }
    }

/*
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
    */

    public class AdjacencyList<TVertex, TConnection>
        where TVertex : struct, IVertex<TConnection>
        where TConnection : struct, IConnection
    {
        public List<TVertex?> Vertices {get;}

        public AdjacencyList()
        {
            Vertices = new();
        }

    /*
        public class VertexSet : IEnumerator<int>
        {
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
        */

        public void Trim()
        {
            List<int> nullIndices = new();

            for (int vertex = 0; vertex < Vertices.Count; ++vertex)
            {
                if (Vertices[vertex] == null)
                {
                    nullIndices.Add(vertex);
                    continue;
                }

                TVertex v = Vertices[vertex].GetValueOrDefault();

                for (int c = 0; c < v.Connections.Count; ++c)
                {
                    if (Vertices[v.Connections[c].To] == null)
                    {
                        v.Connections[c] = v.Connections[^1];
                        v.Connections.RemoveAt(v.Connections.Count - 1);
                    }
                }
            }

            for (int j = 0; j < nullIndices.Count; ++j)
            {
                Vertices[j] = Vertices[^1];
                Vertices.RemoveAt(Vertices.Count - 1);
            }
        }
    }
}

#if false

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

#endif