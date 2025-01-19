using System;
using CSGraph;
using System.Runtime.InteropServices;
using System.Diagnostics;

struct S1 {};
struct S2 {int x;}
ref struct S3 {}

public ref struct S {
	int x;
	int y;
	ref List<int> l;
}

public class C1 {}

public class C2 {int x;}

public class A
{
	public static int x = 0;

	public void Test()
	{
		++x;
	}
}

public abstract class B
{
	public static int x = 0;

	public void Test()
	{
		++x;
	}
}

public class BDerived : B
{
    new public void Test()
    {
        ++x;
    }
}

public static class Test
{
    public static void Main()
    {
		WeightedGraph<int, char> graph = new();

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

		WeightedGraph<int, char> mst = GraphAlgorithms.Prims(graph, 'a');

        Console.WriteLine("\nMST with 'a' as source vertex:");
		foreach (Edge<char, int> e in mst.Edges)
			Console.WriteLine($"{e.From} => {e.To}, {e.Data}");
		
		WeightedGraph w = new();
		int v1 = w.AddVertex();
		int v2 = w.AddVertex();
		w.AddEdge(v1, v2);
		Console.WriteLine(w.GetEdgeData(v1, v2));
		w.SetEdgeData(v1, v2, 10);
		Console.WriteLine(w.GetEdgeData(v1, v2));
		Console.WriteLine(w.GetEdgeData(v2, v1));
		Console.WriteLine(v1 + " " + v2);
	}
}