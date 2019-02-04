using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Heaps.Easy.MedianMaintainance
{
    class Program
    {
        class MinHeap
        {
            public List<int> A { get; set; }

            public MinHeap()
            {
                this.A = new List<int>();
            }

            public void Insert(int x)
            {
                A.Add(x);
                var i = A.Count - 1;

                while (i > 0 && this.A[((i + 1) / 2) - 1] > A[i]) //Bubble up
                {
                    var t = this.A[((i + 1) / 2) - 1];
                    this.A[((i + 1) / 2) - 1] = this.A[i];
                    this.A[i] = t;
                    i = ((i + 1) / 2) - 1;
                }
            }

            public bool Exists(int x, int rootIndex = 0)
            {
                if (rootIndex >= this.A.Count || rootIndex < 0)
                {
                    return false;
                }

                if (x == this.A[rootIndex])
                {
                    return true;
                }
                else
                {
                    return Exists(x, 2 * rootIndex + 1) || Exists(x, 2 * rootIndex + 2);
                }
            }

            public int RemoveMinimum()
            {
                if (this.A.Any())
                {
                    var r = this.A[0];
                    this.A[0] = this.A[this.A.Count - 1];
                    this.A.RemoveAt(this.A.Count - 1);

                    var i = 0;
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
                                if (this.A[i] <= this.A[2 * i + 1]) //Heap conserved, can break
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
                                if (this.A[i] <= this.A[2 * i + 1] && this.A[i] <= this.A[2 * i + 2]) //Heap conserved, can break
                                {
                                    break;
                                }
                                else //Drown down
                                {
                                    var j = -1;
                                    if (this.A[2 * i + 1] < this.A[2 * i + 2])
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

                    return r;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            public int GetMinimum()
            {
                if (this.A.Any())
                {
                    return this.A[0];
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            public int Count()
            {
                return this.A.Count;
            }
        }

        class MaxHeap
        {
            public List<int> A { get; set; }

            public MaxHeap()
            {
                this.A = new List<int>();
            }

            public void Insert(int x)
            {
                A.Add(x);
                var i = A.Count - 1;

                while (i > 0 && this.A[((i + 1) / 2) - 1] < A[i]) //Bubble up
                {
                    var t = this.A[((i + 1) / 2) - 1];
                    this.A[((i + 1) / 2) - 1] = this.A[i];
                    this.A[i] = t;
                    i = ((i + 1) / 2) - 1;
                }
            }

            public bool Exists(int x, int rootIndex = 0)
            {
                if (rootIndex >= this.A.Count || rootIndex < 0)
                {
                    return false;
                }

                if (x == this.A[rootIndex])
                {
                    return true;
                }
                else
                {
                    return Exists(x, 2 * rootIndex + 1) || Exists(x, 2 * rootIndex + 2);
                }
            }

            public int RemoveMaximum()
            {
                if (this.A.Any())
                {
                    var r = this.A[0];
                    this.A[0] = this.A[this.A.Count - 1];
                    this.A.RemoveAt(this.A.Count - 1);

                    var i = 0;
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
                                if (this.A[i] >= this.A[2 * i + 1]) //Heap conserved, can break
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
                                if (this.A[i] >= this.A[2 * i + 1] && this.A[i] >= this.A[2 * i + 2]) //Heap conserved, can break
                                {
                                    break;
                                }
                                else //Drown down
                                {
                                    var j = -1;
                                    if (this.A[2 * i + 1] > this.A[2 * i + 2])
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

                    return r;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            public int GetMaximum()
            {
                if (this.A.Any())
                {
                    return this.A[0];
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            public int Count()
            {
                return this.A.Count;
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"input.txt");

            var hLow = new MaxHeap();
            var hHigh = new MinHeap();
            var median = Convert.ToInt32(textReader.ReadLine().Trim());
            var medianSums = (double)median;

            while (!textReader.EndOfStream)
            {
                var x = Convert.ToInt32(textReader.ReadLine().Trim());

                if (x < median)
                {
                    hLow.Insert(x);
                }
                else
                {
                    hHigh.Insert(x);
                }

                if (hLow.Count() == hHigh.Count() + 1)
                {
                    var newMedian = hLow.RemoveMaximum();
                    hHigh.Insert(median);
                    median = newMedian;
                    medianSums += median;
                }
                else if (hLow.Count() == hHigh.Count() - 1 || hLow.Count() == hHigh.Count())
                {
                    medianSums += median;
                }
                else if (hLow.Count() == hHigh.Count() - 2)
                {
                    var newMedian = hHigh.RemoveMinimum();
                    hLow.Insert(median);
                    median = newMedian;
                    medianSums += median;
                }
                else
                {
                    throw new Exception("This should never happen");
                }
            }

            Console.WriteLine(medianSums);
            Console.ReadKey();
        }
    }
}
