
namespace Graph
{
    public class DirectedGraph<TVertexData, TEdgeData> : Graph<TVertexData, TEdgeData>
    {
        public DirectedGraph(bool sparse = true)
        {
            if (sparse)
                rep = new AdjacencyList<TVertexData, TEdgeData>();
            else
                rep = new AdjacencyMatrix<TVertexData, TEdgeData>();
        }

        public override bool ContainsVertex(Vertex<TVertexData, TEdgeData> vertex)
        {
            return vertex.GetParentGraph() == this && rep.ContainsVertex(vertex.Index);
        }

        public override Vertex<TVertexData, TEdgeData> AddVertex(TVertexData? data)
        {
            int idx = rep.AddVertex(data);
            Vertex<TVertexData, TEdgeData> vertex = new(this, idx, data);
            return vertex;
        }

        public override void AddEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data)
        {
            rep.AddEdge(from.Index, to.Index, data);
        }

        public override void RemoveEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            rep.RemoveEdge(from.Index, to.Index);
        }

        public override TEdgeData? GetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            return rep.GetEdgeData(from.Index, to.Index);
        }

        public override void SetEdgeData(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to, TEdgeData? data)
        {
            rep.SetEdgeData(from.Index, to.Index, data);
        }

        public override Vertex<TVertexData, TEdgeData>[] GetNeighbors(Vertex<TVertexData, TEdgeData> vertex)
        {
            int[] connections = rep.GetConnections(vertex.Index);

            Vertex<TVertexData, TEdgeData>[] neighbors = new Vertex<TVertexData, TEdgeData>[connections.Length];

            for(int i = 0; i < connections.Length; ++i)
                neighbors[i] = new Vertex<TVertexData, TEdgeData>(this, connections[i], rep.GetVertexData(i));

            return neighbors;
        }

        public override void RemoveVertex(Vertex<TVertexData, TEdgeData> toRemove)
        {
            rep.RemoveVertex(toRemove.Index);
        }

        public override TVertexData? GetVertexData(Vertex<TVertexData, TEdgeData> vertex)
        {
            return rep.GetVertexData(vertex.Index);
        }

        public override void SetVertexData(Vertex<TVertexData, TEdgeData> vertex, TVertexData? data)
        {
            rep.SetVertexData(vertex.Index, data);
        }

        public override Vertex<TVertexData, TEdgeData> AddVertex(Vertex<TVertexData, TEdgeData> other)
        {
            throw new System.NotImplementedException();
        }

        public override Vertex<TVertexData, TEdgeData>[] GetVertices(TVertexData data)
        {
            throw new System.NotImplementedException();
        }

        public override bool ContainsEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            throw new System.NotImplementedException();
        }

        public override Edge<TVertexData, TEdgeData> GetEdge(Vertex<TVertexData, TEdgeData> from, Vertex<TVertexData, TEdgeData> to)
        {
            throw new System.NotImplementedException();
        }
    }
}