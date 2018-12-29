namespace Ds.Stacks.Medium.Waiter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static Stack<int>[] A { get; set; }

        static Stack<int>[] B { get; set; }

        static int?[] Primes { get; set; }

        static int GetPrime(int i)
        {
            if (Primes[i] != null)
            {
                return (int)Primes[i];
            }
            else
            {
                var n = Primes[i - 1] + 1;
                while (true)
                {
                    var divisible = false;
                    for (var j = 1; j < i; j++)
                    {
                        if (n % Primes[j] == 0)
                        {
                            divisible = true;
                            break;
                        }
                    }

                    if (!divisible)
                    {
                        Primes[i] = n;
                        break;
                    }

                    n++;
                }

                return (int)n;
            }
        }

        static void waiter(int[] A0, int Q)
        {
            A = new Stack<int>[1201];
            B = new Stack<int>[1201];
            Primes = new int?[10001];

            var a0 = new Stack<int>();
            foreach (var e in A0)
            {
                a0.Push(e);
            }

            A[0] = a0;
            Primes[0] = 0;
            Primes[1] = 2;

            for (var i = 1; i <= Q; i++)
            {
                var prime = GetPrime(i);
                var Ai1 = A[i - 1];
                var Ai = new Stack<int>();
                var Bi = new Stack<int>();
                while(Ai1.Any())
                {
                    var ai1 = Ai1.Pop();
                    if (ai1 % prime == 0)
                    {
                        Bi.Push(ai1);
                    }
                    else
                    {
                        Ai.Push(ai1);
                    }
                }

                A[i] = Ai;
                B[i] = Bi;
            }

            for (var i = 1; i <= Q; i++)
            {
                while (B[i].Any())
                {
                    Console.WriteLine(B[i].Pop());
                }
            }

            while (A[Q].Any())
            {
                Console.WriteLine(A[Q].Pop());
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input11.txt");
            string[] nq = textReader.ReadLine().Split(' ');

            int n = Convert.ToInt32(nq[0]);
            int q = Convert.ToInt32(nq[1]);
            int[] number = Array.ConvertAll(textReader.ReadLine().Split(' '), numberTemp => Convert.ToInt32(numberTemp));
            waiter(number, q);
            Console.ReadKey();
        }
    }
}
