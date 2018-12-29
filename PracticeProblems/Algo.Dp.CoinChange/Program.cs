using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Dp.CoinChange
{
    class Program
    {
        static Dictionary<Tuple<int, long>, double> dpCache { get; set; }

        static double count(long[] C, int M, long N)
        {
            if (N == 0)
                return 1;

            if (N < 0)
                return 0;

            if (M <= 0 && N >= 1)
                return 0;

            var result1 = (double)0;
            var result2 = (double)0;

            if (!dpCache.ContainsKey(new Tuple<int, long>(M - 1, N)))
            {
                result1 = count(C, M - 1, N);
                dpCache.Add(new Tuple<int, long>(M - 1, N), result1);
            }
            else
            {
                result1 = dpCache[new Tuple<int, long>(M - 1, N)];
            }

            if (!dpCache.ContainsKey(new Tuple<int, long>(M, N - C[M - 1])))
            {
                result2 = count(C, M, N - C[M - 1]);
                dpCache.Add(new Tuple<int, long>(M, N - C[M - 1]), result2);
            }
            else
            {
                result2 = dpCache[new Tuple<int, long>(M, N - C[M - 1])];
            }

            return result1 + result2;
        }

        static double getWays(int N, long[] C)
        {
            dpCache = new Dictionary<Tuple<int, long>, double>();
            return count(C, C.Length, N);
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            string[] nm = textReader.ReadLine().Split(' ');
            int n = Convert.ToInt32(nm[0]);
            int m = Convert.ToInt32(nm[1]);
            long[] c = Array.ConvertAll(textReader.ReadLine().Split(' '), cTemp => Convert.ToInt64(cTemp));
            var ways = getWays(n, c);
            Console.Write(ways);
            Console.ReadKey();
        }
    }
}
