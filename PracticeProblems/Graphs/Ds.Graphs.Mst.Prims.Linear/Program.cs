namespace Ds.Graphs.Mst.Prims.Linear
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Repository;

    class PrimsMst
    {
        public WeightedGraph Graph { get; set; }

        public List<Tuple<int, int>> ResultEdges { get; set; }

        public PrimsMst(WeightedGraph graph)
        {
            this.Graph = graph;
        }

        public Tuple<double, List<Tuple<int, int>>> Run()
        {
            this.ResultEdges = new List<Tuple<int, int>>();
            var minWeight = (double)0;
            var exploredSet = new HashSet<int>();

            exploredSet.Add(0);

            while (exploredSet.Count < this.Graph.VertexCount)
            {
                var minEdgeWeight = double.MaxValue;
                var minEdgeHead = -1;
                var minEdgeTail = -1;

                foreach (var v in exploredSet)
                {
                    foreach (var edge in this.Graph.AdjacencyArray[v])
                    {
                        if (!exploredSet.Contains(edge.Key) && edge.Value < minEdgeWeight)
                        {
                            minEdgeWeight = edge.Value;
                            minEdgeHead = edge.Key;
                            minEdgeTail = v;
                        }
                    }
                }

                this.ResultEdges.Add(new Tuple<int, int>(minEdgeTail, minEdgeHead));
                minWeight += minEdgeWeight;
                exploredSet.Add(minEdgeHead);
            }

            return new Tuple<double, List<Tuple<int, int>>>(minWeight, this.ResultEdges);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var directedGraph = new WeightedGraph(11);
            directedGraph.AddDirectedEdge(0, 1, 1);
            directedGraph.AddDirectedEdge(0, 2, 1);
            directedGraph.AddDirectedEdge(0, 3, 1);
            directedGraph.AddDirectedEdge(0, 4, 1);
            directedGraph.AddDirectedEdge(0, 5, 1);
            directedGraph.AddDirectedEdge(1, 6, 1);
            directedGraph.AddDirectedEdge(2, 6, 1);
            directedGraph.AddDirectedEdge(4, 8, 1);
            directedGraph.AddDirectedEdge(5, 9, 1);
            directedGraph.AddDirectedEdge(5, 10, 1);
            directedGraph.AddDirectedEdge(6, 7, 1);
            directedGraph.AddDirectedEdge(10, 1, 2);
            directedGraph.AddDirectedEdge(0, 10, 3);
            directedGraph.AddDirectedEdge(8, 0, 2);
            directedGraph.AddDirectedEdge(3, 2, 4);
            directedGraph.AddDirectedEdge(6, 9, 5);
            directedGraph.AddDirectedEdge(2, 7, 3);

            var mst = new PrimsMst(directedGraph);
            var result = mst.Run();
            Console.WriteLine(result.Item1);
            foreach (var edge in result.Item2)
            {
                Console.Write("(" + edge.Item1 + "," + edge.Item2 + ") ");
            }

            Console.WriteLine();
            Console.WriteLine();

            var inputStream = new StreamReader(@"input.txt");
            var count = Convert.ToInt32(inputStream.ReadLine().Split(' ')[0]);
            var bigUndirectedGraph = new WeightedGraph(count);
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bigUndirectedGraph.AddUndirectedEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToDouble(line[2]));
            }

            mst = new PrimsMst(bigUndirectedGraph);
            result = mst.Run();
            Console.WriteLine(result.Item1);

            Console.ReadKey();
        }
    }
}
