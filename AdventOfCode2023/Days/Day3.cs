namespace AdventOfCode2023.Days {
    using System.Text.RegularExpressions;

    public class Day3(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var (numbers, parts) = GetNumbersAndParts();

            var result = 0;
            foreach (var (number, position) in numbers) {
                var adjacentParts = parts
                    .Where(part => Math.Abs(part.position.x - position.x) <= 1 || Math.Abs(part.position.y - position.y) <= 1)
                    .Where(part => AreAdjacent((number, position), part.position))
                    .ToList();

                if (adjacentParts.Any()) {
                    result += number;
                }
            }

            return result;
        }

        public object SolveSecond() {
            var (numbers, parts) = GetNumbersAndParts();
            var result = 0;
            foreach (var (part, position) in parts) {
                var adjacentNumbers = numbers
                    .Where(number => Math.Abs(number.position.x - position.x) <= 1 || Math.Abs(number.position.y - position.y) <= 1)
                    .Where(number => AreAdjacent(number, position)).ToArray();

                if (adjacentNumbers.Length == 2) {
                    result += adjacentNumbers[0].number * adjacentNumbers[1].number;
                }
            }

            return result;
        }

        private (List<(int number, (int x, int y) position)>, List<(string part, (int x, int y) position)>) GetNumbersAndParts() {
            var lineNumber = 0;
            List<(int number, (int x, int y) position)> numbersList = [];
            List<(string part, (int x, int y) position)> partsList = [];

            foreach (var line in _input) {
                var numbers = Regex.Matches(line, @"\d+")
                    .Select(match => (int.Parse(match.Value), (match.Index, lineNumber)))
                    .ToList();
                numbersList.AddRange(numbers);

                var parts = Regex.Matches(line, @"[^.\d]")
                    .Select(match => (match.Value, (match.Index, lineNumber)))
                    .ToList();
                partsList.AddRange(parts);

                lineNumber++;
            }

            return (numbersList, partsList);
        }

        private bool AreAdjacent((int number, (int x, int y) position) number, (int x, int y) part) {
            var numberPositions = GetNumberPositions(number.number, number.position);
            foreach (var numberPosition in numberPositions) {
                if (Math.Abs(numberPosition.x - part.x) <= 1 && Math.Abs(numberPosition.y - part.y) <= 1) {
                    return true;
                }
            }

            return false;
        }

        private List<(int x, int y)> GetNumberPositions(int number, (int x, int y) position) {
            var list = new List<(int x, int y)> { position };
            var len = number.ToString().Length;

            if (len > 1) {
                for (var i = 1; i < len; i++) {
                    list.Add((position.x + i, position.y));
                }
            }

            return list;
        }
    }
}
