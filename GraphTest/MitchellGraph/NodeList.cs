/*

Part of implementation of the graph data structure described by
Scott Mitchell, "An Extensive Examination of Data Structures Using C# 2.0,"
Accessible at https://learn.microsoft.com/en-us/previous-versions/ms379574(v=vs.80)?redirectedfrom=MSDN#datastructures20_5_topic3

*/

using System.Collections.ObjectModel;

namespace MitchellGraph
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() {}

        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; ++i)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value)
        {
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;
            
            return null;
        }
    }
}