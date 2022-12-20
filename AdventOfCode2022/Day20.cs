namespace AdventOfCode2022 {
    internal class Day20 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day20.txt");
        public static long GetGrooveCoordinatesSum(long decryptionKey, int mixCount) {
            List<(long value, int intitialIndex)> sequence = input.Select((x, index) => (long.Parse(x) * decryptionKey, index)).ToList();
            var maxIndex = sequence.Count - 1;

            for (var j = 0; j < mixCount; j++) {
                for (var i = 0; i < sequence.Count; i++) {
                    var currentIndex = sequence.FindIndex(x => x.intitialIndex == i);
                    var item = sequence[currentIndex];
                    if (item.value == 0) {
                        continue;
                    }

                    int newIndex = (int)(item.value + currentIndex >= 0 ? (item.value + currentIndex) % maxIndex : maxIndex + (item.value + currentIndex) % maxIndex);

                    sequence.RemoveAt(currentIndex);
                    sequence.Insert(newIndex == 0 ? maxIndex : newIndex, item);
                }
            }

            var zeroIndex = sequence.FindIndex(x => x.value == 0);
            return sequence.Where((x, index) => index == (zeroIndex + 1000) % (maxIndex + 1)).Single().value +
                sequence.Where((x, index) => index == (zeroIndex + 2000) % (maxIndex + 1)).Single().value +
                sequence.Where((x, index) => index == (zeroIndex + 3000) % (maxIndex + 1)).Single().value;
        }
    }
}
