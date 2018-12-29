namespace Ds.Queues.Hard.TruckTour
{
    using System;
    using System.IO;

    class Program
    {
        static int truckTour(int[][] petrolPumps)
        {
            var i = 0;
            for (; i < petrolPumps.Length; i++)
            {
                if (petrolPumps[i][0] < petrolPumps[i][1])
                {
                    continue;
                }
                else
                {
                    var tank = 0;
                    for (var j = i; j < i + petrolPumps.Length ; j++)
                    {
                        tank += petrolPumps[j % petrolPumps.Length][0];
                        tank -= petrolPumps[j % petrolPumps.Length][1];
                        if (tank < 0)
                        {
                            break;
                        }
                    }

                    if (tank >= 0)
                    {
                        break;
                    }
                }
            }

            return i;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(textReader.ReadLine());
            int[][] petrolpumps = new int[n][];

            for (int petrolpumpsRowItr = 0; petrolpumpsRowItr < n; petrolpumpsRowItr++)
            {
                petrolpumps[petrolpumpsRowItr] = Array.ConvertAll(textReader.ReadLine().Split(' '), petrolpumpsTemp => Convert.ToInt32(petrolpumpsTemp));
            }

            int result = truckTour(petrolpumps);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
