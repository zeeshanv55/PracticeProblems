namespace Algo.Dp.FloydWarshall
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Graph
    {
        public int VertexCount { get; set; }

        public int EdgeCount { get; set; }

        public Dictionary<int, int>[] AdjacencyArray { get; set; }

        public Graph(int vertexCount, int edgeCount)
        {
            this.VertexCount = vertexCount;
            this.EdgeCount = edgeCount;
            this.AdjacencyArray = new Dictionary<int, int>[this.VertexCount];
            
            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new Dictionary<int, int>();
            }
        }

        public Graph(Graph graph)
        {
            this.VertexCount = graph.VertexCount;
            this.EdgeCount = graph.EdgeCount;
            this.AdjacencyArray = new Dictionary<int, int>[this.VertexCount];

            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new Dictionary<int, int>(graph.AdjacencyArray[i]);
            }
        }

        public void AddEdge(int startVertex, int endVertex, int edgeCost)
        {
            this.AdjacencyArray[startVertex].Add(endVertex, edgeCost);
        }
    }

    class FloydWarshall
    {
        public Graph Graph { get; set; }

        public int?[,] Dp { get; set; }

        public int?[,] DpInitiator { get; set; }

        public int MinPathLength { get; set; }

        public FloydWarshall(Graph graph)
        {
            this.Graph = new Graph(graph);
            this.Dp = new int?[this.Graph.VertexCount, this.Graph.VertexCount];

            for (var i = 0; i < this.Graph.VertexCount; i++)
            {
                for (var j = 0; j < this.Graph.VertexCount; j++)
                {
                    if (i == j)
                    {
                        this.Dp[i, j] = 0;
                    }
                    else if (this.Graph.AdjacencyArray[i].ContainsKey(j))
                    {
                        this.Dp[i, j] = this.Graph.AdjacencyArray[i][j];
                    }
                    else
                    {
                        this.Dp[i, j] = null;
                    }
                }
            }
        }

        public void Run()
        {
            for (var k = 1; k < this.Graph.VertexCount; k++)
            {
                this.DpInitiator = (int?[,])this.Dp.Clone();
                this.Dp = new int?[this.Graph.VertexCount, this.Graph.VertexCount];
                for (var i = 0; i < this.Graph.VertexCount; i++)
                {
                    for (var j = 0; j < this.Graph.VertexCount; j++)
                    {
                        var previous = this.DpInitiator[i, j];
                        var current1 = this.DpInitiator[i, k];
                        var current2 = this.DpInitiator[k, j];
                        
                        if (previous != null && current1 != null && current2 != null)
                        {
                            this.Dp[i, j] = Math.Min((int)previous, (int)current1 + (int)current2);
                        }
                        else
                        {
                            if (previous == null && current1 != null && current2 != null)
                            {
                                this.Dp[i, j] = (int)current1 + (int)current2;
                            }
                            else if (previous != null)
                            {
                                this.Dp[i, j] = (int)previous;
                            }
                            else
                            {
                                this.Dp[i, j] = null;
                            }
                        }
                    }
                }
            }

            var min = int.MaxValue;
            for (var i = 0; i < this.Graph.VertexCount; i++)
            {
                for (var j = 0; j < this.Graph.VertexCount; j++)
                {
                    if (this.Dp[i, j] != null && this.Dp[i, j] < min)
                    {
                        min = (int)this.Dp[i, j];
                    }
                }
            }

            this.MinPathLength = min;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input1.txt");
            var line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph1 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph1.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToInt32(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input2.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph2 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph2.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToInt32(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input3.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph3 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph3.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToInt32(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input0.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph0 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph0.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToInt32(line[2]));
            }
            textReader.Close();

            var fw0 = new FloydWarshall(graph0);
            var fw1 = new FloydWarshall(graph1);
            var fw2 = new FloydWarshall(graph2);
            var fw3 = new FloydWarshall(graph3);

            fw0.Run();
            Console.WriteLine(fw0.MinPathLength);

            fw1.Run();
            Console.WriteLine(fw1.MinPathLength);

            fw2.Run();
            Console.WriteLine(fw2.MinPathLength);

            fw3.Run();
            Console.WriteLine(fw3.MinPathLength);

            Console.ReadKey();
        }
    }
}
