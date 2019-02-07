namespace Algo.Greedy.Easy.Scheduling
{
    using System;
    using System.IO;

    class Program
    {
        static Random RandomGenerator = new Random();

        static int[] Weights { get; set; }

        static int[] Lengths { get; set; }

        static int[] CompletionTimes { get; set; }

        public static int PartitionByDifference(int startIndex, int endIndex)
        {
            var randomMedian = RandomGenerator.Next(startIndex, endIndex + 1);

            var t1 = Weights[startIndex];
            Weights[startIndex] = Weights[randomMedian];
            Weights[randomMedian] = t1;

            var t2 = Lengths[startIndex];
            Lengths[startIndex] = Lengths[randomMedian];
            Lengths[randomMedian] = t2;

            var i = startIndex + 1;
            var j = startIndex + 1;

            while (i <= endIndex && j <= endIndex)
            {
                if (((Weights[j] - Lengths[j]) > (Weights[startIndex] - Lengths[startIndex])) || (((Weights[j] - Lengths[j]) == (Weights[startIndex] - Lengths[startIndex])) && (Weights[j] > Weights[startIndex])))
                {
                    t1 = Weights[i];
                    Weights[i] = Weights[j];
                    Weights[j] = t1;

                    t2 = Lengths[i];
                    Lengths[i] = Lengths[j];
                    Lengths[j] = t2;

                    i++;
                }

                j++;
            }

            t1 = Weights[i - 1];
            Weights[i - 1] = Weights[startIndex];
            Weights[startIndex] = t1;

            t2 = Lengths[i - 1];
            Lengths[i - 1] = Lengths[startIndex];
            Lengths[startIndex] = t2;

            return i - 1;
        }

        public static int PartitionByRatio(int startIndex, int endIndex)
        {
            var randomMedian = RandomGenerator.Next(startIndex, endIndex + 1);

            var t1 = Weights[startIndex];
            Weights[startIndex] = Weights[randomMedian];
            Weights[randomMedian] = t1;

            var t2 = Lengths[startIndex];
            Lengths[startIndex] = Lengths[randomMedian];
            Lengths[randomMedian] = t2;

            var i = startIndex + 1;
            var j = startIndex + 1;

            while (i <= endIndex && j <= endIndex)
            {
                if (((double)Weights[j]/(double)Lengths[j]) > ((double)Weights[startIndex]/(double)Lengths[startIndex]))
                {
                    t1 = Weights[i];
                    Weights[i] = Weights[j];
                    Weights[j] = t1;

                    t2 = Lengths[i];
                    Lengths[i] = Lengths[j];
                    Lengths[j] = t2;

                    i++;
                }

                j++;
            }

            t1 = Weights[i - 1];
            Weights[i - 1] = Weights[startIndex];
            Weights[startIndex] = t1;

            t2 = Lengths[i - 1];
            Lengths[i - 1] = Lengths[startIndex];
            Lengths[startIndex] = t2;

            return i - 1;
        }

        static void QuickSortJobs(int startIndex, int endIndex, bool partitionByDifference)
        {
            if (endIndex <= startIndex)
            {
                return;
            }

            var partitionIndex = -1;
            if (partitionByDifference)
            {
                partitionIndex = PartitionByDifference(startIndex, endIndex);
            }
            else
            {
                partitionIndex = PartitionByRatio(startIndex, endIndex);
            }

            QuickSortJobs(startIndex, partitionIndex - 1, partitionByDifference);
            QuickSortJobs(partitionIndex + 1, endIndex, partitionByDifference);
        }

        static void CalculateCompletionTimes(int count)
        {
            CompletionTimes = new int[count];
            CompletionTimes[0] = Lengths[0];
            for (var j = 1; j < count; j++)
            {
                CompletionTimes[j] = CompletionTimes[j - 1] + Lengths[j];
            }
        }

        static double GetWeightedCompletionTimeSum(int count)
        {
            var sum = (double)0;
            for (var j = 0; j < count; j++)
            {
                sum += (double)Weights[j] * (double)CompletionTimes[j];
            }

            return sum;
        }

        static void Main(string[] args)
        {
            var inputStream = new StreamReader(@"input.txt");
            var count = Convert.ToInt32(inputStream.ReadLine().Trim());
            Weights = new int[count];
            Lengths = new int[count];

            var i = 0;
            while (!inputStream.EndOfStream)
            {
                var line = inputStream.ReadLine().Trim().Split(' ');
                Weights[i] = Convert.ToInt32(line[0]);
                Lengths[i] = Convert.ToInt32(line[1]);
                i++;
            }

            QuickSortJobs(0, count - 1, true);
            CalculateCompletionTimes(count);
            Console.WriteLine(GetWeightedCompletionTimeSum(count));

            QuickSortJobs(0, count - 1, false);
            CalculateCompletionTimes(count);
            Console.WriteLine(GetWeightedCompletionTimeSum(count));

            Console.ReadKey();
        }
    }
}
