namespace AdventOfCode2023.Days {
    using System.Text.RegularExpressions;

    public class Day2(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return _input.Select((line, index) => {
                var rounds = line.Split(": ")[1].Split("; ");
                var impossible = rounds.Any(round => round.Split(", ").Any(set => {
                    var match = Regex.Match(set, @"\d+");
                    if (!match.Success) {
                        throw new Exception("No number found");
                    }

                    var number = int.Parse(match.Value);
                    var color = set[number.ToString().Length..].Trim();

                    return color switch {
                        "blue" when number > 14 => true,
                        "red" when number > 12 => true,
                        "green" when number > 13 => true,
                        _ => false
                    };
                }));

                return impossible ? 0 : index + 1;
            }).Sum();
        }

        public object SolveSecond() {
            return _input.Select(line => {
                var rounds = line.Split(": ")[1].Split("; ");
                var (blue, red, green) = rounds
                    .SelectMany(round => round.Split(", "))
                    .Aggregate((blue: 0, red: 0, green: 0), (acc, set) => {
                        var match = Regex.Match(set, @"\d+");
                        if (!match.Success) {
                            throw new Exception("No number found");
                        }

                        var number = int.Parse(match.Value);
                        var color = set[number.ToString().Length..].Trim();

                        return color switch {
                            "blue" => (Math.Max(acc.blue, number), acc.red, acc.green),
                            "red" => (acc.blue, Math.Max(acc.red, number), acc.green),
                            "green" => (acc.blue, acc.red, Math.Max(acc.green, number)),
                            _ => acc
                        };
                    });

                return blue * red * green;
            }).Sum();
        }
    }
}
