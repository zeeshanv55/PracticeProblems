namespace Algo.Greedy.MaxSpaceKClustering
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static Random RandomGenerator { get; set; }

        const int K = 4;

        static int N { get; set; }

        public static int Partition(int startIndex, int endIndex, Dictionary<Tuple<long, long>, double> edgeList, List<Tuple<long, long>> edgeKeys)
        {
            var randomMedian = RandomGenerator.Next(startIndex, endIndex + 1);

            var t = edgeKeys[startIndex];
            edgeKeys[startIndex] = edgeKeys[randomMedian];
            edgeKeys[randomMedian] = t;

            var i = startIndex + 1;
            var j = startIndex + 1;

            while (i <= endIndex && j <= endIndex)
            {
                if (edgeList[edgeKeys[j]] < edgeList[edgeKeys[startIndex]])
                {
                    t = edgeKeys[i];
                    edgeKeys[i] = edgeKeys[j];
                    edgeKeys[j] = t;

                    i++;
                }

                j++;
            }

            t = edgeKeys[i - 1];
            edgeKeys[i - 1] = edgeKeys[startIndex];
            edgeKeys[startIndex] = t;

            return i - 1;
        }

        static void QuickSortEdges(int startIndex, int endIndex, Dictionary<Tuple<long, long>, double> edgeList, List<Tuple<long, long>> edgeKeys)
        {
            if (endIndex <= startIndex)
            {
                return;
            }

            var partitionIndex = -1;
            partitionIndex = Partition(startIndex, endIndex, edgeList, edgeKeys);

            QuickSortEdges(startIndex, partitionIndex - 1, edgeList, edgeKeys);
            QuickSortEdges(partitionIndex + 1, endIndex, edgeList, edgeKeys);
        }

        static double MaxSpaceKCluster(Dictionary<Tuple<long, long>, double> edgeList, List<Tuple<long, long>> edgeKeys, int n, int k)
        {
            var leaderLists = new Dictionary<long, List<long>>();
            var leaderPointers = new long[n];
            for (var i = 0; i < n; i++)
            {
                leaderPointers[i] = i;
                leaderLists[i] = new List<long> { i };
            }

            RandomGenerator = new Random();
            QuickSortEdges(0, edgeKeys.Count - 1, edgeList, edgeKeys);

            while (leaderLists.Count > k)
            {
                var smallestEdge = edgeKeys[0];
                var v1 = smallestEdge.Item1;
                var v2 = smallestEdge.Item2;
                
                if (leaderPointers[v1] != leaderPointers[v2])
                {
                    if (leaderLists[leaderPointers[v1]].Count >= leaderLists[leaderPointers[v2]].Count)
                    {
                        var leaderToRemove = leaderPointers[v2];
                        foreach (var v in leaderLists[leaderPointers[v2]])
                        {
                            leaderLists[leaderPointers[v1]].Add(v);
                            leaderPointers[v] = leaderPointers[v1];
                        }

                        leaderLists.Remove(leaderToRemove);
                    }
                    else
                    {
                        var leaderToRemove = leaderPointers[v1];
                        foreach (var v in leaderLists[leaderPointers[v1]])
                        {
                            leaderLists[leaderPointers[v2]].Add(v);
                            leaderPointers[v] = leaderPointers[v2];
                        }

                        leaderLists.Remove(leaderToRemove);
                    }
                }

                edgeKeys.RemoveAt(0);
            }

            while (leaderPointers[edgeKeys[0].Item1] == leaderPointers[edgeKeys[0].Item2])
            {
                edgeKeys.RemoveAt(0);
            }

            return edgeList[edgeKeys[0]];
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            N = Convert.ToInt32(textReader.ReadLine().Trim());
            var edgeList = new Dictionary<Tuple<long, long>, double>();
            var edgeKeys = new List<Tuple<long, long>>();

            while (!textReader.EndOfStream)
            {
                var line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var i = Convert.ToInt64(line[0]) - 1;
                var j = Convert.ToInt64(line[1]) - 1;
                var e = Convert.ToInt64(line[2]);

                var edgeKey = new Tuple<long, long>(Math.Min(i, j), Math.Max(i, j));
                if (!edgeList.ContainsKey(edgeKey))
                {
                    edgeKeys.Add(edgeKey);
                    edgeList.Add(edgeKey, e);
                }
                else
                {
                    if (e < edgeList[edgeKey])
                    {
                        edgeList[edgeKey] = e;
                    }
                }
            }

            Console.WriteLine(MaxSpaceKCluster(edgeList, edgeKeys, N, K));
            Console.ReadKey();
        }
    }
}
