namespace Ds.Stacks.Hard.AndXorOr
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static int andXorOr(int[] a)
        {
            try
            {
                var xorCache = new Dictionary<Tuple<int, int>, int>();
                var arrayStack = new Stack<int>();
                var max = 0;

                for (var i = 0; i < a.Length - 1; i++)
                {
                    if (!(xorCache.ContainsKey(new Tuple<int, int>(a[i], a[i + 1])) || xorCache.ContainsKey(new Tuple<int, int>(a[i + 1], a[i]))))
                    {
                        var xorResult = a[i] ^ a[i + 1];
                        xorCache.Add(new Tuple<int, int>(a[i], a[i + 1]), xorResult);
                        if (xorResult > max)
                        {
                            max = xorResult;
                        }
                    }
                }

                for (var i = 0; i < a.Length; i++)
                {
                    while (arrayStack.Any())
                    {
                        var smallVal = arrayStack.Peek();
                        if (!(xorCache.ContainsKey(new Tuple<int, int>(a[i], smallVal)) || xorCache.ContainsKey(new Tuple<int, int>(smallVal, a[i]))))
                        {
                            var xorResult = a[i] ^ smallVal;
                            xorCache.Add(new Tuple<int, int>(a[i], smallVal), xorResult);
                            if (xorResult > max)
                            {
                                max = xorResult;
                            }
                        }

                        if (a[i] < arrayStack.Peek())
                        {
                             arrayStack.Pop();
                        }
                        else
                        {
                            break;
                        }
                    }

                    arrayStack.Push(a[i]);
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
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int aCount = Convert.ToInt32(textReader.ReadLine());

            int[] a = Array.ConvertAll(textReader.ReadLine().Split(' '), aTemp => Convert.ToInt32(aTemp));
            int result = andXorOr(a);

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
