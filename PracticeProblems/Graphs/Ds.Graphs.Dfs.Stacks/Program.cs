namespace Ds.Graphs.Dfs.Stacks
{
    using System;
    using System.Collections.Generic;
    using Repository;

    class DepthFirstSearch
    {
        public UnweightedGraph Graph { get; set; }

        public bool[] Visited { get; set; }

        private Stack<Tuple<int?, int>> DfsStack { get; set; }

        private List<int> ResultVertices { get; set; }

        private List<Tuple<int, int>> ResultEdges { get; set; }

        public DepthFirstSearch(UnweightedGraph graph)
        {
            this.Graph = graph;
        }

        public Tuple<List<int>, List<Tuple<int, int>>> Run(int startVertex)
        {
            this.Visited = new bool[this.Graph.VertexCount];
            this.DfsStack = new Stack<Tuple<int?, int>>();
            this.ResultVertices = new List<int>();
            this.ResultEdges = new List<Tuple<int, int>>();

            this.DfsStack.Push(new Tuple<int?, int>(null, startVertex));

            while (this.DfsStack.Count > 0)
            {
                var stackTop = this.DfsStack.Pop();

                if (!this.Visited[stackTop.Item2])
                {
                    this.Visited[stackTop.Item2] = true;
                    this.ResultVertices.Add(stackTop.Item2);

                    if (stackTop.Item1 != null)
                    {
                        this.ResultEdges.Add(new Tuple<int, int>((int)stackTop.Item1, stackTop.Item2));
                    }

                    foreach (var tailVertex in this.Graph.AdjacencyArray[stackTop.Item2])
                    {
                        if (!this.Visited[tailVertex])
                        {
                            this.DfsStack.Push(new Tuple<int?, int>(stackTop.Item2, tailVertex));
                        }
                    }
                }
            }

            return new Tuple<List<int>, List<Tuple<int, int>>>(this.ResultVertices, this.ResultEdges);
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
