namespace AdventOfCode2023.Days {
    using System.Text.RegularExpressions;

    public class Day4(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return _input.Select(GetWinningNumbersCount)
                .Select(count => count > 0 ? count == 1 ? 1 : Math.Pow(2, count - 1) : 0)
                .Sum();
        }

        public object SolveSecond() {
            var copies = new Dictionary<int, int>();
            foreach (var (line, index) in _input.Select((line, index) => (line, index))) {
                var count = GetWinningNumbersCount(line);

                if (count > 0) {
                    var copy = copies.TryGetValue(index, out var value) ? value : 0;
                    for (var i = 1; i <= count; i++) {
                        copies[index + i] = copies.ContainsKey(index + i) ? copies[index + i] + 1 + copy : 1 + copy;
                    }
                }
            }

            return copies.Values.Sum() + _input.Length;
        }

        private int GetWinningNumbersCount(string line) {
            var cards = line.Split(": ")[1].Split(" | ");

            var winningCards = Regex.Matches(cards[0], @"\d+").Select(match => int.Parse(match.Value));
            var yourCards = Regex.Matches(cards[1], @"\d+").Select(match => int.Parse(match.Value));

            return yourCards.Count(card => winningCards.Contains(card));
        }
    }
}
