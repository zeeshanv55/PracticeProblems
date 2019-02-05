using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Hashsets.Easy.TwoSumProblem
{
    class Program
    {
        static Dictionary<double, int> InputSetWithSortedIndices;

        static List<double> InputList;

        static double[] InputArray;

        const int TLOW = -10000;

        const int THIGH = 10000;

        static int NearestHigher(int low, int high, double a)
        {
            if (low >= high)
            {
                return low + 1 > InputArray.Length - 1 ? InputArray.Length - 1 : low + 1;
            }

            var median = (low + high) / 2;
            if (InputArray[median] == a)
            {
                return median;
            }
            else
            {
                if (InputArray[median] > a)
                {
                    return NearestHigher(low, median - 1, a);
                }
                else
                {
                    return NearestHigher(median + 1, high, a);
                }
            }
        }

        static int NearestLower(int low, int high, double a)
        {
            if (low >= high)
            {
                return high - 1 < 0 ? 0 : high - 1;
            }

            var median = (low + high) / 2;
            if (InputArray[median] == a)
            {
                return median;
            }
            else
            {
                if (InputArray[median] > a)
                {
                    return NearestLower(low, median - 1, a);
                }
                else
                {
                    return NearestLower(median + 1, high, a);
                }
            }
        }

        static void Main(string[] args)
        {
            InputSetWithSortedIndices = new Dictionary<double, int>();
            InputList = new List<double>();
            var inputSet = new HashSet<double>();

            var textReader = new StreamReader(@"input.txt");
            while (!textReader.EndOfStream)
            {
                var val = Convert.ToDouble(textReader.ReadLine().Trim());
                if (!inputSet.Contains(val))
                {
                    InputList.Add(val);
                    inputSet.Add(val);
                }
            }

            InputList.Sort();
            for (var i = 0; i < InputList.Count; i++)
            {
                InputSetWithSortedIndices.Add(InputList[i], i);
            }

            InputArray = InputList.ToArray();

            var counter = 0;
            var lowestInput = InputArray[0];
            var highestInput = InputArray[InputList.Count - 1];
            var distinctElementSet = new HashSet<Tuple<double, double>>();
            var distinctSumSet = new HashSet<double>();

            for (var i = 0; i < InputArray.Length; i++)
            {
                var x = InputArray[i];
                var yLow = TLOW - x;
                var yHigh = THIGH - x;

                var yLowIndex = NearestLower(0, InputArray.Length - 1, yLow);
                var yHighIndex = NearestHigher(0, InputArray.Length - 1, yHigh);

                for (var j = yLowIndex; j <= yHighIndex; j++)
                {
                    var t = x + InputArray[j];
                    if (t <= THIGH && t >= TLOW)
                    {
                        if (!distinctSumSet.Contains(t))
                        {
                            distinctSumSet.Add(t);
                            counter++;
                        }
                    }
                }
            }

            Console.WriteLine(counter);
            Console.ReadKey();
        }
    }
}
