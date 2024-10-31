using MitchellGraph;
using Graph;

using V = Graph.Vertex<string, int>;

class GraphTest
{
    static void Main(string[] args)
    {
        // initialize Scott Mitchell's graph
        /*
        Graph<string> graph = new();

        graph.AddNode("New York");
        graph.AddNode("Chicago");
        graph.AddNode("Miami");
        graph.AddNode("Dallas");
        graph.AddNode("Denver");
        graph.AddNode("San Francisco");
        graph.AddNode("San Diego");
        graph.AddNode("L.A.");

        graph.AddDirectedEdge(graph.GetNode("New York"), graph.GetNode("Miami"), 90);
        graph.AddDirectedEdge(graph.GetNode("New York"), graph.GetNode("Dallas"), 125);
        graph.AddDirectedEdge(graph.GetNode("New York"), graph.GetNode("Denver"), 100);
        graph.AddDirectedEdge(graph.GetNode("New York"), graph.GetNode("Chicago"), 75);
        graph.AddDirectedEdge(graph.GetNode("Chicago"), graph.GetNode("San Francisco"), 25);
        graph.AddDirectedEdge(graph.GetNode("Chicago"), graph.GetNode("Denver"), 20);
        graph.AddDirectedEdge(graph.GetNode("Denver"), graph.GetNode("San Francisco"), 75);
        graph.AddDirectedEdge(graph.GetNode("Denver"), graph.GetNode("L.A."), 100);
        graph.AddDirectedEdge(graph.GetNode("Dallas"), graph.GetNode("L.A."), 80);
        graph.AddDirectedEdge(graph.GetNode("Dallas"), graph.GetNode("San Diego"), 90);
        graph.AddDirectedEdge(graph.GetNode("Miami"), graph.GetNode("Dallas"), 50);
        graph.AddDirectedEdge(graph.GetNode("San Diego"), graph.GetNode("L.A."), 45);
        graph.AddDirectedEdge(graph.GetNode("San Francisco"), graph.GetNode("L.A."), 45);

        Console.WriteLine("Graph:");

        PrintGraph<string>(graph);
        */




        // run Dijkstra's algorithm on Mitchell's graph
        /*
        Dictionary<MitchellGraph.GraphNode<string>, int> costs = new();
        Dictionary<MitchellGraph.GraphNode<string>, MitchellGraph.GraphNode<string>?> routes = new();
        MitchellGraph.Algorithms.Dijkstra<string>(graph, graph.GetNode("New York"), graph.GetNode("L.A."), out costs, out routes);

        Console.WriteLine();

        Console.WriteLine("Costs table:");
        foreach (KeyValuePair<GraphNode<string>, int> pair in costs)
            Console.WriteLine("{0}: {1}", pair.Key.Value, pair.Value);

        Console.WriteLine();

        Console.WriteLine("Routes table:");
        foreach (KeyValuePair<GraphNode<string>, GraphNode<string>?> pair in routes)
            Console.WriteLine("{0}: {1}", pair.Key.Value, pair.Value == null ? "null" : pair.Value.Value);

        List<GraphNode<string>> min_cost_path = new();

        min_cost_path.Add(graph.GetNode("L.A."));
        GraphNode<string>? next = routes[graph.GetNode("L.A.")];
        while (next != null)
        {
            min_cost_path.Add(next);
            next = routes[next];
        }

        min_cost_path.Reverse();

        Console.WriteLine();
        Console.WriteLine("Min cost path:");
        foreach (GraphNode<string> node in min_cost_path)
            Console.WriteLine(node.Value);
        */

        // initialize my graph
        Graph.DirectedGraph<string, int> graph = new();

        Graph.Vertex<String, int> v_ny = graph.AddVertex("New York");
        V v_chicago = graph.AddVertex("Chicago");
        var v_miami = graph.AddVertex("Miami");
        var v_dallas = graph.AddVertex("Dallas");
        var v_denver = graph.AddVertex("Denver");
        var v_sf = graph.AddVertex("San Francisco");
        var v_sd = graph.AddVertex("San Diego");
        var v_la = graph.AddVertex("L.A.");

        graph.AddEdge(v_ny, v_dallas, 125);
        graph.AddEdge(v_ny, v_miami, 90);
        graph.AddEdge(v_ny, v_denver, 100);
        graph.AddEdge(v_ny, v_chicago, 75); // new york -> chicago
        graph.AddEdge(v_chicago, v_sf, 25);
        graph.AddEdge(v_chicago, v_denver, 20);
        graph.AddEdge(v_denver, v_sf, 75);
        graph.AddEdge(v_denver, v_la, 100);
        graph.AddEdge(v_dallas, v_la, 80);
        graph.AddEdge(v_dallas, v_sd, 90);
        graph.AddEdge(v_miami, v_dallas, 50);
        graph.AddEdge(v_sd, v_la, 45);
        graph.AddEdge(v_sf, v_la, 45);

        Console.WriteLine("Graph:");

        Graph.Algorithms.PrintGraph(graph);

        // run Dijkstra's algorithm on my graph
        Dictionary<Graph.Vertex<string, int>, int> costs = new();
        Dictionary<Graph.Vertex<string, int>, Graph.Vertex<string, int>?> routes = new();
        Graph.Algorithms.Dijkstra(graph, v_ny, v_la, out costs, out routes);

        Console.WriteLine();

        Console.WriteLine("Costs table:");
        foreach (KeyValuePair<Graph.Vertex<string, int>, int> pair in costs)
            Console.WriteLine("{0}: {1}", pair.Key.Data, pair.Value);

        Console.WriteLine();

        Console.WriteLine("Routes table:");
        foreach (KeyValuePair<Graph.Vertex<string, int>, Graph.Vertex<string, int>?> pair in routes)
            Console.WriteLine("{0}: {1}", pair.Key.Data, pair.Value == null ? "null" : pair.Value.Data);

        List<Graph.Vertex<string, int>> min_cost_path = new();

        Graph.Vertex<string, int>? next = v_la;
        while (next != null)
        {
            min_cost_path.Add(next);
            next = routes[next];
        }

        min_cost_path.Reverse();

        Console.WriteLine();
        Console.WriteLine("Min cost path:");
        foreach (Graph.Vertex<string, int> v in min_cost_path)
            Console.WriteLine(v.Data);
    }
    
}