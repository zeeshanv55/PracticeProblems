namespace Ds.Arrays.Easy.MaximalSubArray
{
    using System;
    using System.IO;

    class Program
    {
        static long maximalSubArray(int[] A)
        {
            var maxTemp = 0;
            var max = 0;

            for (var i = 0; i < A.Length; i++)
            {
                maxTemp += A[i];

                if (maxTemp < 0)
                {
                    maxTemp = 0;
                }
                else
                {
                    if (maxTemp > max)
                    {
                        max = maxTemp;
                    }
                }
            }

            return max;
        }

        static void Main(string[] args)
        {
            var inputStream = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(inputStream.ReadLine());
            var a = Array.ConvertAll(inputStream.ReadLine().Split(' '), q => Convert.ToInt32(q));

            long result = maximalSubArray(a);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
