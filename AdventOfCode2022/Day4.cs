namespace AdventOfCode2022 {
    internal class Day4 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day4.txt");

        public static int CountFullyOverlappingRanges() => CountRanges(FilterFullyOverlappingRanges);

        public static int CountOverlappingRanges() => CountRanges(FilterOverlappingRanges);

        private static int CountRanges(Func<int[], bool> filter) {
            return input.Select(line => line.Split('-', ',')
                    .Select(x => int.Parse(x))
                    .ToArray())
                .Count(x => filter(x));
        }

        private static bool FilterFullyOverlappingRanges(int[] numbers) => (numbers[0] <= numbers[2] && numbers[1] >= numbers[3])
            || (numbers[2] <= numbers[0] && numbers[3] >= numbers[1]);

        private static bool FilterOverlappingRanges(int[] numbers) => numbers[0] <= numbers[3] && numbers[2] <= numbers[1];
    }
}
