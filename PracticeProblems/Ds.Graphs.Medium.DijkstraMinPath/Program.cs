namespace Ds.Graphs.Medium.DijkstraMinPath
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        const int N = 200;

        static int DijkstraMinLength(int source, int destination, Dictionary<int, int>[] adjacencyArray)
        {
            var discoveredSet = new HashSet<int>();
            var dijkstraScores = new int[N];

            discoveredSet.Add(source);
            dijkstraScores[source] = 0;

            while (discoveredSet.Count < N)
            {
                var minLength = int.MaxValue;
                var minEdgeHead = -1;

                foreach (var s in discoveredSet)
                {
                    foreach (var e in adjacencyArray[s])
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

            return dijkstraScores[destination];
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            var adjacencyArray = new Dictionary<int, int>[N];

            while (!textReader.EndOfStream)
            {
                var line = textReader.ReadLine().Trim().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var currentIndex = Convert.ToInt32(line[0]) - 1;
                adjacencyArray[currentIndex] = new Dictionary<int, int>();

                for (var i = 1; i < line.Length; i++)
                {
                    var edge = line[i].Trim().Split(',');
                    if (adjacencyArray[currentIndex].ContainsKey(Convert.ToInt32(edge[0]) - 1))
                    {
                        if (Convert.ToInt32(edge[1]) < adjacencyArray[currentIndex][Convert.ToInt32(edge[0]) - 1])
                        {
                            adjacencyArray[currentIndex][Convert.ToInt32(edge[0]) - 1] = Convert.ToInt32(edge[1]);
                        }
                    }
                    else
                    {
                        adjacencyArray[currentIndex].Add(Convert.ToInt32(edge[0]) - 1, Convert.ToInt32(edge[1]));
                    }
                }
            }

            Console.Write(DijkstraMinLength(0, 6, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 36, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 58, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 81, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 98, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 114, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 132, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 164, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 187, adjacencyArray) + ",");
            Console.Write(DijkstraMinLength(0, 196, adjacencyArray));

            Console.ReadKey();
        }
    }
}
