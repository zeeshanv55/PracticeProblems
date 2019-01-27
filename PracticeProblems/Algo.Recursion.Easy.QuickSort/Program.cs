using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Recursion.Easy.QuickSort
{
    class Program
    {
        public static Random RandomGenerator = new Random();

        public enum PartitionBy
        {
            First,
            Last,
            Median,
            Random
        }

        public static int FindMedian(int a, int b, int c)
        {
            if ((a <= b && a >= c) || (a >= b && a <= c))
            {
                return 0;
            }

            if ((b <= a && b >= c) || (b >= a && b <= c))
            {
                return 1;
            }

            if ((c <= a && c >= b) || (c >= a && c <= b))
            {
                return 2;
            }

            return -1;
        }

        public static int PartitionByFirst(List<int> list, int startIndex, int endIndex)
        {
            var i = startIndex + 1;
            var j = startIndex + 1;

            while (i <= endIndex && j <= endIndex)
            {
                if (list[j] < list[startIndex])
                {
                    var t1 = list[i];
                    list[i] = list[j];
                    list[j] = t1;
                    i++;
                }

                j++;
            }

            var t2 = list[i - 1];
            list[i - 1] = list[startIndex];
            list[startIndex] = t2;

            return i - 1;
        }

        public static int PartitionByLast(List<int> list, int startIndex, int endIndex)
        {
            var t1 = list[startIndex];
            list[startIndex] = list[endIndex];
            list[endIndex] = t1;

            return PartitionByFirst(list, startIndex, endIndex);
        }

        public static int PartitionByMedian(List<int> list, int startIndex, int endIndex)
        {
            var median = FindMedian(list[startIndex], list[(startIndex + endIndex) / 2], list[endIndex]);

            if (median == 0)
            {
                return PartitionByFirst(list, startIndex, endIndex);
            }

            if (median == 2)
            {
                return PartitionByLast(list, startIndex, endIndex);
            }

            var t1 = list[startIndex];
            list[startIndex] = list[(startIndex + endIndex) / 2];
            list[(startIndex + endIndex) / 2] = t1;

            return PartitionByFirst(list, startIndex, endIndex);
        }

        public static int PartitionByRandom(List<int> list, int startIndex, int endIndex)
        {
            var randomMedian = RandomGenerator.Next(startIndex, endIndex + 1);

            var t1 = list[startIndex];
            list[startIndex] = list[randomMedian];
            list[randomMedian] = t1;

            return PartitionByFirst(list, startIndex, endIndex);
        }

        public static int QuickSortAndCountComparisions(List<int> list, int startIndex, int endIndex, PartitionBy partitionBy)
        {
            if (endIndex <= startIndex)
            {
                return 0;
            }

            var partitionIndex = -1;
            switch (partitionBy)
            {
                case PartitionBy.First:
                    partitionIndex = PartitionByFirst(list, startIndex, endIndex);
                    break;

                case PartitionBy.Last:
                    partitionIndex = PartitionByLast(list, startIndex, endIndex);
                    break;

                case PartitionBy.Median:
                    partitionIndex = PartitionByMedian(list, startIndex, endIndex);
                    break;

                case PartitionBy.Random:
                    partitionIndex = PartitionByRandom(list, startIndex, endIndex);
                    break;
            }

            var leftComparisions = QuickSortAndCountComparisions(list, startIndex, partitionIndex - 1, partitionBy);
            var rightComparisions = QuickSortAndCountComparisions(list, partitionIndex + 1, endIndex, partitionBy);

            return leftComparisions + rightComparisions + endIndex - startIndex;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\inputq.txt");
            var inputList = new List<int>();
            while (!textReader.EndOfStream)
            {
                inputList.Add(Convert.ToInt32(textReader.ReadLine()));
            }

            var firstList = new List<int>(inputList);
            var lastList = new List<int>(inputList);
            var medianList = new List<int>(inputList);
            var randomList = new List<int>(inputList);

            var comparisionCountsByFirst = QuickSortAndCountComparisions(firstList, 0, inputList.Count - 1, PartitionBy.First);
            var comparisionCountsByLast = QuickSortAndCountComparisions(lastList, 0, inputList.Count - 1, PartitionBy.Last);
            var comparisionCountsByMedian = QuickSortAndCountComparisions(medianList, 0, inputList.Count - 1, PartitionBy.Median);
            var comparisionCountsByRandom = QuickSortAndCountComparisions(randomList, 0, inputList.Count - 1, PartitionBy.Random);

            Console.WriteLine(comparisionCountsByFirst);
            Console.WriteLine(comparisionCountsByLast);
            Console.WriteLine(comparisionCountsByMedian);
            Console.WriteLine(comparisionCountsByRandom);

            Console.ReadKey();
        }
    }
}
