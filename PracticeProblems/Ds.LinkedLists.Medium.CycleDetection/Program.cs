namespace Ds.LinkedLists.Medium.CycleDetection
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        class SinglyLinkedListNode
        {
            public int data;
            public SinglyLinkedListNode next;

            public SinglyLinkedListNode(int nodeData)
            {
                this.data = nodeData;
                this.next = null;
            }
        }

        class SinglyLinkedList
        {
            public SinglyLinkedListNode head;
            public SinglyLinkedListNode tail;

            public SinglyLinkedList()
            {
                this.head = null;
                this.tail = null;
            }

            public void InsertNode(int nodeData)
            {
                SinglyLinkedListNode node = new SinglyLinkedListNode(nodeData);

                if (this.head == null)
                {
                    this.head = node;
                }
                else
                {
                    this.tail.next = node;
                }

                this.tail = node;
            }
        }

        static void PrintSinglyLinkedList(SinglyLinkedListNode node, string sep)
        {
            while (node != null)
            {
                Console.Write(node.data);

                node = node.next;

                if (node != null)
                {
                    Console.Write(sep);
                }
            }
        }

        static bool hasCycle(SinglyLinkedListNode node)
        {
            if (node == null)
            {
                return false;
            }

            var parsedDictionary = new Dictionary<SinglyLinkedListNode, bool>();

            while (node != null)
            {
                if (!parsedDictionary.ContainsKey(node))
                {
                    parsedDictionary.Add(node, true);
                    node = node.next;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int tests = Convert.ToInt32(textReader.ReadLine());

            for (int testsItr = 0; testsItr < tests; testsItr++)
            {
                int index = Convert.ToInt32(textReader.ReadLine());
                SinglyLinkedList llist = new SinglyLinkedList();
                int llistCount = Convert.ToInt32(textReader.ReadLine());

                for (int i = 0; i < llistCount; i++)
                {
                    int llistItem = Convert.ToInt32(textReader.ReadLine());
                    llist.InsertNode(llistItem);
                }

                SinglyLinkedListNode extra = new SinglyLinkedListNode(-1);
                SinglyLinkedListNode temp = llist.head;

                for (int i = 0; i < llistCount; i++)
                {
                    if (i == index)
                    {
                        extra = temp;
                    }

                    if (i != llistCount - 1)
                    {
                        temp = temp.next;
                    }
                }

                temp.next = extra;
                bool result = hasCycle(llist.head);
                Console.WriteLine((result ? 1 : 0));
            }

            Console.ReadKey();
        }
    }
}
