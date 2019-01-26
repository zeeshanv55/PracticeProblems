using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Recursion.Easy.CountInversions
{
    class Program
    {
        public static Tuple<List<int>, double> MergeAndInversionCounts(List<int> leftList, List<int> rightList)
        {
            var i = 0;
            var j = 0;
            var returnList = new List<int>();
            var inversionCounts = (double)0;

            while (i < leftList.Count && j < rightList.Count)
            {
                if (leftList[i] <= rightList[j])
                {
                    returnList.Add(leftList[i]);
                    i++;
                }
                else
                {
                    returnList.Add(rightList[j]);
                    inversionCounts += (leftList.Count - i);
                    j++;
                }
            }

            if (i < leftList.Count)
            {
                returnList.AddRange(leftList.GetRange(i, leftList.Count - i));
            }

            if (j < rightList.Count)
            {
                returnList.AddRange(rightList.GetRange(j, rightList.Count - j));
            }

            return new Tuple<List<int>, double>(returnList, inversionCounts);
        }

        public static Tuple<List<int>, double> MergeSortAndCountInversions(List<int> list)
        {
            if (list.Count == 1)
            {
                return new Tuple<List<int>, double>(new List<int> { list[0] }, 0);
            }

            if (list.Count == 0)
            {
                return new Tuple<List<int>, double>(new List<int>(), 0);
            }

            var leftUnsortedList = new List<int>();
            var rightUnsortedList = new List<int>();
            leftUnsortedList.AddRange(list.GetRange(0, list.Count / 2));
            rightUnsortedList.AddRange(list.GetRange(list.Count / 2, (int)Math.Ceiling((list.Count / 2.0))));

            var leftListAndInversionCounts = MergeSortAndCountInversions(leftUnsortedList);
            var rightListAndInversionCounts = MergeSortAndCountInversions(rightUnsortedList);
            var sortedListAndInversionCounts = MergeAndInversionCounts(leftListAndInversionCounts.Item1, rightListAndInversionCounts.Item1);

            return new Tuple<List<int>, double>(sortedListAndInversionCounts.Item1, leftListAndInversionCounts.Item2 + rightListAndInversionCounts.Item2 + sortedListAndInversionCounts.Item2);
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            var inputList = new List<int>();
            while (!textReader.EndOfStream)
            {
                inputList.Add(Convert.ToInt32(textReader.ReadLine()));
            }

            var sortedListAndInversionCounts = MergeSortAndCountInversions(inputList);

            Console.WriteLine(sortedListAndInversionCounts.Item2);
            Console.ReadKey();
        }
    }
}
