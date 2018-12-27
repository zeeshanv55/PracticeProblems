namespace Ds.Stacks.Medium.BalancedBrackets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static string isBalanced(string s)
        {
            var stack = new Stack<char>();
            foreach (var c in s.ToCharArray())
            {
                if (!stack.Any())
                {
                    stack.Push(c);
                }
                else
                {
                    var stackTop = stack.Peek();
                    var toPop = false;
                    switch (c)
                    {
                        case ')':
                            toPop = stackTop == '(';
                            break;

                        case ']':
                            toPop = stackTop == '[';
                            break;

                        case '}':
                            toPop = stackTop == '{';
                            break;
                    }

                    if (toPop)
                    {
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(c);
                    }
                }
            }

            return stack.Any() ? "NO" : "YES";
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int t = Convert.ToInt32(textReader.ReadLine());

            for (int tItr = 0; tItr < t; tItr++)
            {
                string s = textReader.ReadLine();
                string result = isBalanced(s);
                Console.WriteLine(result);
            }

            Console.ReadKey();
        }
    }
}
