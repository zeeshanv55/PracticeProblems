namespace Ds.Arrays.Medium.SparseArrays
{
    using System;
    using System.IO;
    using System.Linq;

    class Program
    {
        static int[] matchingStrings(string[] strings, string[] queries)
        {
            var returnArray = new int[queries.Length];
            for (var i = 0; i < queries.Length; i++)
            {
                returnArray[i] = strings.Where(s => s == queries[i]).Count();
            }

            return returnArray;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int stringsCount = Convert.ToInt32(textReader.ReadLine());
            string[] strings = new string[stringsCount];

            for (int i = 0; i < stringsCount; i++)
            {
                string stringsItem = textReader.ReadLine();
                strings[i] = stringsItem;
            }

            int queriesCount = Convert.ToInt32(textReader.ReadLine());
            string[] queries = new string[queriesCount];
            for (int i = 0; i < queriesCount; i++)
            {
                string queriesItem = textReader.ReadLine();
                queries[i] = queriesItem;
            }

            int[] res = matchingStrings(strings, queries);
            Console.WriteLine(string.Join("\n", res));
            Console.ReadKey();
        }
    }
}