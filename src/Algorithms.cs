#if false

using System.Numerics;

namespace MitchellGraph
{
    public class Algorithms
    {
        private static void Dijkstra<T>(
            Graph<T> g,
            GraphNode<T> from,
            GraphNode<T> to,
            out Dictionary<GraphNode<T>, int> costs,
            out Dictionary<GraphNode<T>, MitchellGraph.GraphNode<T>?> routes)
        {
            costs = new();
            routes = new();

            // create list of nodes to visit
            List<GraphNode<T>> to_visit = new();

            // initialize costs, routes, and to_visit
            foreach (GraphNode<T> node in g)
            {
                if (node == from)
                    costs[node] = 0;
                else
                    costs[node] = int.MaxValue;

                routes[node] = null;

                to_visit.Add(node);
            }

            while (to_visit.Count > 0)
            {
                // find the node in to_visit which has the least cost path from the start node
                GraphNode<T> next = new();
                {
                    int min_cost = int.MaxValue;
                    foreach (GraphNode<T> node in to_visit)
                        if (costs[node] < min_cost)
                        {
                            min_cost = costs[node];
                            next = node;
                        }
                }

                foreach (GraphNode<T> neighbor in next.Neighbors)
                {
                    // if the path to neighbor through this node is less than that of the existing path
                    int cost = costs[next] + next.GetCost(neighbor);
                    if (cost < costs[neighbor])
                    {
                        // update the costs and routes tables
                        costs[neighbor] = cost;
                        routes[neighbor] = next;
                    }
                }

                to_visit.Remove(next);
            }

            return;
        }

        private static void PrintGraph<T>(Graph<T> g)
        {
            Console.WriteLine("{0} Nodes", g.Count);

            foreach (GraphNode<T> node in g)
            {
                Console.Write("{0}: {{ ", node.Value);
                foreach (GraphNode<T> neighbor in node.Neighbors)
                    Console.Write("({0} {1}), ", neighbor.Value, node.GetCost(neighbor));
                Console.Write("}}\n");
            }
        }
    }
}

namespace Graph
{
    public class Algorithms
    {
        public static void Dijkstra<TVertexData>
        (
            Graph<TVertexData, int> g,
            Vertex<TVertexData, int> from,
            Vertex<TVertexData, int> to,
            out Dictionary<Vertex<TVertexData, int>, int> costs,
            out Dictionary<Vertex<TVertexData, int>, Vertex<TVertexData, int>?> routes
        )
        {
            costs = new();
            routes = new();

            // create list of nodes to visit
            List<Vertex<TVertexData, int>> to_visit = new();

            // initialize costs, routes, and to_visit
            foreach (Vertex<TVertexData, int> v in g.Vertices)
            {
                if (v == from)
                    costs[v] = 0;
                else
                    costs[v] = int.MaxValue;

                routes[v] = null;

                to_visit.Add(v);
            }

            while (to_visit.Count > 0)
            {
                // find the node in to_visit which has the least cost path from the start node
                Vertex<TVertexData, int>? next = new();
                {
                    int min_cost = int.MaxValue;
                    foreach (Vertex<TVertexData, int> v in to_visit)
                        if (costs[v] < min_cost)
                        {
                            min_cost = costs[v];
                            next = v;
                        }
                }

                if (next == null)
                    throw new InvalidOperationException();

                foreach (Vertex<TVertexData, int> neighbor in next.Neighbors)
                {
                    // if the path to neighbor through this node is less than that of the existing path
                    int cost = costs[next] + g.GetEdge(next, neighbor).Data;
                    if (cost < costs[neighbor])
                    {
                        // update the costs and routes tables
                        costs[neighbor] = cost;
                        routes[neighbor] = next;
                    }
                }

                to_visit.Remove(next);
            }

            return;
        }

        public static void PrintGraph<TVertexData, TEdgeData>(Graph<TVertexData, TEdgeData> g)
        {
            Console.WriteLine("{0} Vertices", g.Vertices.Length);

            foreach (var v in g.Vertices)
            {
                Console.Write("{0}: {{ ", v.Data);
                foreach (var neighbor in v.Neighbors)
                    Console.Write("({0} {1}), ", neighbor.Data, g.GetEdge(v, neighbor).Data);
                Console.Write("}}\n");
            }
        }
    }
}

#endif