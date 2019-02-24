namespace Ds.Graphs.MinPath.Dijkstras.Linear
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Repository;
    
    class DijkstrasMinPath
    {
        WeightedGraph Graph { get; set; }

        public DijkstrasMinPath(WeightedGraph graph)
        {
            this.Graph = graph;
        }

        public double Run(int startVertex, int endVertex)
        {
            var discoveredSet = new HashSet<int>();
            var dijkstraScores = new double[this.Graph.VertexCount];

            discoveredSet.Add(startVertex);
            dijkstraScores[startVertex] = 0;

            while (discoveredSet.Count < this.Graph.VertexCount)
            {
                var minLength = double.MaxValue;
                var minEdgeHead = -1;

                foreach (var s in discoveredSet)
                {
                    foreach (var e in this.Graph.AdjacencyArray[s])
                    {
                        if (!discoveredSet.Contains(e.Key) && dijkstraScores[s] + e.Value < minLength)
                        {
                            minLength = dijkstraScores[s] + e.Value;
                            minEdgeHead = e.Key;
                        }
                    }
                }

                dijkstraScores[minEdgeHead] = minLength;
                discoveredSet.Add(minEdgeHead);
            }

            return dijkstraScores[endVertex];
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

            var dmp = new DijkstrasMinPath(directedGraph);
            Console.WriteLine(dmp.Run(4, 10));
            Console.WriteLine(dmp.Run(0, 7));
            Console.WriteLine(dmp.Run(0, 10));
            Console.WriteLine();

            var bigDirectedGraph = new WeightedGraph(200);
            var inputStream = new StreamReader(@"input.txt");
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var currentVertex = Convert.ToInt32(line[0]) - 1;

                for (var i = 1; i < line.Length; i++)
                {
                    var edge = line[i].Trim().Split(',');
                    bigDirectedGraph.AddDirectedEdge(currentVertex, Convert.ToInt32(edge[0]) - 1, Convert.ToDouble(edge[1]));
                }
            }

            dmp = new DijkstrasMinPath(bigDirectedGraph);
            Console.WriteLine(dmp.Run(0, 6));
            Console.WriteLine(dmp.Run(0, 36));
            Console.WriteLine(dmp.Run(0, 58));
            Console.WriteLine(dmp.Run(0, 81));
            Console.WriteLine(dmp.Run(0, 98));
            Console.WriteLine(dmp.Run(0, 114));
            Console.WriteLine(dmp.Run(0, 132));
            Console.WriteLine(dmp.Run(0, 164));
            Console.WriteLine(dmp.Run(0, 187));
            Console.WriteLine(dmp.Run(0, 196));

            Console.ReadKey();
        }
    }
}
