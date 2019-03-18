namespace Algo.TwoSat.Papadimitriou
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class BoolSetComparer : IEqualityComparer<bool[]>
    {
        public bool Equals(bool[] x, bool[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(bool[] obj)
        {
            var sum = 0;
            for (var i = 0; i < obj.Length; i++)
            {
                sum += obj[i] ? i + (i * i) : 0;
            }

            return sum * obj.Length;
        }
    }

    class TwoSat
    {
        public int VariableCount { get; set; }

        public bool[] Variables { get; set; }

        public HashSet<int> OnlyPositiveVariables { get; set; }

        public HashSet<int> OnlyNegativeVariables { get; set; }

        public HashSet<int> VariableVariables { get; set; }

        public HashSet<Tuple<int, int>> VariableConstraints { get; set; }

        public Tuple<int, int>[] Constraints { get; set; }

        private List<Tuple<int, int>> LastUnsatConstraints { get; set; }

        private HashSet<bool[]> PastStates { get; set; }

        private Random RandomGenerator { get; set; }

        public TwoSat(int variableCount, Tuple<int, int>[] constraints)
        {
            this.VariableCount = variableCount;
            this.Constraints = constraints;
            
            this.Initialize();
        }

        private void Initialize()
        {
            this.RandomGenerator = new Random();
            this.OnlyPositiveVariables = new HashSet<int>();
            this.OnlyNegativeVariables = new HashSet<int>();
            this.VariableVariables = new HashSet<int>();
            this.PastStates = new HashSet<bool[]>(new BoolSetComparer());
            this.Variables = new bool[this.VariableCount];
            this.VariableConstraints = new HashSet<Tuple<int, int>>(this.Constraints);

            var lastReductionCount = int.MaxValue;
            do
            {
                lastReductionCount = this.VariableConstraints.Count();
                this.VariableConstraints = this.Reduce(this.VariableConstraints);
            }
            while (this.VariableConstraints.Count() < lastReductionCount);

            foreach (var c in this.VariableConstraints)
            {
                this.VariableVariables.Add(Math.Abs(c.Item1) - 1);
                this.VariableVariables.Add(Math.Abs(c.Item2) - 1);
            }

            if (this.OnlyPositiveVariables.Intersect(this.OnlyNegativeVariables).Any())
            {
                throw new Exception("Variables can't be both positive and negative");
            }

            foreach (var i in this.OnlyPositiveVariables)
            {
                this.Variables[i] = true;
            }

            foreach (var i in this.OnlyNegativeVariables)
            {
                this.Variables[i] = false;
            }
        }

        private HashSet<Tuple<int, int>> Reduce(HashSet<Tuple<int, int>> constraints)
        {
            var variableConstraints = new HashSet<Tuple<int, int>>();
            var variablesUsedAsPositive = new HashSet<int>();
            var variablesUsedAsNegative = new HashSet<int>();

            foreach (var c in constraints)
            {
                if (c.Item1 > 0)
                {
                    variablesUsedAsPositive.Add(Math.Abs(c.Item1) - 1);
                }
                else
                {
                    variablesUsedAsNegative.Add(Math.Abs(c.Item1) - 1);
                }

                if (c.Item2 > 0)
                {
                    variablesUsedAsPositive.Add(Math.Abs(c.Item2) - 1);
                }
                else
                {
                    variablesUsedAsNegative.Add(Math.Abs(c.Item2) - 1);
                }
            }

            var onlyPositiveVariables = new HashSet<int>();
            var onlyNegativeVariables = new HashSet<int>();
            var variableVariables = new HashSet<int>();
            for (var i = 0; i < this.VariableCount; i++)
            {
                if (variablesUsedAsPositive.Contains(i) && variablesUsedAsNegative.Contains(i))
                {
                    variableVariables.Add(i);
                }

                if (variablesUsedAsPositive.Contains(i) && !variablesUsedAsNegative.Contains(i))
                {
                    onlyPositiveVariables.Add(i);
                }

                if (!variablesUsedAsPositive.Contains(i) && variablesUsedAsNegative.Contains(i))
                {
                    onlyNegativeVariables.Add(i);
                }
            }

            this.OnlyPositiveVariables.UnionWith(onlyPositiveVariables);
            this.OnlyNegativeVariables.UnionWith(onlyNegativeVariables);

            foreach (var c in constraints)
            {
                if (variableVariables.Contains(Math.Abs(c.Item1) - 1) && variableVariables.Contains(Math.Abs(c.Item2) - 1))
                {
                    variableConstraints.Add(c);
                }
            }

            return variableConstraints;
        }

        public bool Run()
        {
            var outerLimit = Math.Ceiling(Math.Log(this.Variables.Length, 2));
            var innerLimit = (double)this.VariableVariables.Count() * (double)2 * (double)this.VariableVariables.Count();

            for (var i = 0; i < outerLimit; i++)
            {
                Console.WriteLine($"Running attempt {i + 1}/{outerLimit}");
                this.RandomlyInitialize();
                
                for (var j = 0; j < innerLimit; j++)
                {
                    var unsatisfiedConstraints = this.LastUnsatConstraints == null ? this.GetUnsatisfiedConstraints() : this.LastUnsatConstraints;
                    if (j % 10 == 0)
                    {
                        //Console.WriteLine($"{(int)(j * 100 / innerLimit)}% done. Number of unsatisfied constraints: {unsatisfiedConstraints.Count}");
                    }

                    if (unsatisfiedConstraints.Any())
                    {
                        do
                        {
                            this.RandomlyFlipVariable(unsatisfiedConstraints);
                        }
                        while (this.PastStates.Contains(this.Variables));
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Flip(int index)
        {
            this.Variables[index] = !this.Variables[index];
        }

        private void RandomlyFlipVariable(List<Tuple<int, int>> unsatisfiedConstraints)
        {
            var pickedConstraint = this.RandomGenerator.Next(0, unsatisfiedConstraints.Count);

            var variable1 = Math.Abs(unsatisfiedConstraints[pickedConstraint].Item1) - 1;
            var variable2 = Math.Abs(unsatisfiedConstraints[pickedConstraint].Item2) - 1;

            var variablesIf1Flipped = new bool[this.Variables.Length];
            this.Variables.CopyTo(variablesIf1Flipped, 0);
            variablesIf1Flipped[variable1] = !variablesIf1Flipped[variable1];

            var variablesIf2Flipped = new bool[this.Variables.Length];
            this.Variables.CopyTo(variablesIf2Flipped, 0);
            variablesIf2Flipped[variable2] = !variablesIf1Flipped[variable2];

            if (!this.VariableVariables.Contains(variable2))
            {
                this.Flip(variable1);
                this.LastUnsatConstraints = null;
            }
            else if (!this.VariableVariables.Contains(variable1))
            {
                this.Flip(variable2);
                this.LastUnsatConstraints = null;
            }
            else
            {
                var unsatConstraints1 = this.GetUnsatisfiedConstraints(variablesIf1Flipped);
                var unsatConstraints2 = this.GetUnsatisfiedConstraints(variablesIf2Flipped);

                if (unsatConstraints1.Count < unsatConstraints2.Count)
                {
                    this.Flip(variable1);
                    this.LastUnsatConstraints = new List<Tuple<int, int>>(unsatConstraints1);
                }
                else if (unsatConstraints1.Count > unsatConstraints2.Count)
                {
                    this.Flip(variable2);
                    this.LastUnsatConstraints = new List<Tuple<int, int>>(unsatConstraints2);
                }
                else
                {
                    if (this.RandomGenerator.Next(0, 2) == 0)
                    {
                        this.Flip(variable1);
                        this.LastUnsatConstraints = new List<Tuple<int, int>>(unsatConstraints1);
                    }
                    else
                    {
                        this.Flip(variable2);
                        this.LastUnsatConstraints = new List<Tuple<int, int>>(unsatConstraints2);
                    }
                }
            }
        }

        private List<Tuple<int, int>> GetUnsatisfiedConstraints(bool[] variables)
        {
            var returnList = new List<Tuple<int, int>>();
            foreach (var c in this.VariableConstraints)
            {
                if (!this.IsConstraintSatisfied(c, variables))
                {
                    returnList.Add(c);
                }
            }

            return returnList;
        }

        private List<Tuple<int, int>> GetUnsatisfiedConstraints()
        {
            var returnList = new List<Tuple<int, int>>();
            foreach (var c in this.VariableConstraints)
            {
                if (!this.IsConstraintSatisfied(c))
                {
                    returnList.Add(c);
                }
            }

            return returnList;
        }

        private bool IsConstraintSatisfied(Tuple<int, int> constraint)
        {
            return 
                (
                    constraint.Item1 < 0
                        ? !this.Variables[(-1 * constraint.Item1) - 1]
                        : this.Variables[constraint.Item1 - 1]
                ) ||
                (
                    constraint.Item2 < 0
                        ? !this.Variables[(-1 * constraint.Item2) - 1]
                        : this.Variables[constraint.Item2 - 1]
                );
        }

        private bool IsConstraintSatisfied(Tuple<int, int> constraint, bool[] variables)
        {
            return
                (
                    constraint.Item1 < 0
                        ? !variables[(-1 * constraint.Item1) - 1]
                        : variables[constraint.Item1 - 1]
                ) ||
                (
                    constraint.Item2 < 0
                        ? !variables[(-1 * constraint.Item2) - 1]
                        : variables[constraint.Item2 - 1]
                );
        }

        private void RandomlyInitialize()
        {
            foreach (var i in this.VariableVariables)
            {
                this.Variables[i] = this.RandomGenerator.Next(0, 2) == 0;
            }

            this.PastStates = new HashSet<bool[]>();
            this.RecordState();
        }

        private void RecordState()
        {
            var copyVars = new bool[this.VariableCount];
            this.Variables.CopyTo(copyVars, 0);
            this.PastStates.Add(copyVars);
        }
    }

    class Program
    {
        static Tuple<int, Tuple<int, int>[]> ReadInput(string fileName)
        {
            var textReader = new StreamReader(fileName);
            var n = Convert.ToInt32(textReader.ReadLine().Trim());
            var constraints = new List<Tuple<int, int>>();
            while (!textReader.EndOfStream)
            {
                var line = textReader.ReadLine().Trim().Split(' ');

                var variable1 = Convert.ToInt32(line[0].Trim());
                var variable2 = Convert.ToInt32(line[1].Trim());

                constraints.Add(new Tuple<int, int>(variable1, variable2));
            }

            textReader.Close();
            textReader.Dispose();

            return new Tuple<int, Tuple<int, int>[]>(n, constraints.ToArray());
        }

        static void Main(string[] args)
        {
            var inpuFiles = new string[] { "input1.txt", "input2.txt", "input3.txt", "input4.txt", "input5.txt", "input6.txt" };
            var solution = new int[inpuFiles.Length];

            for (var i = 0; i < inpuFiles.Length; i++)
            {
                var inputs = ReadInput(inpuFiles[i]);
                Console.WriteLine($"{inputs.Item1} total variables");
                var twoSat = new TwoSat(inputs.Item1, inputs.Item2);
                solution[i] = twoSat.Run() ? 1 : 0;
                Console.WriteLine(solution[i]);
            }

            Console.ReadKey();
        }
    }
}
