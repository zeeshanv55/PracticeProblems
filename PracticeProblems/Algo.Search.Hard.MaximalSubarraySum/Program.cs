using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Solution
{
    static Dictionary<Tuple<int, int>, long> DpDictionary;

    static long maximumSubarray(long[] a, int startIndex, int endIndex, long m)
    {
        if (startIndex == endIndex)
        {
            return a[startIndex] % m;
        }
        else
        {
            var dpKeyFull = new Tuple<int, int>(startIndex, endIndex);
            if (!DpDictionary.ContainsKey(dpKeyFull))
            {
                var sum = a[startIndex];
                for (var i = startIndex + 1; i <= endIndex; i++)
                {
                    sum += a[i];
                }

                DpDictionary.Add(dpKeyFull, sum % m);
            }

            var max = DpDictionary[dpKeyFull];

            for (var i = startIndex; i < endIndex; i++)
            {
                var leftMax = (long)-1;
                var dpKeyLeft = new Tuple<int, int>(startIndex, i);

                if (!DpDictionary.ContainsKey(dpKeyLeft))
                {
                    leftMax = maximumSubarray(a, startIndex, i, m);
                    if (!DpDictionary.ContainsKey(dpKeyLeft))
                    {
                        DpDictionary.Add(dpKeyLeft, leftMax);
                    }
                }
                else
                {
                    leftMax = DpDictionary[dpKeyLeft];
                }

                if (leftMax > max)
                {
                    max = leftMax;
                }

                var rightMax = (long)-1;
                var dpKeyRight = new Tuple<int, int>(i + 1, endIndex);

                if (!DpDictionary.ContainsKey(dpKeyRight))
                {
                    rightMax = maximumSubarray(a, i + 1, endIndex, m);
                    if (!DpDictionary.ContainsKey(dpKeyRight))
                    { 
                        DpDictionary.Add(dpKeyRight, rightMax);
                    }
                }
                else
                {
                    rightMax = DpDictionary[dpKeyRight];
                }

                if (rightMax > max)
                {
                    max = rightMax;
                }
            }

            return max;
        }
    }

    static long maximumSum(long[] a, long m)
    {
        DpDictionary = new Dictionary<Tuple<int, int>, long>();
        return maximumSubarray(a, 0, a.Length - 1, m);
    }

    static void Main(string[] args)
    {
        var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
        int q = Convert.ToInt32(textReader.ReadLine());
        for (int qItr = 0; qItr < q; qItr++)
        {
            string[] nm = textReader.ReadLine().Split(' ');
            int n = Convert.ToInt32(nm[0]);
            long m = Convert.ToInt64(nm[1]);
            long[] a = Array.ConvertAll(textReader.ReadLine().Split(' '), aTemp => Convert.ToInt64(aTemp));
            long result = maximumSum(a, m);

            Console.WriteLine(result);
        }

        Console.ReadKey();
    }
}
