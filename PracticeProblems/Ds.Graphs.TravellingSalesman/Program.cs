namespace Ds.Graphs.TravellingSalesman
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ds.Graphs.Repository;

    class TupleHashSetComparer : IEqualityComparer<Tuple<int, HashSet<int>>>
    {
        public bool Equals(Tuple<int, HashSet<int>> x, Tuple<int, HashSet<int>> y)
        {
            return x.Item1 == y.Item1 && x.Item2.SetEquals(y.Item2);
        }

        public int GetHashCode(Tuple<int, HashSet<int>> obj)
        {
            var sum = 0;
            foreach (var item in obj.Item2)
            {
                sum = sum + item + (item * item);
            }

            return sum * obj.Item1;
        }
    }

    class EuclideanPoint
    {
        public double X { get; set; }

        public double Y { get; set; }

        public EuclideanPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double Distance(EuclideanPoint p)
        {
            return Math.Sqrt(Math.Pow(p.X - this.X, 2) + Math.Pow(p.Y - this.Y, 2));
        }
    }

    class TravellingSalesmanRunner : IDisposable
    {
        private Dictionary<Tuple<int, HashSet<int>>, double> Dp { get; set; }

        public WeightedGraph Graph { get; set; }

        public TravellingSalesmanRunner(WeightedGraph graph, Dictionary<Tuple<int, HashSet<int>>, double> dp)
        {
            this.Graph = new WeightedGraph(graph);
            this.Dp = new Dictionary<Tuple<int, HashSet<int>>, double>(dp);
        }

        ~TravellingSalesmanRunner()
        {
            this.Dp = null;
        }

        public Dictionary<Tuple<int, HashSet<int>>, double> RunIteration(double maxDistance)
        {
            var nextDp = new Dictionary<Tuple<int, HashSet<int>>, double>(new TupleHashSetComparer());

            foreach (var dpPoint in this.Dp)
            {
                for (var i = 1; i < this.Graph.VertexCount; i++)
                {
                    if (dpPoint.Key.Item1 == i || dpPoint.Key.Item2.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        var newParsedSet = new HashSet<int>(dpPoint.Key.Item2);
                        newParsedSet.Add(dpPoint.Key.Item1);

                        var newDistance = dpPoint.Value + this.Graph.AdjacencyArray[dpPoint.Key.Item1][i];

                        if (newDistance <= maxDistance)
                        {
                            var newDpKey = new Tuple<int, HashSet<int>>(i, newParsedSet);

                            if (nextDp.ContainsKey(newDpKey))
                            {
                                if (newDistance < nextDp[newDpKey])
                                {
                                    nextDp[newDpKey] = newDistance;
                                }
                            }
                            else
                            {
                                nextDp.Add(newDpKey, newDistance);
                            }
                        }
                    }
                }
            }

            return nextDp;
        }

        public Dictionary<Tuple<int, HashSet<int>>, double> RunQuickIteration(int maxCount, double maxDistance)
        {
            var nextDp = new Dictionary<Tuple<int, HashSet<int>>, double>(new TupleHashSetComparer());
            var dpList = this.Dp.ToList();
            dpList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            foreach (var dpPoint in dpList)
            {
                for (var i = 1; i < this.Graph.VertexCount; i++)
                {
                    if (dpPoint.Key.Item1 == i || dpPoint.Key.Item2.Contains(i))
                    {
                        continue;
                    }
                    else
                    {
                        var newParsedSet = new HashSet<int>(dpPoint.Key.Item2);
                        newParsedSet.Add(dpPoint.Key.Item1);

                        var newDistance = dpPoint.Value + this.Graph.AdjacencyArray[dpPoint.Key.Item1][i];

                        if (newDistance <= maxDistance)
                        {
                            var newDpKey = new Tuple<int, HashSet<int>>(i, newParsedSet);

                            if (nextDp.ContainsKey(newDpKey))
                            {
                                if (newDistance < nextDp[newDpKey])
                                {
                                    nextDp[newDpKey] = newDistance;
                                }
                            }
                            else
                            {
                                nextDp.Add(newDpKey, newDistance);
                                if (nextDp.Count == maxCount)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (nextDp.Count == maxCount)
                {
                    break;
                }
            }

            return nextDp;
        }

        public void Dispose()
        {
            this.Graph.Dispose();
            this.Dp.Clear();
        }
    }

    class TravellingSalesman
    {
        public WeightedGraph Graph { get; set; }

        private Dictionary<Tuple<int, HashSet<int>>, double> Dp { get; set; }

        private double[] MaxPathLengthAtStep { get; set; }

        private double HeuristicPathLength { get; set; }

        public TravellingSalesman(WeightedGraph graph)
        {
            this.Graph = graph;
        }

        public double Run()
        {
            this.InitializeHeuristics(new List<int> { 100, 2000, 10000, 20000, 100000, 200000, 1000000 });
            this.InitializeDp();

            for (var k = 0; k < this.Graph.VertexCount - 1; k++)
            {
                using (var iterationRunner = new TravellingSalesmanRunner(this.Graph, this.Dp))
                {
                    Console.WriteLine($"Checking sets with cardinality: {k + 1}. Max path length allowed: {this.MaxPathLengthAtStep[k]}. Last iteraton size was: {this.Dp.Count}.");
                    var nextDp = iterationRunner.RunIteration(this.MaxPathLengthAtStep[k]);
                    this.Dp = new Dictionary<Tuple<int, HashSet<int>>, double>(nextDp);
                }
            }

            var min = double.PositiveInfinity;
            foreach (var dpPoint in this.Dp)
            {
                var currentDistance = dpPoint.Value + this.Graph.AdjacencyArray[dpPoint.Key.Item1][0];
                if (currentDistance < min)
                {
                    min = currentDistance;
                }
            }

            return min;
        }

        private void InitializeHeuristics(List<int> maxDpSizes)
        {
            var currentHeuristicDistance = double.PositiveInfinity;
            var maxLengthAtStep = new double[this.Graph.VertexCount - 1];

            for (var k = 0; k < this.Graph.VertexCount - 1; k++)
            {
                maxLengthAtStep[k] = double.PositiveInfinity;
            }

            foreach (var maxDpSize in maxDpSizes)
            {
                Console.WriteLine($"\nMax DP size allowed: {maxDpSize}");
                this.InitializeDp();

                for (var k = 0; k < this.Graph.VertexCount - 1; k++)
                {
                    using (var iterationRunner = new TravellingSalesmanRunner(this.Graph, this.Dp))
                    {
                        Console.WriteLine($"Heuristically checking sets with cardinality: {k + 1}. Max path length allowed: {maxLengthAtStep[k]}. Last iteraton size was: {this.Dp.Count}.");
                        var nextDp = iterationRunner.RunQuickIteration(maxDpSize, maxLengthAtStep[k]);

                        this.Dp = new Dictionary<Tuple<int, HashSet<int>>, double>(nextDp);
                        maxLengthAtStep[k] = nextDp.Values.Max();
                    }
                }

                var min = double.PositiveInfinity;
                foreach (var dpPoint in this.Dp)
                {
                    var currentDistance = dpPoint.Value + this.Graph.AdjacencyArray[dpPoint.Key.Item1][0];
                    if (currentDistance < min)
                    {
                        min = currentDistance;
                    }
                }

                currentHeuristicDistance = min;
                Console.WriteLine($"Current heuristic path length: {currentHeuristicDistance}.");
            }

            this.HeuristicPathLength = currentHeuristicDistance;
            this.MaxPathLengthAtStep = maxLengthAtStep;
        }

        private void InitializeDp()
        {
            this.Dp = new Dictionary<Tuple<int, HashSet<int>>, double>(new TupleHashSetComparer());
            this.Dp.Add(new Tuple<int, HashSet<int>>(0, new HashSet<int>()), 0);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var smallGraph = new WeightedGraph(5);
            smallGraph.AddUndirectedEdge(0, 1, 5);
            smallGraph.AddUndirectedEdge(0, 2, 3);
            smallGraph.AddUndirectedEdge(0, 3, 2);
            smallGraph.AddUndirectedEdge(0, 4, 1);
            smallGraph.AddUndirectedEdge(1, 2, 7);
            smallGraph.AddUndirectedEdge(1, 3, 4);
            smallGraph.AddUndirectedEdge(1, 4, 9);
            smallGraph.AddUndirectedEdge(2, 3, 8);
            smallGraph.AddUndirectedEdge(2, 4, 3);
            smallGraph.AddUndirectedEdge(3, 4, 6);
            var tsp = new TravellingSalesman(smallGraph);
            Console.WriteLine(tsp.Run());

            var inputStream = new StreamReader(@"input.txt");
            var count = Convert.ToInt32(inputStream.ReadLine().Trim());
            var undirectedGraph = new WeightedGraph(count);
            var euclideanPoints = new List<EuclideanPoint>();
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                euclideanPoints.Add(new EuclideanPoint(Convert.ToDouble(line[0]), Convert.ToDouble(line[1])));
            }

            for (var i = 0; i < count; i++)
            {
                for (var j = i + 1; j < count; j++)
                {
                    undirectedGraph.AddUndirectedEdge(i, j, euclideanPoints[i].Distance(euclideanPoints[j]));
                }
            }

            tsp = new TravellingSalesman(undirectedGraph);
            Console.WriteLine(tsp.Run());

            Console.ReadKey();
        }
    }
}
