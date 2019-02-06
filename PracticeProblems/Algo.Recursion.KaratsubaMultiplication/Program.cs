namespace Algo.Recursion.KaratsubaMultiplication
{
    using System;

    class Program
    {
        static string Add(string A, string B)
        {
            var X = A.Length >= B.Length ? A : B;
            var Y = A.Length < B.Length ? A : B;
            Y = Y.PadLeft(X.Length, '0');

            var xArray = X.ToCharArray();
            var yArray = Y.ToCharArray();
            var resultBuilder = string.Empty;

            var carry = 0;
            for (var i = X.Length - 1; i >= 0; i--)
            {
                var y = (int)char.GetNumericValue(Y[i]);
                var x = (int)char.GetNumericValue(X[i]);
                var result = carry + x + y;
                carry = result / 10;
                resultBuilder = Convert.ToString(result % 10) + resultBuilder;
            }

            return (Convert.ToString(carry) + resultBuilder).TrimStart('0');
        }

        static string MultiplySingle(string Y, int x)
        {
            var yArray = Y.ToCharArray();
            var carry = 0;
            var resultBuilder = string.Empty;

            for (var i = yArray.Length - 1; i >= 0; i--)
            {
                var y = (int)char.GetNumericValue(Y[i]);
                var result = (x * y) + carry;
                carry = result / 10;
                resultBuilder = Convert.ToString(result % 10) + resultBuilder;
            }

            return Convert.ToString(carry) + resultBuilder;
        }

        static string Multiply(string X, string Y)
        {
            if (string.IsNullOrWhiteSpace(X))
            {
                return Y;
            }

            if (string.IsNullOrWhiteSpace(Y))
            {
                return X;
            }

            if (X.Length == 1)
            {
                return MultiplySingle(Y, (int)char.GetNumericValue(X[0])).TrimStart('0');
            }

            if (Y.Length == 1)
            {
                return MultiplySingle(X, (int)char.GetNumericValue(Y[0])).TrimStart('0');
            }

            var n = X.Length;
            var m = Y.Length;

            var n1 = n / 2;
            var n2 = n - n1;
            var m1 = m / 2;
            var m2 = m - m1;

            var X1 = X.Substring(0, n1);
            var X2 = X.Substring(n1);
            var Y1 = Y.Substring(0, m1);
            var Y2 = Y.Substring(m1);

            var A = Multiply(X1, Y1);
            var B = Multiply(X1, Y2);
            var C = Multiply(X2, Y1);
            var D = Multiply(X2, Y2);

            A = A.PadRight(A.Length + n2 + m2, '0');
            B = B.PadRight(B.Length + n2, '0');
            C = C.PadRight(C.Length + m2, '0');

            return Add(A, Add(B, Add(C, D)));
        }

        static string Factorial(int t)
        {
            var currentResult = "1";
            for (var i = 1; i <= t; i++)
            {
                currentResult = Multiply(currentResult, Convert.ToString(i));
            }

            return currentResult;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Factorial(0) + "\n");
            Console.WriteLine(Factorial(1) + "\n");
            Console.WriteLine(Factorial(5) + "\n");
            Console.WriteLine(Factorial(10) + "\n");
            Console.WriteLine(Factorial(100) + "\n");
            Console.WriteLine(Factorial(500) + "\n");
            Console.WriteLine(Factorial(1000) + "\n");

            Console.ReadKey();
        }
    }
}
