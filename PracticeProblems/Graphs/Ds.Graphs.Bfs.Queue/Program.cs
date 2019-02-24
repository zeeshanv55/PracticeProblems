namespace Ds.Graphs.Bfs.Queue
{
    using System;
    using System.Collections.Generic;

    class UnweightedGraph
    {
        public HashSet<int>[] AdjacencyArray { get; set; }

        public int VertexCount { get; set; }

        public UnweightedGraph(int vertexCount)
        {
            this.VertexCount = vertexCount;
            this.AdjacencyArray = new HashSet<int>[this.VertexCount];
            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new HashSet<int>();
            }
        }

        public void AddUndirectedEdge(int startVertex, int endVertex)
        {
            this.AddDirectedEdge(startVertex, endVertex);
            this.AddDirectedEdge(endVertex, startVertex);
        }

        public void AddDirectedEdge(int startVertex, int endVertex)
        {
            this.AdjacencyArray[startVertex].Add(endVertex);
        }
    }

    class BreadthFirstSearch
    {
        public UnweightedGraph Graph { get; set; }

        public bool[] Visited { get; set; }

        private Queue<Tuple<int?, int>> BfsQueue { get; set; }

        private List<int> ResultVertices { get; set; }

        private List<Tuple<int, int>> ResultEdges { get; set; }

        public BreadthFirstSearch(UnweightedGraph graph)
        {
            this.Graph = graph;
        }

        public Tuple<List<int>, List<Tuple<int, int>>> Run(int startVertex)
        {
            this.Visited = new bool[this.Graph.VertexCount];
            this.BfsQueue = new Queue<Tuple<int?, int>>();
            this.ResultVertices = new List<int>();
            this.ResultEdges = new List<Tuple<int, int>>();

            this.BfsQueue.Enqueue(new Tuple<int?, int>(null, startVertex));

            while (this.BfsQueue.Count > 0)
            {
                var queueEnd = this.BfsQueue.Dequeue();

                if (!this.Visited[queueEnd.Item2])
                {
                    this.Visited[queueEnd.Item2] = true;
                    this.ResultVertices.Add(queueEnd.Item2);

                    if (queueEnd.Item1 != null)
                    {
                        this.ResultEdges.Add(new Tuple<int, int>((int)queueEnd.Item1, queueEnd.Item2));
                    }

                    foreach (var tailVertex in this.Graph.AdjacencyArray[queueEnd.Item2])
                    {
                        if (!this.Visited[tailVertex])
                        {
                            this.BfsQueue.Enqueue(new Tuple<int?, int>(queueEnd.Item2, tailVertex));
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
            var bfs = new BreadthFirstSearch(undirectedGraph);
            var result = bfs.Run(0);
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
            bfs = new BreadthFirstSearch(directedGraph);
            result = bfs.Run(0);
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
