namespace AdventOfCode2023.Days {
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day6(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var times = Regex.Matches(_input[0].Split(": ")[1], @"\d+")
                .Select(match => int.Parse(match.Value))
                .ToArray();
            var distances = Regex.Matches(_input[1].Split(": ")[1], @"\d+")
                .Select(match => int.Parse(match.Value))
                .ToArray();

            return times.Zip(
                 distances,
                 (time, distance) => Enumerable.Range(1, time - 1).Count(j => (time - j) * j > distance))
                 .Aggregate(1, (acc, win) => acc * win);
        }

        public object SolveSecond() {
            var time = long.Parse(_input[0].Split(": ")[1].Replace(" ", ""));
            var distance = long.Parse(_input[1].Split(": ")[1].Replace(" ", ""));

            return Enumerable.Range(1, (int)time - 1)
                .Count(j => (time - j) * j > distance);
        }
    }
}
