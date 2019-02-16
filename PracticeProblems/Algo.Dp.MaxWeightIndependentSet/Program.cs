namespace Algo.Dp.MaxWeightIndependentSet
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static int N { get; set; }

        static double[] MaxWeightsByLastIndex { get; set; }

        static HashSet<int>[] MaxWeightIndependentSetsByLastIndex { get; set; }

        public static void MaxWeightIndependentSet(List<double> weights, int lastIndex)
        {
            if (lastIndex < 0)
            {
                return;
            }

            if (MaxWeightsByLastIndex[lastIndex] > -1)
            {
                return;
            }

            if (lastIndex == 0)
            {
                MaxWeightsByLastIndex[0] = weights[0];
                MaxWeightIndependentSetsByLastIndex[0].Add(0);
                return;
            }

            if (lastIndex == 1)
            {
                if (weights[1] > weights[0])
                {
                    MaxWeightsByLastIndex[1] = weights[1];
                    MaxWeightIndependentSetsByLastIndex[1].Add(1);
                }
                else
                {
                    MaxWeightsByLastIndex[1] = weights[0];
                    MaxWeightIndependentSetsByLastIndex[1].Add(0);
                }

                return;
            }

            MaxWeightIndependentSet(weights, lastIndex - 2);
            MaxWeightIndependentSet(weights, lastIndex - 1);

            if (MaxWeightsByLastIndex[lastIndex - 1] > MaxWeightsByLastIndex[lastIndex - 2] + weights[lastIndex])
            {
                MaxWeightsByLastIndex[lastIndex] = MaxWeightsByLastIndex[lastIndex - 1];
                MaxWeightIndependentSetsByLastIndex[lastIndex] = new HashSet<int>(MaxWeightIndependentSetsByLastIndex[lastIndex - 1]);
            }
            else
            {
                MaxWeightsByLastIndex[lastIndex] = MaxWeightsByLastIndex[lastIndex - 2] + weights[lastIndex];
                MaxWeightIndependentSetsByLastIndex[lastIndex] = new HashSet<int>(MaxWeightIndependentSetsByLastIndex[lastIndex - 2]);
                MaxWeightIndependentSetsByLastIndex[lastIndex].Add(lastIndex);
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            N = Convert.ToInt32(textReader.ReadLine().Trim());
            var weights = new List<double>();

            while (!textReader.EndOfStream)
            {
                weights.Add(Convert.ToDouble(textReader.ReadLine().Trim()));
            }

            MaxWeightsByLastIndex = new double[N];
            MaxWeightIndependentSetsByLastIndex = new HashSet<int>[N];
            for (var i = 0; i < N; i++)
            {
                MaxWeightsByLastIndex[i] = -1;
                MaxWeightIndependentSetsByLastIndex[i] = new HashSet<int>();
            }

            MaxWeightIndependentSet(weights, N - 1);

            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(0) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(1) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(2) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(3) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(16) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(116) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(516) ? "1" : "0");
            Console.Write(MaxWeightIndependentSetsByLastIndex[N - 1].Contains(996) ? "1" : "0");

            Console.ReadKey();
        }
    }
}
