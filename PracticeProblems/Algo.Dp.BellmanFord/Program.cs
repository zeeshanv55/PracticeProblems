namespace Algo.Dp.BellmanFord
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Graph
    {
        public int VertexCount { get; set; }

        public int EdgeCount { get; set; }

        public Dictionary<int, double>[] AdjacencyArray { get; set; }

        public Dictionary<int, double>[] ReverseAdjacencyArray { get; set; }

        public Graph(int vertexCount, int edgeCount)
        {
            this.VertexCount = vertexCount;
            this.EdgeCount = edgeCount;
            this.AdjacencyArray = new Dictionary<int, double>[this.VertexCount];
            this.ReverseAdjacencyArray = new Dictionary<int, double>[this.VertexCount];

            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new Dictionary<int, double>();
                this.ReverseAdjacencyArray[i] = new Dictionary<int, double>();
            }
        }

        public Graph(Graph graph)
        {
            this.VertexCount = graph.VertexCount;
            this.EdgeCount = graph.EdgeCount;
            this.AdjacencyArray = new Dictionary<int, double>[this.VertexCount];
            this.ReverseAdjacencyArray = new Dictionary<int, double>[this.VertexCount];

            for (var i = 0; i < this.VertexCount; i++)
            {
                this.AdjacencyArray[i] = new Dictionary<int, double>(graph.AdjacencyArray[i]);
                this.ReverseAdjacencyArray[i] = new Dictionary<int, double>(graph.ReverseAdjacencyArray[i]);
            }
        }

        public void AddEdge(int startVertex, int endVertex, double edgeCost)
        {
            this.AdjacencyArray[startVertex].Add(endVertex, edgeCost);
            this.ReverseAdjacencyArray[endVertex].Add(startVertex, edgeCost);
        }
    }

    class BellmanFord
    {
        public Graph Graph { get; set; }

        public int StartVertex { get; set; }

        public double[][] Dp { get; set; }

        public BellmanFord(Graph graph, int startVertex)
        {
            this.Graph = new Graph(graph);
            this.StartVertex = startVertex;
            this.Dp = new double[this.Graph.VertexCount][];

            for (var i = 0; i < this.Dp.Length; i++)
            {
                this.Dp[i] = new double[this.Graph.VertexCount];
            }

            for (var i = 0; i < this.Graph.VertexCount; i++)
            {
                if (i == this.StartVertex)
                {
                    this.Dp[0][this.StartVertex] = 0;
                }
                else
                {
                    this.Dp[0][i] = double.PositiveInfinity;
                }
            }
        }

        public bool ContainsNegativeCycle()
        {
            var min = double.PositiveInfinity;
            var found = false;
            for (var i = 1; i < this.Graph.VertexCount; i++)
            {
                for (var j = 0; j < this.Graph.VertexCount; j++)
                {
                    var minIndegreeCost = double.PositiveInfinity;
                    foreach (var inEdge in this.Graph.ReverseAdjacencyArray[j])
                    {
                        if (this.Dp[i - 1][inEdge.Key] + inEdge.Value < minIndegreeCost)
                        {
                            minIndegreeCost = this.Dp[i - 1][inEdge.Key] + inEdge.Value;
                        }
                    }

                    this.Dp[i][j] = Math.Min(minIndegreeCost, this.Dp[i - 1][j]);

                    if (this.Dp[i][j] < min)
                    {
                        min = this.Dp[i][j];
                    }
                }
            }

            for (var j = 0; j < this.Graph.VertexCount; j++)
            {
                var minIndegreeCost = double.PositiveInfinity;
                foreach (var inEdge in this.Graph.ReverseAdjacencyArray[j])
                {
                    if (this.Dp[this.Graph.VertexCount - 1][inEdge.Key] + inEdge.Value < minIndegreeCost)
                    {
                        minIndegreeCost = this.Dp[this.Graph.VertexCount - 1][inEdge.Key] + inEdge.Value;
                    }
                }

                if (minIndegreeCost < this.Dp[this.Graph.VertexCount - 1][j])
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public double Run()
        {
            var i = 1;
            var min = double.PositiveInfinity;
            for (; i < this.Graph.VertexCount; i++)
            {
                var valuesChanged = false;
                for (var j = 0; j < this.Graph.VertexCount; j++)
                {
                    var minIndegreeCost = double.PositiveInfinity;
                    foreach (var inEdge in this.Graph.ReverseAdjacencyArray[j])
                    {
                        if (this.Dp[i - 1][inEdge.Key] + inEdge.Value < minIndegreeCost)
                        {
                            minIndegreeCost = this.Dp[i - 1][inEdge.Key] + inEdge.Value;
                        }
                    }

                    this.Dp[i][j] = Math.Min(minIndegreeCost, this.Dp[i - 1][j]);
                    
                    if (this.Dp[i][j] != this.Dp[i - 1][j])
                    {
                        valuesChanged = true;
                    }

                    if (this.Dp[i][j] < min)
                    {
                        min = this.Dp[i][j];
                    }
                }

                if (!valuesChanged)
                {
                    break;
                }
            }

            return min;
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
                graph1.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToDouble(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input2.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph2 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph2.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToDouble(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input3.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph3 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph3.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToDouble(line[2]));
            }
            textReader.Close();

            textReader = new StreamReader(@"input0.txt");
            line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var graph0 = new Graph(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                graph0.AddEdge(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[1]) - 1, Convert.ToDouble(line[2]));
            }
            textReader.Close();

            var bf0 = new BellmanFord(graph0, 0);
            var bf1 = new BellmanFord(graph1, 0);
            var bf2 = new BellmanFord(graph2, 0);
            var bf3 = new BellmanFord(graph3, 0);

            var cycles0 = bf0.ContainsNegativeCycle();
            var cycles1 = bf1.ContainsNegativeCycle();
            var cycles2 = bf2.ContainsNegativeCycle();
            var cycles3 = bf3.ContainsNegativeCycle();

            var minimum0 = double.PositiveInfinity;
            var minimum1 = double.PositiveInfinity;
            var minimum2 = double.PositiveInfinity;
            var minimum3 = double.PositiveInfinity;

            if (!cycles0)
            {
                for (var i = 0; i < graph0.VertexCount; i++)
                {
                    var bf = new BellmanFord(graph0, i);
                    var results = bf.Run();
                    minimum0 = Math.Min(minimum0, results);
                }
            }

            if (!cycles1)
            {
                for (var i = 0; i < graph1.VertexCount; i++)
                {
                    var bf = new BellmanFord(graph1, i);
                    var results = bf.Run();
                    minimum1 = Math.Min(minimum1, results);
                }
            }

            if (!cycles2)
            {
                for (var i = 0; i < graph2.VertexCount; i++)
                {
                    var bf = new BellmanFord(graph2, i);
                    var results = bf.Run();
                    minimum2 = Math.Min(minimum2, results);
                }
            }

            if (!cycles3)
            {
                for (var i = 0; i < graph3.VertexCount; i++)
                {
                    var bf = new BellmanFord(graph3, i);
                    var results = bf.Run();
                    minimum3 = Math.Min(minimum3, results);
                }
            }

            Console.WriteLine(minimum0);
            Console.WriteLine(minimum1);
            Console.WriteLine(minimum2);
            Console.WriteLine(minimum3);

            Console.ReadKey();
        }
    }
}
