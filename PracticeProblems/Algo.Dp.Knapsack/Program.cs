namespace Algo.Dp.Knapsack
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static double W { get; set; }

        static int N { get; set; }

        static Dictionary<Tuple<int, double>, double> OptimumSums { get; set; }

        static Dictionary<Tuple<int, double>, HashSet<int>> OptimumSumIndices { get; set; }

        static HashSet<int> Knapsack(double[] values, double[] weights, int i, double w)
        {
            if (i < 0 || w <= 0)
            {
                return new HashSet<int>();
            }

            var dpKey = new Tuple<int, double>(i, w);
            if (OptimumSumIndices.ContainsKey(dpKey))
            {
                return OptimumSumIndices[dpKey];
            }
            else
            {
                if (i == 0)
                {
                    if (weights[0] <= w)
                    {
                        OptimumSumIndices.Add(dpKey, new HashSet<int> { 0 });
                        OptimumSums.Add(dpKey, values[0]);
                        return new HashSet<int> { 0 };
                    }
                    else
                    {
                        OptimumSumIndices.Add(dpKey, new HashSet<int>());
                        OptimumSums.Add(dpKey, 0);
                        return new HashSet<int>();
                    }
                }
                else
                {
                    var excludingDpKey = new Tuple<int, double>(i - 1, w);
                    var includingDpKey = new Tuple<int, double>(i - 1, w - weights[i]);

                    var excludingCurrent = Knapsack(values, weights, i - 1, w);
                    var includingCurrent = Knapsack(values, weights, i - 1, w - weights[i]);
                    var excludingSum = OptimumSums.ContainsKey(excludingDpKey) ? OptimumSums[excludingDpKey] : double.MinValue;
                    var includingSum = OptimumSums.ContainsKey(includingDpKey) ? OptimumSums[includingDpKey] + values[i] : double.MinValue;

                    if (excludingSum > includingSum)
                    {
                        OptimumSumIndices.Add(dpKey, excludingCurrent);
                        OptimumSums.Add(dpKey, excludingSum);
                        return excludingCurrent;
                    }
                    else
                    {
                        includingCurrent.Add(i);
                        OptimumSumIndices.Add(dpKey, includingCurrent);
                        OptimumSums.Add(dpKey, includingSum);
                        return includingCurrent;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            var line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            W = Convert.ToDouble(line[0]);
            N = Convert.ToInt32(line[1]);
            var values = new double[N];
            var weights = new double[N];
            OptimumSums = new Dictionary<Tuple<int, double>, double>();
            OptimumSumIndices = new Dictionary<Tuple<int, double>, HashSet<int>>();

            var i = 0;
            while (!textReader.EndOfStream)
            {
                line = textReader.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                values[i] = Convert.ToDouble(line[0]);
                weights[i] = Convert.ToDouble(line[1]);
                i++;
            }

            var indices = Knapsack(values, weights, N - 1, W);
            Console.WriteLine(OptimumSums[new Tuple<int, double>(N - 1, W)]);
            Console.ReadKey();
        }
    }
}
