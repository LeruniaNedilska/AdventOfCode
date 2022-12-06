namespace AdventOfCode2022 {

    internal class Day6 {
        private static readonly string input = File.ReadAllText(@"inputs\day6.txt");

        public static int GetFirstMarkerIndex(int markerLength) {
            for(var i = 0; i < input.Length - markerLength; i++) {
                var marker = input.Substring(i, markerLength);
                if (hasAllUniqueCharacters(marker)) {
                    return i + markerLength;
                }
            }

            return -1;
        }

        private static bool hasAllUniqueCharacters(string substring) {
            var hash = substring.ToHashSet();
            return hash.Count == substring.Length;
        }
    }
}
