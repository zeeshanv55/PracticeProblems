namespace Algo.Generic.Easy.MatrixRotationInPlace
{
    using System;
    using System.IO;

    class Program
    {
        static int[][] rotate(int[][] matrix)
        {
            for (var i = 0; i < matrix.Length/2; i++)
            {
                for (var j = i; j < matrix.Length - i - 1; j++)
                {
                    var t = matrix[i][j];
                    matrix[i][j] = matrix[matrix.Length - 1 - j][i];
                    matrix[matrix.Length - 1 - j][i] = matrix[matrix.Length - 1 - i][matrix.Length - 1 - j];
                    matrix[matrix.Length - 1 - i][matrix.Length - 1 - j] = matrix[j][matrix.Length - 1 - i];
                    matrix[j][matrix.Length - 1 - i] = t;
                }
            }

            return matrix;
        }

        static void Main(string[] args)
        {
            var textReader = new StreamReader(@"C:\Users\zeesvo\Desktop\input.txt");
            var n = Convert.ToInt32(textReader.ReadLine());
            var matrix = new int[n][];

            for (int i = 0; i < n; i++)
            {
                matrix[i] = Array.ConvertAll(textReader.ReadLine().Split(' '), x => Convert.ToInt32(x));
            }

            var rotatedMatrix = rotate(matrix);

            for (var i = 0; i < n; i++)
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
