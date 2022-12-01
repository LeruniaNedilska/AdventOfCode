namespace AdventOfCode2022 {
    internal class Day1 {
        public static IEnumerable<int> GetMaxCaloriesSums() {
            string[] lines = File.ReadAllLines(@"inputs\day1.txt");
            List<int> sums = new();
            int tmpSum = 0;
            foreach (string line in lines) {
                if (string.IsNullOrWhiteSpace(line)) {
                    sums.Add(tmpSum);
                    tmpSum = 0;
                    continue;
                }

                tmpSum += int.Parse(line);
            }
            return sums.OrderByDescending(x => x).Take(3);
        }
    }
}
