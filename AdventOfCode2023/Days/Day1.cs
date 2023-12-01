namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day1(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return _input
                .Select(line => int.Parse(line.First(char.IsDigit).ToString()) * 10 + int.Parse(line.Last(char.IsDigit).ToString()))
                .Sum();
        }

        public object SolveSecond() {
            var sum = 0;
            int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9];
            Dictionary<string, int> words = new() {
                {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}
            };

            foreach (var line in _input) {
                var list = numbers.Select(digit => (digit, line.IndexOf(digit.ToString()), line.LastIndexOf(digit.ToString())))
                                 .Concat(words.Keys.Select(word => (words[word], line.IndexOf(word), line.LastIndexOf(word))))
                                 .Where(x => x.Item2 != -1)
                                 .OrderBy(x => x.Item2)
                                 .ToList();

                var first = list.First().Item1;
                var last = list.OrderByDescending(x => x.Item3).First().Item1;

                sum += first * 10 + last;
            }

            return sum;
        }
    }
}
