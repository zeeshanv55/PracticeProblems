using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo.Generic.Easy.MatrixRotation
{
    class Program
    {
        static int[][] rotate(int[][] matrix)
        {
            var rotatedMatrix = new int[matrix[0].Length][];
            for (var i = 0; i < matrix[0].Length; i++)
            {
                rotatedMatrix[i] = new int[matrix.Length];
                for (var j = 0; j < matrix.Length; j++)
                {
                    rotatedMatrix[i][j] = matrix[matrix.Length - 1 - j][i];
                }
            }

            return rotatedMatrix;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            var nm = textReader.ReadLine();
            var n = Convert.ToInt32(nm.Split(' ')[0]);
            var m = Convert.ToInt32(nm.Split(' ')[1]);
            var matrix = new int[n][];

            for (int i = 0; i < n; i++)
            {
                matrix[i] = Array.ConvertAll(textReader.ReadLine().Split(' '), x => Convert.ToInt32(x));
            }

            var rotatedMatrix = rotate(matrix);

            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    Console.Write(rotatedMatrix[i][j] + " ");
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
