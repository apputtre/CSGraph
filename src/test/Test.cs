using System;
using Graph;

public static class Test
{
    public static void Main()
    {
        Graph<int, int> g = new();
        g.AddVertex(5);
        System.Console.Write(g.Vertices.Count);
        System.Console.Write("Hello world");
    }
}