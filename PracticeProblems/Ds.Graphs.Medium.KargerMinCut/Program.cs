namespace Ds.Graphs.Medium.KargerMinCut
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static Random RandomGenerator;

        const int N = 200;

        static Tuple<int, int> RandomlySelect(List<List<int>> adjacencyList)
        {
            int startNodeIndex = -1;
            int endNodeIndex = -1;

            do
            {
                startNodeIndex = RandomGenerator.Next(adjacencyList.Count);
            }
            while (adjacencyList[startNodeIndex] == null || !adjacencyList[startNodeIndex].Any());

            do
            {
                endNodeIndex = adjacencyList[startNodeIndex][RandomGenerator.Next(adjacencyList[startNodeIndex].Count)];
            }
            while (adjacencyList[endNodeIndex] == null || startNodeIndex == endNodeIndex);

            return new Tuple<int, int>(Math.Min(startNodeIndex, endNodeIndex), Math.Max(startNodeIndex, endNodeIndex));
        }

        static void Contract(List<List<int>> adjacencyList, int startIndex, int endIndex)
        {
            adjacencyList[startIndex].RemoveAll(x => x == endIndex);

            foreach (var node in adjacencyList[endIndex])
            {
                if (node != startIndex && node != endIndex)
                {
                    adjacencyList[startIndex].Add(node);
                }
            }

            for (var i = 0; i < N; i++)
            {
                if (i != endIndex && adjacencyList[i] != null)
                {
                    for (var j = 0; j < adjacencyList[i].Count; j++)
                    {
                        if (adjacencyList[i][j] == endIndex)
                        {
                            adjacencyList[i][j] = startIndex;
                        }
                    }
                }
            }

            adjacencyList[endIndex] = null;
        }

        static int? MinCut(List<List<int>> adjacencyList)
        {
            while(adjacencyList.Count(l => l != null) > 2)
            {
                var edge = RandomlySelect(adjacencyList);
                Contract(adjacencyList, edge.Item1, edge.Item2);
            }

            var firstNode = adjacencyList.First(x => x != null);
            var lastNode = adjacencyList.Last(x => x != null);
            var indexOfFirst = adjacencyList.IndexOf(firstNode);
            var indexOfLast = adjacencyList.IndexOf(lastNode);

            if (firstNode.Count == lastNode.Count)
            {
                foreach (var target in firstNode)
                {
                    if (target != indexOfLast)
                    {
                        return null;
                    }
                }

                foreach (var target in lastNode)
                {
                    if (target != indexOfFirst)
                    {
                        return null;
                    }
                }

                return firstNode.Count;
            }

            return null;
        }

        static int RepeatedMinCuts(List<List<int>> adjacencyList)
        {
            var trials = (int)(N * N * Math.Log(N));
            var minimumResult = int.MaxValue;

            for (var i = 0; i < trials; i++)
            {
                var result = (int)MinCut(adjacencyList.Select(x => x.ToList()).ToList());
                if (result < minimumResult)
                {
                    minimumResult = result;
                }

                Console.WriteLine($"Trial: {i}\tResult: {minimumResult}");
            }

            return minimumResult;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            var adjacencyArray = new List<int>[N];
            RandomGenerator = new Random();

            while (!textReader.EndOfStream)
            {
                var line = textReader.ReadLine().Trim('\t');
                var inputLine = Array.ConvertAll(line.Split('\t'), q => Convert.ToInt32(q) - 1);
                var adjacentVertices = new int[inputLine.Length - 1];
                Array.Copy(inputLine, 1, adjacentVertices, 0, inputLine.Length - 1);
                adjacencyArray[inputLine[0]] = adjacentVertices.ToList();
            }

            Console.WriteLine(RepeatedMinCuts(adjacencyArray.ToList()));
            Console.ReadKey();
        }
    }
}
