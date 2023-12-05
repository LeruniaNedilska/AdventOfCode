namespace AdventOfCode2023.Days {
    using System.Text.RegularExpressions;

    public class Day5(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var (seeds, maps) = GetData();

            var transformations = new Dictionary<long, long>[7];
            foreach (var (map, index) in maps.Select((map, index) => (map, index))) {
                transformations[index] = [];
                foreach (var (dest, source, range) in map) {
                    foreach (var seed in seeds) {
                        if (seed >= source && seed < source + range) {
                            transformations[index][seed] = dest + seed - source;
                        }
                    }
                }

                foreach (var seed in seeds) {
                    if (!transformations[index].ContainsKey(seed)) {
                        transformations[index][seed] = seed;
                    }
                }
                seeds = transformations[index].Values.ToArray();
            }

            return seeds.Min();
        }

        public object SolveSecond() {
            var (ranges, maps) = GetData();

            var seedRanges = Enumerable.Range(0, ranges.Count() / 2)
                .Select(j => (start: ranges.ElementAt(j * 2), end: ranges.ElementAt(j * 2 + 1) + ranges.ElementAt(j * 2) - 1))
                .ToList();

            var transformations = new Dictionary<(long start, long end), (long start, long end)>[7];
            foreach (var (map, index) in maps.Select((map, index) => (map, index))) {
                transformations[index] = [];
                foreach (var (dest, source, range) in map) {
                    for (var i = 0; i < seedRanges.Count; i++) {
                        var seedRange = seedRanges[i];

                        if (seedRange.start <= source + range - 1 && source <= seedRange.end) {
                            var rangeStart = Math.Max(seedRange.start, source);
                            var rangeEnd = Math.Min(seedRange.end, source + range - 1);
                            var destStart = dest + Math.Abs(source - seedRange.start);
                            var destEnd = dest + Math.Abs(source - seedRange.start) + rangeEnd - rangeStart;

                            transformations[index][(rangeStart, rangeEnd)] = (destStart, destEnd);

                            if (seedRange.start < source) {
                                seedRanges.Remove(seedRange);
                                seedRanges.Add((rangeStart, rangeEnd));
                                seedRanges.Add((seedRange.start, source - 1));
                            }

                            if (seedRange.end > source + range) {
                                seedRanges.Remove(seedRange);
                                seedRanges.Add((rangeStart, rangeEnd));
                                seedRanges.Add((source + range, seedRange.end));
                            }
                        }
                    }
                }

                foreach (var seedRange in seedRanges) {
                    if (!transformations[index].ContainsKey(seedRange)) {
                        transformations[index][seedRange] = seedRange;
                    }
                }

                seedRanges = [.. transformations[index].Values];
            }

            return seedRanges.Min(x => x.start);
        }

        private (IEnumerable<long> seeds, IEnumerable<IEnumerable<(long dest, long source, long range)>> maps) GetData() {
            var seedsLine = _input[0].Split(": ")[1];
            var seeds = Regex.Matches(seedsLine, @"\d+").Select(match => long.Parse(match.Value));

            var maps = new List<List<(long dest, long source, long range)>>();
            foreach (var line in _input.Skip(2)) {
                if (line.Length == 0) {
                    continue;
                }

                if (char.IsLetter(line[0])) {
                    maps.Add([]);
                    continue;
                }

                var numbers = Regex.Matches(line, @"\d+").Select(match => long.Parse(match.Value)).ToArray();
                maps.Last().Add((numbers[0], numbers[1], numbers[2]));
            }

            return (seeds, maps);
        }
    }
}
