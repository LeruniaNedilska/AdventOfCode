namespace AdventOfCode2022 {
    internal class Day3 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day3.txt");

        public static int GetMisplacedItemsSum() {
            return input.Select(line => {
                var item = GetCommonItem(new[] { line.Substring(0, line.Length / 2), line.Substring(line.Length / 2) });
                return GetItemPriority(item);
            }).Sum();
        }

        public static int GetBadgeItemsSum() {
            var groupCount = input.Length / 3;
            var sum = 0;
            for (var i = 0; i < groupCount; i++) {
                var item = GetCommonItem(input.Skip(i*3).Take(3).ToArray());
                sum += GetItemPriority(item);
            }
            return sum;
        }

        private static char GetCommonItem(string[] compartments) {
            var charHashSet = compartments[0].ToHashSet();
            for (var i = 1; i < compartments.Length; i++) {
                charHashSet.IntersectWith(compartments[i]);
            }

            return charHashSet.Single();          
        }

        private static int GetItemPriority(char item) {
            return char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;
        }
    }
}
