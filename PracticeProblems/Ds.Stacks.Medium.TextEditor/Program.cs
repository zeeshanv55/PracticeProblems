namespace Ds.Stacks.Medium.TextEditor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        class TextOperation
        {
            public int OpType { get; set; }

            public string Args { get; set; }

            public TextOperation(string[] args)
            {
                this.OpType = Convert.ToInt32(args[0]);

                if (args.Length > 1)
                {
                    this.Args = args[1];
                }
            }
        }

        static string EditorText { get; set; }

        static Stack<Tuple<string, int>> HistoryStack { get; set; }

        static void ProcessText(List<TextOperation> textOps)
        {
            HistoryStack = new Stack<Tuple<string, int>>();

            foreach (var textOp in textOps)
            {
                switch (textOp.OpType)
                {
                    case 1:
                        EditorText = $"{EditorText}{textOp.Args}";
                        HistoryStack.Push(new Tuple<string, int>(textOp.Args, 1));
                        break;

                    case 2:
                        var removeLength = Convert.ToInt32(textOp.Args);
                        var stringToRemove = EditorText.Substring(EditorText.Length - removeLength, removeLength);
                        HistoryStack.Push(new Tuple<string, int>(stringToRemove, 2));
                        EditorText = EditorText.Remove(EditorText.Length - removeLength, removeLength);
                        break;

                    case 3:
                        Console.WriteLine(EditorText.ElementAt(Convert.ToInt32(textOp.Args) - 1));
                        break;

                    case 4:
                        var lastOp = HistoryStack.Pop();
                        switch (lastOp.Item2)
                        {
                            case 1:
                                EditorText = EditorText.Remove(EditorText.Length - lastOp.Item1.Length, lastOp.Item1.Length);
                                break;

                            case 2:
                                EditorText = $"{EditorText}{lastOp.Item1}";
                                break;
                        }

                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(textReader.ReadLine());
            var textOps = new List<TextOperation>(n);
            for (var i = 0; i < n; i++)
            {
                textOps.Add(new TextOperation(Array.ConvertAll(textReader.ReadLine().Split(' '), a => Convert.ToString(a))));
            }

            ProcessText(textOps);
            Console.ReadKey();
        }
    }
}
