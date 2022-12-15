namespace AdventOfCode2022 {
    using System.Numerics;
    using System.Text.RegularExpressions;

    internal class Day15 {
        private static readonly ((int x, int y) Sensor, (int x, int y) Beacon, int Distance)[] input = File.ReadAllLines(@"inputs\day15.txt")
            .Select(line => {
                var matches = Regex.Matches(line, @"x=(-?\d+),\s*y=(-?\d+)");
                return ((int.Parse(matches.First().Groups[1].Value), int.Parse(matches.First().Groups[2].Value)), (int.Parse(matches.Last().Groups[1].Value), int.Parse(matches.Last().Groups[2].Value)));
            })
            .Select(x => (x.Item1, x.Item2, GetDistance(x.Item1, x.Item2)))
            .ToArray();

        public static int GetUnsuitablePositionsCount() {
            var y = 2000000;
            var hash = GetUnsuitablePositions(y);
            return hash.Count - input.Where(x => x.Beacon.y == y).Count();
        }

        public static BigInteger GetTuningFrequency() {
            foreach (var line in input) {
                for (var i = 0; i <= line.Distance + 1; i++) {
                    (int x, int y)[] moves = new[] { (-1, -1), (-1, 1), (1, -1), (1, 1) };
                    foreach (var move in moves) {
                        var x = line.Sensor.x + (i * move.x);
                        var y = line.Sensor.y + (line.Distance + 1 - i * move.y);

                        if (x < 0 || x > 4000000 || y < 0 || y > 4000000) {
                            continue;
                        }

                        if (!input.Any(line => GetDistance((x, y), line.Sensor) <= line.Distance)) {
                            return (BigInteger)x * 4000000 + y;
                        }
                    }
                }
            }

            return -1;
        }

        private static HashSet<int> GetUnsuitablePositions(int y) {
            var hash = new HashSet<int>();
            foreach (var line in input) {
                var radius = line.Distance - Math.Abs(line.Sensor.y - y);
                if (radius < 0) {
                    continue;
                }

                for (var i = line.Sensor.x - radius; i <= line.Sensor.x + radius; i++) {
                    hash.Add(i);
                }
            }

            return hash;
        }

        private static int GetDistance((int x, int y) a, (int x, int y) b) {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }
    }
}
