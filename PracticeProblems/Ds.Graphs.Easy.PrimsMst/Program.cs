namespace Ds.Graphs.Easy.PrimsMst
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static double PrimsMst(Dictionary<int, int>[] adjacencyArray)
        {
            var minWeight = (double)0;
            var exploredSet = new HashSet<int>();
            
            exploredSet.Add(0);

            while (exploredSet.Count < adjacencyArray.Length)
            {
                var minEdgeWeight = int.MaxValue;
                var minEdgeHead = -1;

                foreach (var v in exploredSet)
                {
                    foreach (var edge in adjacencyArray[v])
                    {
                        if (!exploredSet.Contains(edge.Key) && edge.Value < minEdgeWeight)
                        {
                            minEdgeWeight = edge.Value;
                            minEdgeHead = edge.Key;
                        }
                    }
                }

                minWeight += minEdgeWeight;
                exploredSet.Add(minEdgeHead);
            }

            return minWeight;
        }

        static void Main(string[] args)
        {
            var inputStream = new StreamReader(@"input.txt");
            var count = Convert.ToInt32(inputStream.ReadLine().Split(' ')[0]);
            var adjacencyArray = new Dictionary<int, int>[count];

            for (var i = 0; i < count; i++)
            {
                adjacencyArray[i] = new Dictionary<int, int>();
            }
            
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (adjacencyArray[Convert.ToInt32(line[0]) - 1].ContainsKey(Convert.ToInt32(line[1]) - 1))
                {
                    if (Convert.ToInt32(line[2]) < adjacencyArray[Convert.ToInt32(line[0]) - 1][Convert.ToInt32(line[1]) - 1])
                    {
                        adjacencyArray[Convert.ToInt32(line[0]) - 1][Convert.ToInt32(line[1]) - 1] = Convert.ToInt32(line[2]);
                    }
                }
                else
                {
                    adjacencyArray[Convert.ToInt32(line[0]) - 1].Add(Convert.ToInt32(line[1]) - 1, Convert.ToInt32(line[2]));
                }

                if (adjacencyArray[Convert.ToInt32(line[1]) - 1].ContainsKey(Convert.ToInt32(line[0]) - 1))
                {
                    if (Convert.ToInt32(line[2]) < adjacencyArray[Convert.ToInt32(line[1]) - 1][Convert.ToInt32(line[0]) - 1])
                    {
                        adjacencyArray[Convert.ToInt32(line[1]) - 1][Convert.ToInt32(line[0]) - 1] = Convert.ToInt32(line[2]);
                    }
                }
                else
                {
                    adjacencyArray[Convert.ToInt32(line[1]) - 1].Add(Convert.ToInt32(line[0]) - 1, Convert.ToInt32(line[2]));
                }
            }

            Console.WriteLine(PrimsMst(adjacencyArray));
            Console.ReadKey();
        }
    }
}
