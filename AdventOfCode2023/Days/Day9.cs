namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day9(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return GetNumbers().Select(line => GetNextNumber(line)).Sum();
        }

        public object SolveSecond() {
            return GetNumbers().Select(line => GetNextNumber(line.Reverse().ToArray())).Sum();
        }

        private static int GetNextNumber(int[] numbers) {
            var nextSequence = new List<int>();
            for (var i = 0; i < numbers.Length - 1; i++) {
                nextSequence.Add(numbers[i + 1] - numbers[i]);
            }

            return nextSequence.All(v => v == 0) ? numbers.Last() : numbers.Last() + GetNextNumber(nextSequence.ToArray());
        }

        private int[][] GetNumbers() {
            return _input
                .Select(line => line.Split(' ').Select(int.Parse).ToArray())
                .ToArray();
        }
    }
}
