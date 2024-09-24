namespace MyGraph
{
    public class AdjacencyMatrix<TVertexData, TEdgeData> : GraphRepresentation<TVertexData, TEdgeData>
    {
        public AdjacencyMatrix()
        {
            throw new NotImplementedException();
        }

        public override int[] Vertices => throw new NotImplementedException();

        public override void AddEdge(int index1, int index2, TEdgeData? data)
        {
            throw new NotImplementedException();
        }

        public override int AddVertex(TVertexData? data)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsVertex(int index)
        {
            throw new NotImplementedException();
        }

        public override int[] GetConnections(int index)
        {
            throw new NotImplementedException();
        }

        public override TEdgeData GetEdgeData(int index1, int index2)
        {
            throw new NotImplementedException();
        }

        public override TVertexData GetVertexData(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveEdge(int index1, int index2)
        {
            throw new NotImplementedException();
        }

        public override void RemoveVertex(int index)
        {
            throw new NotImplementedException();
        }

        public override void SetEdgeData(int index1, int index2, TEdgeData data)
        {
            throw new NotImplementedException();
        }

        public override void SetVertexData(int index, TVertexData data)
        {
            throw new NotImplementedException();
        }
    }
}