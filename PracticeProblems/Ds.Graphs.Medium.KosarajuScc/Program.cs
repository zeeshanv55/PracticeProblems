using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Graphs.Medium.KosarajuScc
{
    class Program
    {
        const int N = 875714;
        
        static int[] RunningTimes = new int[N];

        static int[] Leaders = new int[N];

        static int T = 0;

        static int S;

        static HashSet<int> ExploredSet = new HashSet<int>();

        public static void DepthFirstSearch(List<int>[] adjacencyArray, int s)
        {
            var dfsStack = new Stack<int>();
            dfsStack.Push(s);
            ExploredSet.Add(s);
            Leaders[S]++;

            while (dfsStack.Any())
            {
                var v = dfsStack.Peek();

                var foundNewVertices = false;
                for (var i = 0; i < adjacencyArray[v].Count; i++)
                {
                    if (!ExploredSet.Contains(adjacencyArray[v][i]))
                    {
                        foundNewVertices = true;
                        dfsStack.Push(adjacencyArray[v][i]);
                        ExploredSet.Add(adjacencyArray[v][i]);
                        Leaders[S]++;
                    }
                }

                if (!foundNewVertices)
                {
                    dfsStack.Pop();
                    RunningTimes[T] = v;
                    T++;
                }
            }
        }

        public static List<int> GetSccs(List<int>[] adjacencyArray, List<int>[] reverseAdjacencyArray)
        {
            for (var i = N - 1; i >= 0; i--)
            {
                if (!ExploredSet.Contains(i))
                {
                    S = i;
                    DepthFirstSearch(reverseAdjacencyArray, i);
                }
            }

            ExploredSet = new HashSet<int>();
            Leaders = new int[N];
            T = 0;
            
            for (var i = N - 1; i >= 0; i--)
            {
                if (!ExploredSet.Contains(RunningTimes[i]))
                {
                    S = RunningTimes[i];
                    DepthFirstSearch(adjacencyArray, RunningTimes[i]);
                }
            }

            var leaderList = Leaders.ToList();
            leaderList.Sort();
            return leaderList;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            var adjacencyArray = new List<int>[N];
            var reverseAdjacencyArray = new List<int>[N];

            for (var i = 0; i < N; i++)
            {
                adjacencyArray[i] = new List<int>();
                reverseAdjacencyArray[i] = new List<int>();
            }

            while (!textReader.EndOfStream)
            {
                var line = textReader.ReadLine().Trim().Split(' ');
                adjacencyArray[Convert.ToInt32(line[0]) - 1].Add(Convert.ToInt32(line[1]) - 1);
                reverseAdjacencyArray[Convert.ToInt32(line[1]) - 1].Add(Convert.ToInt32(line[0]) - 1);
            }

            var leaderList = GetSccs(adjacencyArray, reverseAdjacencyArray);

            for (var i = N - 1; i >= N - 6; i--)
            {
                Console.WriteLine(leaderList[i]);
            }

            Console.ReadKey();
        }
    }
}
