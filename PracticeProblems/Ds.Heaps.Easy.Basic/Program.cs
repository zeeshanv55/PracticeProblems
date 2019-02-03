using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.Heaps.Easy.Basic
{
    class Program
    {
        class Heap
        {
            public List<int> A { get; set; }

            public Heap()
            {
                this.A = new List<int>();
            }

            public void Insert(int x)
            {
                A.Add(x);
                var i = A.Count - 1;

                while (i > 0 && this.A[((i + 1) / 2) - 1] > A[i])
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
                                else //Sift down
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
                                else //Sift down
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
        }

        static void Main(string[] args)
        {
            var h = new Heap();

            h.Insert(10);
            h.Insert(15);
            h.Insert(30);
            h.Insert(40);
            h.Insert(50);
            h.Insert(100);
            h.Insert(40);
            h.Insert(20);
            h.Insert(6);

            Console.WriteLine(h.Exists(100));
            Console.WriteLine(h.Exists(10));
            Console.WriteLine(h.Exists(15));
            Console.WriteLine(h.Exists(30));
            Console.WriteLine(h.Exists(40));
            Console.WriteLine(h.Exists(50));
            Console.WriteLine(h.Exists(40));
            Console.WriteLine(h.Exists(20));
            Console.WriteLine(h.Exists(6));
            Console.WriteLine(h.Exists(200));
            Console.WriteLine(h.Exists(25));
            Console.WriteLine(h.Exists(45));
            Console.WriteLine(h.Exists(55));
            Console.WriteLine(h.Exists(35));

            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());
            Console.WriteLine(h.RemoveMinimum());

            Console.ReadKey();
        }
    }
}
