namespace Ds.Graphs.TravellingSalesman.NearestNeighbour
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ds.Graphs.Repository;

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

    class NearestNeighbourNoGraph
    {
        public List<EuclideanPoint> Points { get; set; }

        public HashSet<int> VisitedSet { get; set; }

        public NearestNeighbourNoGraph(List<EuclideanPoint> points)
        {
            this.Points = points;
        }

        public double Run()
        {
            this.VisitedSet = new HashSet<int> { 0 };
            var currentNode = 0;
            var distanceTravelled = (double)0;

            while (this.VisitedSet.Count < this.Points.Count)
            {
                var minimumDistance = double.PositiveInfinity;
                var nextNode = -1;

                for (var nextNodeCandidate = 0; nextNodeCandidate < this.Points.Count; nextNodeCandidate++)
                {
                    if (nextNodeCandidate == currentNode || this.VisitedSet.Contains(nextNodeCandidate))
                    {
                        continue;
                    }
                    else
                    {
                        var distance = this.Points[currentNode].Distance(this.Points[nextNodeCandidate]);
                        if (distance < minimumDistance)
                        {
                            nextNode = nextNodeCandidate;
                            minimumDistance = distance;
                        }
                    }
                }

                currentNode = nextNode;
                this.VisitedSet.Add(currentNode);
                distanceTravelled += minimumDistance;
                Console.WriteLine($"Processed {this.VisitedSet.Count} out of {this.Points.Count}. Current node: {currentNode + 1}.");
            }

            return distanceTravelled + this.Points[currentNode].Distance(this.Points[0]);
        }
    }

    class NearestNeighbour
    {
        public WeightedGraph Graph { get; set; }

        public HashSet<int> VisitedSet { get; set; }

        public NearestNeighbour(WeightedGraph graph)
        {
            this.Graph = graph;
        }

        public double Run()
        {
            this.VisitedSet = new HashSet<int> { 0 };
            var currentNode = 0;
            var distanceTravelled = (double)0;

            while (this.VisitedSet.Count < this.Graph.VertexCount)
            {
                var nodeList = this.Graph.AdjacencyArray[currentNode].ToList();
                nodeList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                foreach (var nextNode in nodeList)
                {
                    if (!this.VisitedSet.Contains(nextNode.Key))
                    {
                        distanceTravelled += this.Graph.AdjacencyArray[currentNode][nextNode.Key];
                        currentNode = nextNode.Key;
                        this.VisitedSet.Add(currentNode);
                        break;
                    }
                }

                Console.WriteLine($"Processed {this.VisitedSet.Count} out of {this.Graph.VertexCount}.");
            }

            return distanceTravelled + this.Graph.AdjacencyArray[currentNode][0];
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
            var tspnn = new NearestNeighbour(smallGraph);
            Console.WriteLine(tspnn.Run());
            Console.WriteLine();

            var inputStream = new StreamReader(@"smallinput.txt");
            var count = Convert.ToInt32(inputStream.ReadLine().Trim());
            var euclideanPoints = new List<EuclideanPoint>();
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                euclideanPoints.Add(new EuclideanPoint(Convert.ToDouble(line[1]), Convert.ToDouble(line[2])));
            }

            var tspnnng = new NearestNeighbourNoGraph(euclideanPoints);
            Console.WriteLine(tspnnng.Run());
            Console.WriteLine();

            inputStream = new StreamReader(@"input.txt");
            count = Convert.ToInt32(inputStream.ReadLine().Trim());
            euclideanPoints = new List<EuclideanPoint>();
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                euclideanPoints.Add(new EuclideanPoint(Convert.ToDouble(line[1]), Convert.ToDouble(line[2])));
            }

            tspnnng = new NearestNeighbourNoGraph(euclideanPoints);
            Console.WriteLine(tspnnng.Run());
            Console.ReadKey();
        }
    }
}
