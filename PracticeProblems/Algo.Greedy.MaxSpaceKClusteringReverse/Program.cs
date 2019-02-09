namespace Algo.Greedy.MaxSpaceKClusteringReverse
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    class Program
    {
        const int D = 3;

        public static HashSet<int> DepthFirstSearch(List<int>[] adjacencyList, int s)
        {
            var exploredSet = new HashSet<int>();
            var dfsStack = new Stack<int>();
            dfsStack.Push(s);
            exploredSet.Add(s);

            while (dfsStack.Count > 0)
            {
                var t = dfsStack.Pop();
                foreach (var v in adjacencyList[t])
                {
                    if (!exploredSet.Contains(v))
                    {
                        dfsStack.Push(v);
                        exploredSet.Add(v);
                    }
                }
            }

            return exploredSet;
        }

        static string BitFlip(string s, int i)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var resultBuilder = new StringBuilder();
            for (var j = 0; j < s.Length; j++)
            {
                if (i == j)
                {
                    if (s[j] == '0')
                    {
                        resultBuilder.Append('1');
                    }
                    else
                    {
                        resultBuilder.Append('0');
                    }
                }
                else
                {
                    resultBuilder.Append(s[j]);
                }
            }

            return resultBuilder.ToString();
        }

        static HashSet<string> GetPossibleStringsWithDistance(string s, int d)
        {
            var returnSet = new HashSet<string>();

            if (d == 1)
            {
                for (var i = 0; i < s.Length; i++)
                {
                    returnSet.Add(BitFlip(s, i));
                }
            }
            else
            {
                for (var i = 0; i < s.Length - 1; i++)
                {
                    var preStringBuilder = new StringBuilder();

                    for (var j = 0; j < i; j++)
                    {
                        preStringBuilder.Append(s[j]);
                    }

                    if (s[i] == '0')
                    {
                        preStringBuilder.Append('1');
                    }
                    else
                    {
                        preStringBuilder.Append('0');
                    }

                    var preString = preStringBuilder.ToString();
                    var postStrings = GetPossibleStringsWithDistance(s.Substring(i + 1), d - 1);

                    foreach (var s2 in postStrings)
                    {
                        returnSet.Add($"{preString}{s2}");
                    }
                }
            }

            return returnSet;
        }

        static HashSet<string> GetPossibleStringsWithDistanceUpto(string s, int d)
        {
            var returnSet = new HashSet<string>();

            for (var i = 1; i <= d; i++)
            {
                var partialSet = GetPossibleStringsWithDistance(s, i);
                foreach (var s1 in partialSet)
                {
                    returnSet.Add(s1);
                }
            }

            return returnSet;
        }

        static int MaxSpaceKClusterReverse(HashSet<string> inputSet)
        {
            var inputList = new List<string>();
            foreach (var s in inputSet)
            {
                inputList.Add(s);
            }

            var inputDictionary = new Dictionary<string, int>();
            var leaderLists = new List<int>[inputList.Count];

            for (var i = 0; i < inputList.Count; i++)
            {
                leaderLists[i] = new List<int>();
                inputDictionary.Add(inputList[i], i);
            }

            for (var i = 0; i < inputList.Count; i++)
            {
                var allPossibleNeighbours = GetPossibleStringsWithDistanceUpto(inputList[i], D - 1);
                foreach (var p in allPossibleNeighbours)
                {
                    if (inputSet.Contains(p))
                    {
                        leaderLists[i].Add(inputDictionary[p]);
                    }
                }
            }

            var clusteredSets = new List<HashSet<string>>();
            for (var i = 0; i < inputList.Count; i++)
            {
                if (inputSet.Contains(inputList[i]))
                {
                    var reachableSet = DepthFirstSearch(leaderLists, i);
                    var clusteredSet = new HashSet<string>();
                    foreach (var v in reachableSet)
                    {
                        inputSet.Remove(inputList[v]);
                        clusteredSet.Add(inputList[v]);
                    }

                    clusteredSets.Add(clusteredSet);
                }
            }

            return clusteredSets.Count;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            var line = textReader.ReadLine();
            var inputSet = new HashSet<string>();

            while (!textReader.EndOfStream)
            {
                inputSet.Add(textReader.ReadLine().Trim().Replace(" ", string.Empty));
            }

            Console.WriteLine(MaxSpaceKClusterReverse(inputSet));
            Console.ReadKey();
        }
    }
}
