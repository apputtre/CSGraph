#if false

public class WeightedGraph<E> : IWeightedGraph<E>
    {
        public override IReadOnlyCollection<int> Vertices => new VertexSet(adj);
        public override IReadOnlyCollection<Edge<int, E>> Edges => new EdgeSet(adj);

        private WeightedAdjacencyList<E> adj;

        public WeightedGraph(E defaultEdgeDataVal = default) : base(defaultEdgeDataVal)
        {
            adj = new WeightedAdjacencyList<E>();
        }

        public WeightedGraph(WeightedGraph<E> other) : this(other.defaultEdgeDataVal)
        {
            throw new NotImplementedException();
        }

        public override int AddVertex()
        {
            return adj.AddVertex();
        }

        public override void RemoveVertex(int vertex)
        {
            adj.RemoveVertex(vertex);
        }

        public override bool ContainsVertex(int vertex)
        {
            return adj.ContainsVertex(vertex);
        }

        public override void AddEdge(int from, int to, E data)
        {
            if (!adj.ContainsVertex(from) || !adj.ContainsVertex(to))
                throw new Exception("Nonexistant vertex");
            
            /*
            Ensure that from_idx is less than to_idx.
            Enforcing that "real" edges must have from < to
            allows us to distinguish them from "false" edges
            when enumerating them.
            */
            if (from > to)
                (to, from) = (from, to);

            // add the "real" edge
            adj.Connect(from, to, data);
            // add the "false" edge
            adj.Connect(to, from, data);
        }

        public override void RemoveEdge(int from, int to)
        {
            if (!adj.ContainsVertex(from) || !adj.ContainsVertex(to))
                throw new Exception("Nonexistant vertex");

            adj.Disconnect(from, to);
            adj.Disconnect(to, from);
        }

        public override bool ContainsEdge(int from, int to)
        {
            if (!adj.ContainsVertex(from) || !adj.ContainsVertex(to))
                throw new Exception("Nonexistant vertex");

            return adj.ContainsConnection(from, to);
        }

        public override void SetEdgeData(int from, int to, E data)
        {
            if (!adj.ContainsVertex(from) || !adj.ContainsVertex(to))
                throw new Exception("Nonexistant vertex");

            adj.SetEdgeData(from, to, data);
            adj.SetEdgeData(to, from, data);
        }

        public override E GetEdgeData(int from, int to)
        {
            if (!adj.ContainsVertex(from) || !adj.ContainsVertex(to))
                throw new Exception("Nonexistant vertex");

            return adj.GetEdgeData(from, to);
        }

        public override Edge<int, E>[] GetEdges(int vertex)
        {
            int[] neighbors = adj.GetConnected(vertex);

            Edge<int, E>[] edges = new Edge<int, E>[neighbors.Length];

            for (int i = 0; i < neighbors.Length; ++i)
                edges[i] = new Edge<int, E>(vertex, neighbors[i], adj.GetEdgeData(vertex, neighbors[i]));
            
            return edges;
        }

        public override int[] GetNeighbors(int vertex)
        {
            int[] neighborIndices = adj.GetConnected(vertex);

            int[] neighbors = new int[neighborIndices.Length];

            for (int i = 0; i < neighborIndices.Length; ++i)
                neighbors[i] = neighborIndices[i];
            
            return neighbors;
        }

        public void ClearEdges()
        {
            adj.ClearEdges();
        }

        public void Clear()
        {
            adj.Clear();
        }

        public class VertexSet : IReadOnlyCollection<int>
        {
            public int Count => adj.Vertices.Count;

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
                return (IEnumerator) GetEnumerator();
            }
        }

        public class VertexEnumerator : IEnumerator<int>
        {
            public int Current => set.Current;

            private WeightedAdjacencyList<E> adj;
            private IEnumerator<int> set;

            object IEnumerator.Current => Current;

            public VertexEnumerator(WeightedAdjacencyList<E> adj)
            {
                this.adj = adj;
                this.set = adj.Vertices.GetEnumerator();
            }

            public bool MoveNext()
            {
                return set.MoveNext();
            }

            public void Reset()
            {
                set.Reset();
            }

            public void Dispose()
            {
                set.Dispose();
            }
        }

        public class EdgeSet : IReadOnlyCollection<Edge<int, E>>
        {
            public int Count => adj.Edges.Count / 2;

            private WeightedAdjacencyList<E> adj;

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
                return (IEnumerator) GetEnumerator();
            }
        }

        public class EdgeEnumerator : IEnumerator<Edge<int, E>>
        {
            public Edge<int, E> Current => new Edge<int, E>(
                set.Current.From,
                set.Current.To,
                set.Current.Data
            );

            object IEnumerator.Current => Current;

            private WeightedAdjacencyList<E> adj;
            private IEnumerator<Edge<int, E>> set;

            public EdgeEnumerator(WeightedAdjacencyList<E> adj)
            {
                this.adj = adj;
                set = adj.Edges.GetEnumerator();
            }

            public bool MoveNext()
            {
                if (!set.MoveNext())
                    return false;
                
                Edge<int, E> currentEdge = set.Current;

                while(currentEdge.To < currentEdge.From)
                {
                    if (!set.MoveNext())
                        return false;
                    
                    currentEdge = set.Current;
                }

                return true;
            }

            public void Reset()
            {
                set.Reset();
            }

            public void Dispose()
            {
                set.Dispose();
            }
        }
    }

    /*
    A weighted, undirected graph with each vertex having a unique
    label of type 'V' and each edge having associated data of type 'E'.
    */
    public class WeightedGraph<E, V> : IWeightedGraph<E, V>
    {
        public override IReadOnlyCollection<V> Vertices => new VertexSet(adj);
        public override IReadOnlyCollection<Edge<V, E>> Edges => new EdgeSet(adj);

        private IWeightedGraphRepresentation<E, V> adj;

        public WeightedGraph(E defaultEdgeDataVal = default) : base(defaultEdgeDataVal)
        {
            adj = new WeightedAdjacencyList<E, V>();
        }

        public WeightedGraph(WeightedGraph<E, V> other) : this(other.defaultEdgeDataVal)
        {
            foreach (V vertex in other.Vertices)
            {
                Edge<V, E>[] edges = other.GetEdges(vertex);

                AddVertex(vertex);

                foreach (Edge<V, E> e in edges)
                {
                    AddEdge(vertex, e.To, e.Data);
                }
            }
        }

        public override void AddVertex(V vertex)
        {
            adj.AddVertex(vertex);
        }

        public override void RemoveVertex(V vertex)
        {
            int idx = adj.GetIndex(vertex);
            adj.RemoveVertex(idx);
        }

        public override bool ContainsVertex(V vertex)
        {
            return adj.TryGetIndex(vertex, out int idx);
        }

        public override void AddEdge(V from, V to, E data = default)
        {
            int from_idx;
            int to_idx;

            if (!adj.TryGetIndex(from, out from_idx) || !adj.TryGetIndex(to, out to_idx))
                throw new Exception("Nonexistant vertex");
            
            /*
            Ensure that from_idx is less than to_idx.
            Enforcing that "real" edges must have from < to
            allows us to distinguish them from "false" edges
            when enumerating them.
            */
            if (from_idx > to_idx)
                (to_idx, from_idx) = (from_idx, to_idx);

            // add the "real" edge
            adj.Connect(from_idx, to_idx, data);
            // add the "false" edge
            adj.Connect(to_idx, from_idx, data);
        }

        public override void RemoveEdge(V from, V to)
        {
            int from_idx;
            int to_idx;

            if (!adj.TryGetIndex(from, out from_idx) || !adj.TryGetIndex(to, out to_idx))
                throw new Exception("Nonexistant vertex");

            adj.Disconnect(from_idx, to_idx);
            adj.Disconnect(to_idx, from_idx);
        }

        public override bool ContainsEdge(V from, V to)
        {
            int from_idx;
            int to_idx;

            if (!adj.TryGetIndex(from, out from_idx) || !adj.TryGetIndex(to, out to_idx))
                throw new Exception("Nonexistant vertex");

            return adj.ContainsConnection(from_idx, to_idx);
        }

        public override void SetEdgeData(V from, V to, E data)
        {
            int from_idx;
            int to_idx;

            if (!adj.TryGetIndex(from, out from_idx) || !adj.TryGetIndex(to, out to_idx))
                throw new Exception("Nonexistant vertex");

            adj.SetEdgeData(from_idx, to_idx, data);
            adj.SetEdgeData(to_idx, from_idx, data);
        }

        public override E GetEdgeData(V from, V to)
        {
            int from_idx;
            int to_idx;

            if (!adj.TryGetIndex(from, out from_idx) || !adj.TryGetIndex(to, out to_idx))
                throw new Exception("Nonexistant vertex");

            return adj.GetEdgeData(from_idx, to_idx);
        }

        public override Edge<V, E>[] GetEdges(V vertex)
        {
            int idx = adj.GetIndex(vertex);
            
            int[] neighbors = adj.GetNeighbors(idx);

            Edge<V, E>[] edges = new Edge<V, E>[neighbors.Length];

            for (int i = 0; i < neighbors.Length; ++i)
                edges[i] = new Edge<V, E>(vertex, adj.GetVertexData(neighbors[i]), adj.GetEdgeData(idx, neighbors[i]));
            
            return edges;
        }

        public override V[] GetNeighbors(V vertex)
        {
            int idx = adj.GetIndex(vertex);
            
            int[] neighborIndices = adj.GetNeighbors(idx);

            V[] neighbors = new V[neighborIndices.Length];

            for (int i = 0; i < neighborIndices.Length; ++i)
                neighbors[i] = adj.GetVertexData(neighborIndices[i]);
            
            return neighbors;
        }

        public void ClearEdges()
        {
            adj.ClearEdges();
        }

        public void Clear()
        {
            adj.Clear();
        }

        public class VertexSet : IReadOnlyCollection<V>
        {
            public int Count => adj.Vertices.Count;

            private IWeightedGraphRepresentation<E, V> adj;

            public VertexSet(IWeightedGraphRepresentation<E, V> adj)
            {
                this.adj = adj;
            }

            public IEnumerator<V> GetEnumerator()
            {
                return new VertexEnumerator(adj);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator) GetEnumerator();
            }
        }

        public class VertexEnumerator : IEnumerator<V>
        {
            public V Current => adj.GetVertexData(set.Current);

            private IWeightedGraphRepresentation<E, V> adj;
            private IEnumerator<int> set;

            object IEnumerator.Current => Current;

            public VertexEnumerator(IWeightedGraphRepresentation<E, V> adj)
            {
                this.adj = adj;
                this.set = adj.Vertices.GetEnumerator();
            }

            public bool MoveNext()
            {
                return set.MoveNext();
            }

            public void Reset()
            {
                set.Reset();
            }

            public void Dispose()
            {
                set.Dispose();
            }
        }

        public class EdgeSet : IReadOnlyCollection<Edge<V, E>>
        {
            public int Count => adj.Edges.Count / 2;

            private IWeightedGraphRepresentation<E, V> adj;

            public EdgeSet(IWeightedGraphRepresentation<E, V> adj)
            {
                this.adj = adj;
            }

            public IEnumerator<Edge<V, E>> GetEnumerator()
            {
                return new EdgeEnumerator(adj);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class EdgeEnumerator : IEnumerator<Edge<V, E>>
        {
            public Edge<V, E> Current => new Edge<V, E>(
                adj.GetVertexData(set.Current.From),
                adj.GetVertexData(set.Current.To),
                set.Current.Data
            );

            object IEnumerator.Current => Current;

            private IWeightedGraphRepresentation<E, V> adj;
            private IEnumerator<Edge<int, E>> set;

            public EdgeEnumerator(IWeightedGraphRepresentation<E, V> adj)
            {
                this.adj = adj;
                set = adj.Edges.GetEnumerator();
            }

            public bool MoveNext()
            {
                if (!set.MoveNext())
                    return false;
                
                Edge<int, E> currentEdge = set.Current;

                while(currentEdge.To < currentEdge.From)
                {
                    if (!set.MoveNext())
                        return false;
                    
                    currentEdge = set.Current;
                }

                return true;
            }

            public void Reset()
            {
                set.Reset();
            }

            public void Dispose()
            {
                set.Dispose();
            }
        }
    }

    #endif