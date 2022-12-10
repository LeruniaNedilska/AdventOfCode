namespace AdventOfCode2022 {
    internal class Day10 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day10.txt");

        public static void RenderImage() {
            var xPositions = GetXPositionsOverTime().ToArray();
            for(var i = 0; i < xPositions.Length; i++) {
                var cycle = i % 40;
                var isPixelLit = Math.Abs(xPositions[i] - cycle) <= 1;
                Console.Write(isPixelLit ? "#" : ".");

                if (cycle == 39) {
                    Console.Write("\n");
                }
            }
        }

        public static int GetSignalStrengthSum() {
            var interestingSignalCycleIndexes = new[] { 20, 60, 100, 140, 180, 220 };
            var xPositions = GetXPositionsOverTime().Select((position, index) => position * (index + 1)).ToArray();
            return interestingSignalCycleIndexes.Select(i => xPositions[i - 1]).Sum();
        }

        private static IEnumerable<int> GetXPositionsOverTime() {
            var x = 1;
            foreach (var line in input) {
                if (line == "noop") {
                    yield return x;
                    continue;
                }

                var addx = line.Split(' ');
                var tt = int.Parse(addx[1]);
                yield return x;
                yield return x;
                x += tt;
            }
        }
    }
}
