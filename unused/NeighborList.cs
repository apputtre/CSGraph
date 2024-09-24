using System.Collections.ObjectModel;

namespace MyGraph
{
    public class NeighborList : Collection<Vertex>
    {
        Vertex parent;

        public NeighborList(Vertex parent)
        {
            this.parent = parent;
        }

        public int GetWeight(Vertex neighbor)
        {
            return parent.GetWeight(neighbor);
        }

        public int this[Vertex neighbor]
        {
            get => parent.GetWeight(neighbor);
            set => parent.SetWeight(neighbor, value);
        }
    }
}