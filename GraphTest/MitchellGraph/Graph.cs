/*

Part of implementation of the graph data structure described by
Scott Mitchell, "An Extensive Examination of Data Structures Using C# 2.0,"
Accessible at https://learn.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN#datastructures20_5_topic3

*/

using System.Collections;           // must include this to implement IEnumerable

namespace MitchellGraph
{
    public class Graph<T> : IEnumerable<GraphNode<T>>
    {
        private NodeList<T> nodeSet;

        public Graph() : this(null) {}
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            nodeSet.Add(node);
        }

        public GraphNode<T> AddNode(T value)
        {
            GraphNode<T> node = new(value);
            nodeSet.Add(node);
            return node;
        }

        public GraphNode<T> GetNode(T value)
        {
            return (GraphNode<T>) nodeSet.FindByValue(value);
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            AddDirectedEdge(from, to, 1);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            AddUndirectedEdge(from, to, 1);
        }

        public int GetCost(GraphNode<T> from, GraphNode<T> to)
        {
            return from.GetCost(to);
        }

        public bool Contains(T value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            GraphNode<T> nodeToRemove = (GraphNode<T>) nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                return false;
            
            nodeSet.Remove(nodeToRemove);

            foreach (GraphNode<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public NodeList<T> Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        public int Count
        {
            get { return nodeSet.Count; }
        }

        IEnumerator<GraphNode<T>> IEnumerable<GraphNode<T>>.GetEnumerator()
        {
            return new GraphEnumerator(nodeSet);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)(this)).GetEnumerator();
        }

        public class GraphEnumerator : IEnumerator<GraphNode<T>>
        {
            private NodeList<T> list;
            int position = -1;

            public GraphEnumerator(NodeList<T> list)
            {
                this.list = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < list.Count);
            }

            public void Reset()
            {
                position = -1;
            }

            public object Current
            {
                get
                {
                    return Current;
                }
            }

            GraphNode<T> IEnumerator<GraphNode<T>>.Current
            {
                get
                {
                    try
                    {
                        return (GraphNode<T>) list[position];
                    }
                    catch(IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            private bool disposedValue = false;
            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                    }
                }

                this.disposedValue = true;
            }
        }
    }
}