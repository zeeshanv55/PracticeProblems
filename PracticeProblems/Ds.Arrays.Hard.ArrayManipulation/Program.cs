namespace Ds.Arrays.Hard.ArrayManipulation
{
    using System;
    using System.IO;

    class Program
    {
        static long arrayManipulation(int n, int[][] queries)
        {
            try
            {
                var sumArray = new long[n];

                for (var j = 0; j < queries.Length; j++)
                {
                    if (queries[j][2] > 0)
                    {
                        sumArray[queries[j][0] - 1] += queries[j][2];
                        if (queries[j][1] < n)
                        {
                            sumArray[queries[j][1]] -= queries[j][2];
                        }
                    }
                }

                var max = sumArray[0];
                for (var i = 1; i < n; i++)
                {
                    sumArray[i] = sumArray[i - 1] + sumArray[i];
                    if (sumArray[i] > max)
                    {
                        max = sumArray[i];
                    }
                }

                return max;
            }
            catch
            {
                return 0;
            }
        }

        static void Main(string[] args)
        {
            var inputStream = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            string[] nm = inputStream.ReadLine().Split(' ');
            int n = Convert.ToInt32(nm[0]);
            int m = Convert.ToInt32(nm[1]);
            int[][] queries = new int[m][];

            for (int i = 0; i < m; i++)
            {
                queries[i] = Array.ConvertAll(inputStream.ReadLine().Split(' '), queriesTemp => Convert.ToInt32(queriesTemp));
            }

            long result = arrayManipulation(n, queries);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}