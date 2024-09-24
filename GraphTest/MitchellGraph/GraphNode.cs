/*

Part of implementation of the graph data structure described by
Scott Mitchell, "An Extensive Examination of Data Structures Using C# 2.0,"
Accessible at https://learn.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN#datastructures20_5_topic3

*/

namespace MitchellGraph
{
    public class GraphNode<T> : Node<T>
    {
        private List<int> costs;

        public GraphNode() : base() { }
        public GraphNode(T value) : base(value) { }
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>();
                
                return base.Neighbors;
            }
        }

        public List<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();
                
                return costs;
            }
        }

        public int GetCost(GraphNode<T> neighbor)
        {
            return Costs[Neighbors.IndexOf(neighbor)];
        }
    }
}