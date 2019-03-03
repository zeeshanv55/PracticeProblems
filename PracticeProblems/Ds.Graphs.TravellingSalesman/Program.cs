namespace Ds.Graphs.TravellingSalesman
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
                sum += item;
            }

            return (int)(Math.Pow(10, Math.Floor(Math.Log10(sum) + 1)) * obj.Item1) + (obj.Item2.Count == 0 ? 0 : (sum % obj.Item2.Count));
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

    class TravellingSalesman
    {
        public WeightedGraph Graph { get; set; }

        private Dictionary<Tuple<int, HashSet<int>>, double> Dp { get; set; }

        public TravellingSalesman(WeightedGraph graph)
        {
            this.Graph = graph;
        }

        public double Run()
        {
            this.InitializeDp();

            for (var k = 0; k < this.Graph.VertexCount - 1; k++)
            {
                Console.WriteLine($"Checking sets with cardinality: {k + 1}. Last iteraton size was: {this.Dp.Count}.");
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

                this.Dp = nextDp;
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
