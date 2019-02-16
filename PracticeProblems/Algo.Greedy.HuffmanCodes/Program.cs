namespace Algo.Greedy.HuffmanCodes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    abstract class Heapable : IComparable<Heapable>
    {
        abstract public int CompareTo(Heapable other);
    }

    class HuffmanNode : Heapable
    {
        public HashSet<int> Indices { get; set; }

        public double Frequency { get; set; }

        public HuffmanNode(HashSet<int> indices, double frequency)
        {
            this.Indices = new HashSet<int>(indices);
            this.Frequency = frequency;
        }

        public override int CompareTo(Heapable obj)
        {
            if (this.Frequency - ((HuffmanNode)obj).Frequency < 0)
            {
                return -1;
            }

            if (this.Frequency - ((HuffmanNode)obj).Frequency > 0)
            {
                return 1;
            }

            return 0;
        }
    }

    class Heap<T> where T : Heapable
    {
        public List<T> A { get; set; }

        public Heap()
        {
            this.A = new List<T>();
        }

        public Heap(List<T> a)
        {
            this.A = new List<T>();
            foreach (var element in a)
            {
                this.Insert(element);
            }
        }

        public void Insert(T x)
        {
            A.Add(x);
            this.BubbleUp(A.Count - 1);
        }

        public bool Exists(T x, int rootIndex = 0)
        {
            if (rootIndex >= this.A.Count || rootIndex < 0)
            {
                return false;
            }

            if (x.CompareTo(this.A[rootIndex]) == 0)
            {
                return true;
            }
            else
            {
                return Exists(x, 2 * rootIndex + 1) || Exists(x, 2 * rootIndex + 2);
            }
        }

        public bool Remove(T x, int rootIndex = 0)
        {
            if (rootIndex >= this.A.Count || rootIndex < 0)
            {
                return false;
            }

            if (x.CompareTo(this.A[rootIndex]) == 0)
            {
                this.A[rootIndex] = this.A[this.A.Count - 1];
                this.A.RemoveAt(this.A.Count - 1);

                if (rootIndex < this.A.Count)
                {
                    if (((rootIndex + 1) / 2) - 1 > 0 && A[rootIndex].CompareTo(A[((rootIndex + 1) / 2) - 1]) < 0)
                    {
                        this.BubbleUp(rootIndex);
                    }
                    else
                    {
                        this.DrownDown(rootIndex);
                    }
                }

                return true;
            }
            else
            {
                return Remove(x, 2 * rootIndex + 1) || Remove(x, 2 * rootIndex + 2);
            }
        }

        public T RemoveMinimum()
        {
            if (this.A.Any())
            {
                var r = this.A[0];
                this.A[0] = this.A[this.A.Count - 1];
                this.A.RemoveAt(this.A.Count - 1);

                this.DrownDown(0);

                return r;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void DrownDown(int i)
        {
            while (true)
            {
                if (2 * i + 1 >= this.A.Count) //No child nodes
                {
                    break;
                }
                else
                {
                    if (2 * i == this.A.Count - 2) //One child node
                    {
                        if (this.A[i].CompareTo(this.A[2 * i + 1]) <= 0) //Heap conserved, can break
                        {
                            break;
                        }
                        else //Drown down
                        {
                            var t = this.A[i];
                            this.A[i] = this.A[2 * i + 1];
                            this.A[2 * i + 1] = t;
                            i = 2 * i + 1;
                        }
                    }
                    else //Two child nodes
                    {
                        if (this.A[i].CompareTo(this.A[2 * i + 1]) <= 0 && this.A[i].CompareTo(this.A[2 * i + 2]) <= 0) //Heap conserved, can break
                        {
                            break;
                        }
                        else //Drown down
                        {
                            var j = -1;
                            if (this.A[2 * i + 1].CompareTo(this.A[2 * i + 2]) < 0)
                            {
                                j = 2 * i + 1;
                            }
                            else
                            {
                                j = 2 * i + 2;
                            }

                            var t = this.A[i];
                            this.A[i] = this.A[j];
                            this.A[j] = t;
                            i = j;
                        }
                    }
                }
            }
        }

        private void BubbleUp(int i)
        {
            while (i > 0 && this.A[((i + 1) / 2) - 1].CompareTo(A[i]) > 0)
            {
                var t = this.A[((i + 1) / 2) - 1];
                this.A[((i + 1) / 2) - 1] = this.A[i];
                this.A[i] = t;
                i = ((i + 1) / 2) - 1;
            }
        }
    }

    class Program
    {
        static int N { get; set; }

        static string[] ProcessHuffman(List<HuffmanNode> weights)
        {
            var huffmanHeap = new Heap<HuffmanNode>(weights);
            var huffmanCodes = new string[weights.Count];

            for (var i = 0; i < weights.Count; i++)
            {
                huffmanCodes[i] = string.Empty;
            }

            while (huffmanHeap.A.Count > 1)
            {
                var min1 = huffmanHeap.RemoveMinimum();
                var min2 = huffmanHeap.RemoveMinimum();

                foreach (var i in min1.Indices)
                {
                    huffmanCodes[i] = $"0{huffmanCodes[i]}";
                }

                foreach (var j in min2.Indices)
                {
                    huffmanCodes[j] = $"1{huffmanCodes[j]}";
                }

                var combinedIndices = new HashSet<int>();
                combinedIndices.UnionWith(min1.Indices);
                combinedIndices.UnionWith(min2.Indices);

                var combinedNode = new HuffmanNode(combinedIndices, min1.Frequency + min2.Frequency);
                huffmanHeap.Insert(combinedNode);
            }

            return huffmanCodes;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");
            N = Convert.ToInt32(textReader.ReadLine().Trim());
            var weights = new List<HuffmanNode>();
            var currentIndex = 0;

            while (!textReader.EndOfStream)
            {
                weights.Add(new HuffmanNode(new HashSet<int> { currentIndex }, Convert.ToDouble(textReader.ReadLine().Trim())));
                currentIndex++;
            }

            var codes = ProcessHuffman(weights);

            var min = int.MaxValue;
            var max = int.MinValue;
            for (var i = 0; i < codes.Length; i++)
            {
                if (codes[i].Length < min)
                {
                    min = codes[i].Length;
                }

                if (codes[i].Length > max)
                {
                    max = codes[i].Length;
                }
            }

            Console.WriteLine(min);
            Console.WriteLine(max);
            Console.ReadKey();
        }
    }
}
