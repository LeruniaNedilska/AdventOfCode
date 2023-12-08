namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day8(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var nodes = GetInput();
            var currentNode = nodes.First(x => x.Key == "AAA").Key;
            return GetSteps(nodes, currentNode, x => x == "ZZZ");
        }

        public object SolveSecond() {
            var nodes = GetInput();
            var currentNodes = nodes.Where(x => x.Key.EndsWith('A'))
                .Select(x => x.Key)
                .ToArray();
            var steps = new long[currentNodes.Length];

            for (var i = 0; i < currentNodes.Length; i++) {
                var currentNode = currentNodes[i];
                steps[i] = GetSteps(nodes, currentNode, x => x.EndsWith('Z'));
            }

            var result = steps[0];
            for (var i = 1; i < steps.Length; i++) {
                result = GetLCM(result, steps[i]);
            }

            return result;
        }

        private Dictionary<string, (string left, string right)> GetInput() {
            var nodes = new Dictionary<string, (string, string)>();
            foreach (var line in _input.Skip(2)) {
                var parts = Regex.Matches(line, @"[0-9A-Z]+").Select(m => m.Value).ToArray();
                nodes.Add(parts[0], (parts[1], parts[2]));
            }
            return nodes;
        }

        private long GetSteps(Dictionary<string, (string left, string right)> nodes, string currentNode, Func<string, bool> predicate) {
            var result = 0L;
            while (!predicate(currentNode)) {
                foreach (var instruction in _input[0]) {
                    if (instruction == 'R') {
                        currentNode = nodes[currentNode].right;
                    }
                    else {
                        currentNode = nodes[currentNode].left;
                    }

                    result++;
                }
            }

            return result;
        }

        private static long GetLCM(long x, long y) {
            return Math.Abs(x * y) / GetGCD(x, y);
        }

        private static long GetGCD(long x, long y) {
            while (y != 0) {
                var temp = y;
                y = x % y;
                x = temp;
            }
            return x;
        }
    }
}
