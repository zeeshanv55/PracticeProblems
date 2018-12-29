namespace Ds.Stacks.Hard.PoisonousPlants
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static int poisonousPlants(int[] p)
        {
            var listOfLists = new List<List<int>>();

            listOfLists.Add(new List<int> { p[0] });
            for (var i = 1; i < p.Length; i++)
            {
                if (listOfLists.Last().Last() >= p[i])
                {
                    listOfLists.Last().Add(p[i]);
                }
                else
                {
                    listOfLists.Add(new List<int> { p[i] });
                }
            }

            var maxDays = 0;
            while (listOfLists.Count > 1)
            {
                maxDays++;

                for (var i = 1; i < listOfLists.Count; i++)
                {
                    listOfLists.ElementAt(i).RemoveAt(0);
                }

                listOfLists.RemoveAll(l => !l.Any());

                for (var i = 0; i < listOfLists.Count - 1; i++)
                {
                    if (listOfLists.ElementAt(i).Last() >= listOfLists.ElementAt(i + 1).First())
                    {
                        listOfLists.ElementAt(i).AddRange(listOfLists.ElementAt(i + 1));
                        listOfLists.RemoveAt(i + 1);
                        i--;
                    }
                }
            }

            return maxDays;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(textReader.ReadLine());
            int[] p = Array.ConvertAll(textReader.ReadLine().Split(' '), pTemp => Convert.ToInt32(pTemp));
            int result = poisonousPlants(p);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
