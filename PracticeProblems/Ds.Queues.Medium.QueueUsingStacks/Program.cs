namespace Ds.Queues.Medium.QueueUsingStacks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        class QueueQuery
        {
            public int Type { get; set; }

            public int Value { get; set; }

            public QueueQuery(string input)
            {
                if (input.StartsWith("1"))
                {
                    this.Type = 1;
                    this.Value = Convert.ToInt32(input.Split(' ')[1]);
                }
                else
                {
                    this.Type = Convert.ToInt32(input);
                }
            }
        }

        class StackyQueue
        {
            public Stack<int> EnqueueStack { get; set; }

            public Stack<int> DequeueStack { get; set; }


            public StackyQueue()
            {
                this.EnqueueStack = new Stack<int>();
                this.DequeueStack = new Stack<int>();
            }

            public void Enqueue(int v)
            {
                this.EnqueueStack.Push(v);
            }

            public int? Dequeue()
            {
                if (!this.DequeueStack.Any())
                {
                    while (this.EnqueueStack.Any())
                    {
                        this.DequeueStack.Push(this.EnqueueStack.Pop());
                    }
                }

                if (this.DequeueStack.Any())
                {
                    return this.DequeueStack.Pop();
                }
                else
                {
                    return null;
                }
            }

            public int? Peek()
            {
                if (!this.DequeueStack.Any())
                {
                    while (this.EnqueueStack.Any())
                    {
                        this.DequeueStack.Push(this.EnqueueStack.Pop());
                    }
                }

                if (this.DequeueStack.Any())
                {
                    return this.DequeueStack.Peek();
                }
                else
                {
                    return null;
                }
            }
        }

        static void ProcessQueries(List<QueueQuery> queries)
        {
            var queue = new StackyQueue();

            foreach (var q in queries)
            {
                switch (q.Type)
                {
                    case 1:
                        queue.Enqueue(q.Value);
                        break;

                    case 2:
                        queue.Dequeue();
                        break;

                    case 3:
                        Console.WriteLine(queue.Peek());
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            var count = Convert.ToInt32(textReader.ReadLine());
            var queries = new List<QueueQuery>();

            for (int i = 0; i < count; i++)
            {
                queries.Add(new QueueQuery(textReader.ReadLine()));
            }

            ProcessQueries(queries);
            Console.ReadKey();
        }
    }
}
