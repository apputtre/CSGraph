using System;
using Graph;

public static class Test
{
    public static void Main()
    {
Graph<char, int> graph = new();

		graph.AddVertex('a');
		graph.AddVertex('b');
		graph.AddVertex('c');
		graph.AddVertex('d');
		graph.AddVertex('e');
		graph.AddVertex('f');
		graph.AddVertex('g');
		graph.AddVertex('h');
		graph.AddVertex('i');

		graph.AddEdge('a', 'b', 4);
		graph.AddEdge('a', 'h', 8);

		graph.AddEdge('b', 'c', 8);
		graph.AddEdge('b', 'h', 11);

		graph.AddEdge('c', 'd', 7);
		graph.AddEdge('c', 'f', 4);
		graph.AddEdge('c', 'i', 2);

		graph.AddEdge('d', 'e', 9);
		graph.AddEdge('d', 'f', 14);

		graph.AddEdge('e', 'f', 10);

		graph.AddEdge('f', 'g', 2);

		graph.AddEdge('g', 'h', 1);
		graph.AddEdge('g', 'i', 6);

		graph.AddEdge('h', 'i', 7);

        System.Console.WriteLine("Graph:");
		foreach (Edge<char, int> edge in graph.Edges)
			Console.WriteLine($"{edge.From} => {edge.To}, {edge.Data}");

		GraphAlgorithms.Dijkstra<char>(graph, 'a', out var costs, out var routes);

        System.Console.WriteLine("\nShortest path from 'a' to 'e':");
		List<char> path = GraphAlgorithms.ShortestPath<char>(routes, 'e');

		foreach (char c in path)
			Console.Write(c + ", ");
        Console.WriteLine($"\nCost of shortest path: {costs['e']}");

		Graph<char, int> mst = GraphAlgorithms.Prims(graph, 'a');

        Console.WriteLine("\nMST with 'a' as source vertex:");
		foreach (Edge<char, int> e in mst.Edges)
			Console.WriteLine($"{e.From} => {e.To}, {e.Data}");
    }
}