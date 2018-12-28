namespace Ds.Stacks.Medium.GameOfTwoStacks
{
    using System;
    using System.IO;

    class Program
    {
        static long twoStacks(int x, int[] a, int[] b)
        {
            var turns = 0;
            var sum = 0;
            var i = 0;
            var j = 0;
            while (i < a.Length && sum + a[i] <= x)
            {
                sum += a[i];
                i++;
            }

            turns = i;

            while (j < b.Length && i >= 0)
            {
                sum += b[j];
                j++;
                while (sum > x && i > 0)
                {
                    i--;
                    sum -= a[i];
                }

                if (sum <= x && i + j > turns)
                {
                    turns = i + j;
                }
            }

            return turns;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int g = Convert.ToInt32(textReader.ReadLine());
            for (int gItr = 0; gItr < g; gItr++)
            {
                string[] nmx = textReader.ReadLine().Split(' ');
                int n = Convert.ToInt32(nmx[0]);
                int m = Convert.ToInt32(nmx[1]);
                int x = Convert.ToInt32(nmx[2]);
                int[] a = Array.ConvertAll(textReader.ReadLine().Split(' '), aTemp => Convert.ToInt32(aTemp));
                int[] b = Array.ConvertAll(textReader.ReadLine().Split(' '), bTemp => Convert.ToInt32(bTemp));
                var result = twoStacks(x, a, b);

                Console.WriteLine(result);
            }

            Console.ReadKey();
        }
    }
}
