namespace Ds.Stacks.Medium.LargestRectangle
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static long largestRectangle(int[] h)
        {
            var maxArea = 0;
            var hStack = new Stack<Tuple<int, int>>();
            hStack.Push(new Tuple<int, int>(h[0], 0));
            for (var i = 1; i < h.Length; i++)
            {
                if (h[i] >= h[i - 1])
                {
                    hStack.Push(new Tuple<int, int>(h[i], i));
                }
                else
                {
                    int newMin = -1;
                    while (hStack.Any() && hStack.Peek().Item1 >= h[i])
                    {
                        var x = hStack.Pop();
                        newMin = x.Item2;
                        if (x.Item1 * (i - x.Item2) > maxArea)
                        {
                            maxArea = x.Item1 * (i - x.Item2);
                        }
                    }

                    hStack.Push(new Tuple<int, int>(h[i], newMin == -1 ? i : newMin));
                }
            }

            while (hStack.Any())
            {
                var v = hStack.Pop();
                if (v.Item1 * (h.Length - v.Item2) > maxArea)
                {
                    maxArea = v.Item1 * (h.Length - v.Item2);
                }
            }

            return maxArea;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(textReader.ReadLine());
            int[] h = Array.ConvertAll(textReader.ReadLine().Split(' '), hTemp => Convert.ToInt32(hTemp));
            long result = largestRectangle(h);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
