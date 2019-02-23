namespace Algo.Dp.PerfectSumWithIndices
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static int[] Array;

        static HashSet<HashSet<int>> ResultSetSet;

        static void PerfectSum(HashSet<int> indices, HashSet<int> removedIndices, int sum)
        {
            if (sum == 0)
            {
                ResultSetSet.Add(removedIndices);
            }
            else
            {
                foreach (var indexToRemove in indices)
                {
                    if (sum - Array[indexToRemove] >= 0)
                    {
                        var reducedIndices = new HashSet<int>(indices);
                        reducedIndices.Remove(indexToRemove);
                        var appendedRemovedIndices = new HashSet<int>(removedIndices);
                        appendedRemovedIndices.Add(indexToRemove);

                        PerfectSum(reducedIndices, appendedRemovedIndices, sum - Array[indexToRemove]);
                    }
                }
            }
        }

        class HashSetComparer : IEqualityComparer<HashSet<int>>
        {
            public bool Equals(HashSet<int> x, HashSet<int> y)
            {
                return x.SetEquals(y);
            }

            public int GetHashCode(HashSet<int> obj)
            {
                var sum = 0;
                foreach (var item in obj)
                {
                    sum += item;
                }

                return sum % obj.Count;
            }
        }

        static void Main(string[] args)
        {
            Array = new int[] { 10, 3, 6, 4, 1, 8, 2, 2 };
            ResultSetSet = new HashSet<HashSet<int>>(new HashSetComparer());
            PerfectSum(new HashSet<int> { 0, 1, 2, 3, 4, 5, 6, 7 }, new HashSet<int>(), 10);
            Console.ReadKey();
        }
    }
}
