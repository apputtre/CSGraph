namespace MyGraph
{
    public class AdjacencyList<TVertexData, TEdgeData> : GraphRepresentation<TVertexData, TEdgeData>
    {
        private List<Vertex?> vertices = new();
        private int nextIndex = 0;

        public override int[] Vertices
        {
            get
            {
                int[] indices = new int[vertices.Count];

                for (int i = 0; i < vertices.Count; ++i)
                    if (vertices[i] is not null)
                        indices[i] = vertices[i].Index;
                
                return indices;
            }
        }

        public override bool ContainsVertex(int index)
        {
            if (index < nextIndex)
                return vertices[index] is not null;
            
            return false;
        }

        public override int AddVertex(TVertexData? data)
        {
            vertices.Add(new Vertex(nextIndex, data, new List<Edge>()));
            return nextIndex++;
        }

        public override void AddEdge(int index1, int index2, TEdgeData? data)
        {
            try
            {
                vertices[index1].Connections.Add(new Edge(vertices[index2], Data: data));
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }


        public override TEdgeData? GetEdgeData(int index1, int index2)
        {
            try
            {
                foreach (Edge edge in vertices[index1].Connections)
                    if (edge.To.Index == index2)
                        return edge.Data;
                
                throw new ArgumentException();
            }
            catch(NullReferenceException)
            {
                throw new ArgumentException();
            }
        }

        public override TVertexData GetVertexData(int index)
        {
            try
            {
                return vertices[index].Data;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override void RemoveEdge(int index1, int index2)
        {
            try
            {
                foreach (Edge edge in vertices[index1].Connections)
                    if (edge.To.Index == index2)
                    {
                        vertices[index1].Connections.Remove(edge);
                        return;
                    }
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException();
            }
        }

        public override void RemoveVertex(int index)
        {
            try
            {
                foreach (Edge edge in vertices[index].Connections)
                    RemoveEdge(index, edge.To.Index);
                
                vertices[index] = null;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetEdgeData(int index1, int index2, TEdgeData data)
        {
            try
            {
                foreach (Edge edge in vertices[index1].Connections)
                    if (edge.To.Index == index2)
                        edge.Data = data;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException();
            }
        }

        public override void SetVertexData(int index, TVertexData data)
        {
            try
            {
                vertices[index].Data = data;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public override int[] GetConnections(int index)
        {
            try
            {
                int[] connections = new int[vertices[index].Connections.Count];

                for (int i = 0; i < connections.Length; ++i)
                    connections[i] = vertices[index].Connections[i].To.Index;
                
                return connections;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private record Edge(Vertex To, TEdgeData? Data)
        {
            public Vertex To {get; set;} = To;
            public TEdgeData? Data {get; set;} = Data;
        }

        private record Vertex(int Index, TVertexData? Data, List<Edge> Connections)
        {
            public TVertexData? Data {get; set;} = Data;
        }
    }
}