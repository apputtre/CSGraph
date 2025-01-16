using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    public abstract class GraphRepresentation
    {
        public abstract IReadOnlyCollection<int> Vertices { get; }
        public abstract IReadOnlyCollection<Edge<int>> Edges { get; }

        public abstract int AddVertex();
        public abstract void RemoveVertex(int idx);
        public abstract void Connect(int v1, int v2);
        public abstract void Disconnect(int v1, int v2);
        public abstract bool ContainsVertex(int v);
        public abstract bool ContainsConnection(int v1, int v2);
        public abstract int[] GetNeighbors(int idx);
        public abstract void ClearEdges();
        public abstract void Clear();
    }

    public abstract class GraphRepresentation<V>
    {
        public abstract IReadOnlyCollection<int> Vertices { get; }
        public abstract IReadOnlyCollection<Edge<V>> Edges { get; }

        public abstract int AddVertex(V vData);
        public abstract void RemoveVertex(int idx);
        public abstract void Connect(int v1, int v2);
        public abstract void Disconnect(int v1, int v2);
        public abstract void SetVertexData(int idx, V vData);
        public abstract V GetVertexData(int idx);
        public abstract bool ContainsVertex(int v);
        public abstract bool ContainsConnection(int v1, int v2);
        public abstract bool TryGetIndex(V vData, out int idx);
        public abstract int GetIndex(V vData);
        public abstract int[] GetNeighbors(int idx);
        public abstract void ClearEdges();
        public abstract void Clear();
    }

    public abstract class WeightedGraphRepresentation<E>
    {
        public abstract IReadOnlyCollection<int> Vertices { get; }
        public abstract IReadOnlyCollection<Edge<int, E>> Edges { get; }

        public abstract int AddVertex();
        public abstract void RemoveVertex(int idx);
        public abstract void Connect(int v1, int v2, E eData);
        public abstract void Disconnect(int v1, int v2);
        public abstract void SetEdgeData(int v1, int v2, E eData);
        public abstract E GetEdgeData(int v1, int v2);
        public abstract bool ContainsVertex(int v);
        public abstract bool ContainsConnection(int v1, int v2);
        public abstract int[] GetNeighbors(int idx);
        public abstract void ClearEdges();
        public abstract void Clear();
    }

    public abstract class WeightedGraphRepresentation<E, V>
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
}