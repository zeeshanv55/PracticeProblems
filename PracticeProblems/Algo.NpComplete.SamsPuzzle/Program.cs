using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.NpComplete.SamsPuzzle
{
    class AStarNode
    {
        public int[,] Matrix { get; set; }

        public int? G { get; set; }

        public int? H { get; set; }

        public int? F { get; set; }

        public int Goodness { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public int K { get; set; }

        public AStarNode ParentNode { get; set; }

        public List<AStarNode> GetSuccessors(int n)
        {
            var successorList = new List<AStarNode>();
            for (var length = 2; length <= n; length++)
            {
                for (var r = 0; r <= n - length; r++)
                {
                    for (var c = 0; c <= n - length; c++)
                    {
                        var newNode = new AStarNode();
                        newNode.G = this.G + 1;
                        newNode.Matrix = this.GetRotatedMatrix(length, r, c, n);
                        newNode.ParentNode = this;
                        newNode.H = newNode.GetH(n);
                        newNode.F = newNode.G + newNode.H;
                        newNode.Goodness = newNode.GetGoodness(n);
                        newNode.I = r;
                        newNode.J = c;
                        newNode.K = length;
                        successorList.Add(newNode);
                    }
                }
            }

            return successorList;
        }

        public int GetGoodness(int n)
        {
            var goodness = 0;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    for (var k = j + 1; k < n; k++)
                    {
                        if (this.Matrix[i,j] < this.Matrix[i,k])
                        {
                            goodness++;
                        }

                        if (this.Matrix[j,i] < this.Matrix[k,i])
                        {
                            goodness++;
                        }
                    }
                }
            }

            return goodness;
        }

        public int GetH(int n)
        {
            var h = 0;

            for (var i = 0; i < n; i++)
            {
                var goingDownRow = this.Matrix[i, 0] > this.Matrix[i, 1];
                var goingDownColumn = this.Matrix[0, i] > this.Matrix[1, i];
                for (var j = 1; j < n - 1; j++)
                {
                    if (goingDownRow ? this.Matrix[i,j + 1] > this.Matrix[i,j] : this.Matrix[i, j + 1] < this.Matrix[i, j])
                    {
                        goingDownRow = !goingDownRow;
                        h++;
                    }

                    if (goingDownColumn ? this.Matrix[j + 1, i] > this.Matrix[j, i] : this.Matrix[j + 1, i] < this.Matrix[j, i])
                    {
                        goingDownColumn = !goingDownColumn;
                        h++;
                    }
                }
            }

            return h;
        }

        public int[,] GetRotatedMatrix(int length, int r, int c, int n)
        {
            var rotatedMatrix = new int[n,n];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (i >= r && i < r + length && j >= c && j < c + length)
                    {
                        rotatedMatrix[j + r - c, length - 1 - i + r + c] = this.Matrix[i,j];
                    }
                    else
                    {
                        rotatedMatrix[i,j] = this.Matrix[i,j];
                    }
                }
            }

            return rotatedMatrix;
        }
    }

    class Program
    {
        static List<AStarNode> ClosedNodes { get; set; }

        static List<AStarNode> OpenNodes { get; set; }

        static AStarNode ComputeAStar(int N)
        {
            var maxPossibleGoodness = Math.Pow(N, 2)  * (N - 1); 
            while (OpenNodes.Any())
            {
                var bestOpenNode = OpenNodes.ElementAt(0);
                OpenNodes.RemoveAt(0);
                var newNodes = bestOpenNode.GetSuccessors(N);
                foreach (var newNode in newNodes)
                {
                    if (newNode.Goodness == maxPossibleGoodness)
                    {
                        return newNode;
                    }

                    if (newNode.G >= 500)
                    {
                        continue;
                    }

                    if (OpenNodes.Any(o => newNode.Matrix.Cast<int>().SequenceEqual(o.Matrix.Cast<int>()) && o.F <= newNode.F))
                    {
                        continue;
                    }

                    if (ClosedNodes.Any(c => newNode.Matrix.Cast<int>().SequenceEqual(c.Matrix.Cast<int>()) && c.F <= newNode.F))
                    {
                        continue;
                    }

                    var insertPoint = 0;
                    while (insertPoint < OpenNodes.Count && OpenNodes.ElementAt(insertPoint).F < newNode.F)
                    {
                        insertPoint++;
                    }

                    OpenNodes.Insert(insertPoint, newNode);
                }

                ClosedNodes.Add(bestOpenNode);
            }

            return ClosedNodes.FirstOrDefault(n => n.Goodness == ClosedNodes.Max(m => m.Goodness));
        }

        static void SolvePuzzle(int n)
        {
            var bestNode = ComputeAStar(n);
            var node = bestNode;
            var nodeStack = new Stack<AStarNode>();

            while (node.ParentNode != null)
            {
                nodeStack.Push(node);
                node = node.ParentNode;
            }

            Console.WriteLine(bestNode.G);

            while (nodeStack.Any())
            {
                node = nodeStack.Pop();
                Console.WriteLine($"{node.I + 1} {node.J + 1} {node.K}");
            }
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            int n = Convert.ToInt32(textReader.ReadLine());
            int[][] puzzle = new int[n][];

            for (int i = 0; i < n; i++)
            {
                puzzle[i] = Array.ConvertAll(textReader.ReadLine().Split(' '), puzzleTemp => Convert.ToInt32(puzzleTemp));
            }

            var initialNode = new AStarNode();
            initialNode.Matrix = new int[n,n];

            for(var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    initialNode.Matrix[i, j] = puzzle[i][j];
                }
            }

            initialNode.G = 0;
            initialNode.H = 0;
            initialNode.F = 0;
            initialNode.I = -1;
            initialNode.J = -1;
            initialNode.K = -1;
            initialNode.Goodness = initialNode.GetGoodness(n);

            ClosedNodes = new List<AStarNode>();
            OpenNodes = new List<AStarNode> { initialNode };

            SolvePuzzle(n);
            Console.ReadKey();
        }
    }
}
