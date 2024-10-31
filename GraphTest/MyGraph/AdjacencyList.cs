using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Graph
{
    public class AdjacencyList<TVertex> : GraphRepresentation<TVertex>
        where TVertex : Vertex, new()
    {
        private List<Vertex?> vertices = new();
        private int nextIndex = 0;

        public override int[] Vertices
        {
            get
            {
                List<int> indices = new();

                for (int i = 0; i < vertices.Count; ++i)
                    if (vertices[i] is not null)
                        indices.Add(i);
                
                return indices.ToArray();
            }
        }

        public override bool ContainsVertex(int index)
        {
            if (index < nextIndex)
                return vertices[index] is not null;
            
            return false;
        }

        public override int AddVertex()
        {
            TVertex vertex = TVertex.Create()
            return nextIndex++;
        }

        public override void AddEdge(Edge e)
        {
            throw new NotImplementedException();
        }

        public override void AddEdge(int index1, int index2)
        {
            AddEdge(new Edge(index1, index2));
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