namespace Ds.Graphs.Dfs.Recursive
{
    using System;
    using System.Collections.Generic;
    using Repository;

    class DepthFirstSearch
    {
        public UnweightedGraph Graph { get; set; }

        public bool[] Visited { get; set; }

        private List<int> ResultVertices { get; set; }

        private List<Tuple<int, int>> ResultEdges { get; set; }

        public DepthFirstSearch(UnweightedGraph graph)
        {
            this.Graph = graph;
        }

        public Tuple<List<int>, List<Tuple<int, int>>> Run(int startVertex)
        {
            this.Visited = new bool[this.Graph.VertexCount];
            this.ResultVertices = new List<int>();
            this.ResultEdges = new List<Tuple<int, int>>();

            this._Run(startVertex, null);

            return new Tuple<List<int>, List<Tuple<int, int>>>(this.ResultVertices, this.ResultEdges);
        }

        private void _Run(int startVertex, int? parentVertex)
        {
            this.Visited[startVertex] = true;
            this.ResultVertices.Add(startVertex);

            if (parentVertex != null)
            {
                this.ResultEdges.Add(new Tuple<int, int>((int)parentVertex, startVertex));
            }

            foreach (var tailVertex in this.Graph.AdjacencyArray[startVertex])
            {
                if (!this.Visited[tailVertex])
                {
                    this._Run(tailVertex, startVertex);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var undirectedGraph = new UnweightedGraph(5);
            undirectedGraph.AddUndirectedEdge(0, 1);
            undirectedGraph.AddUndirectedEdge(0, 2);
            undirectedGraph.AddUndirectedEdge(0, 3);
            undirectedGraph.AddUndirectedEdge(1, 2);
            undirectedGraph.AddUndirectedEdge(1, 4);
            undirectedGraph.AddUndirectedEdge(2, 3);
            undirectedGraph.AddUndirectedEdge(2, 4);
            undirectedGraph.AddUndirectedEdge(3, 4);
            var dfs = new DepthFirstSearch(undirectedGraph);
            var result = dfs.Run(0);
            foreach (var node in result.Item1)
            {
                Console.Write(node + " ");
            }
            Console.WriteLine();
            foreach (var node in result.Item2)
            {
                Console.Write("(" + node.Item1 + "," + node.Item2 + ") ");
            }

            Console.WriteLine();
            Console.WriteLine();

            var directedGraph = new UnweightedGraph(11);
            directedGraph.AddDirectedEdge(0, 1);
            directedGraph.AddDirectedEdge(0, 2);
            directedGraph.AddDirectedEdge(0, 3);
            directedGraph.AddDirectedEdge(0, 4);
            directedGraph.AddDirectedEdge(0, 5);
            directedGraph.AddDirectedEdge(1, 6);
            directedGraph.AddDirectedEdge(2, 6);
            directedGraph.AddDirectedEdge(4, 8);
            directedGraph.AddDirectedEdge(5, 9);
            directedGraph.AddDirectedEdge(5, 10);
            directedGraph.AddDirectedEdge(6, 7);
            dfs = new DepthFirstSearch(directedGraph);
            result = dfs.Run(0);
            foreach (var node in result.Item1)
            {
                Console.Write(node + " ");
            }
            Console.WriteLine();
            foreach (var node in result.Item2)
            {
                Console.Write("(" + node.Item1 + "," + node.Item2 + ") ");
            }

            Console.ReadKey();
        }
    }
}
