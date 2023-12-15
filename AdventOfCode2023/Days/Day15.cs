namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day15(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return _input[0].Split(',')
                .Select(GetHash)
                .Sum();
        }

        public object SolveSecond() {
            var lenses = _input[0].Split(',');
            var hashMap = new Dictionary<int, List<(string label, int focalLength)>>();

            foreach (var lens in lenses) {
                var (label, focalLength) = ParseLens(lens);
                var hash = GetHash(label);

                if (hashMap.TryGetValue(hash, out var list)) {
                    var index = list.FindIndex(x => x.label == label);

                    if (index >= 0) {
                        if (focalLength != -1) {
                            list[index] = (label, focalLength);
                        }
                        else {
                            list.RemoveAt(index);
                        }
                    }
                    else if (focalLength != -1) {
                        list.Add((label, focalLength));
                    }
                }
                else if (focalLength != -1) {
                    hashMap[hash] = new List<(string, int)>() { (label, focalLength) };
                }
            }


            var sum = 0;
            foreach (var (key, value) in hashMap) {
                foreach (var ((label, number), index) in value.Select((value, index) => (value, index))) {
                    sum += (key + 1) * (index + 1) * number;
                }
            }

            return sum;
        }

        private static (string label, int focalLength) ParseLens(string lens) {
            var name = lens.Replace("=", " ").Replace("-", " ");
            var splitName = name.Split(" ");

            if (lens.Contains('=')) {
                return (splitName[0], int.Parse(splitName[1]));
            }
            else {
                return (splitName[0], -1);
            }
        }

        private static int GetHash(string value) {
            return value
              .Select(c => (int)c)
              .Aggregate(0, (currentValue, asciiCode) => (currentValue + asciiCode) * 17 % 256);
        }
    }
}
