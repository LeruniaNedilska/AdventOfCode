namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day13(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var maps = GetMaps();
            return maps.Sum(x => GetReflection(x, countSmudges: 0));
        }

        public object SolveSecond() {
            var maps = GetMaps();
            return maps.Sum(x => GetReflection(x, countSmudges: 1));
        }

        private static int GetReflection(char[][] map, int countSmudges = 0) {
            var rowReflection = GetRowReflection(map, countSmudges);
            if (rowReflection != 0) {
                return 100 * rowReflection;
            }

            var transposedMap = Transpose(map);
            return GetRowReflection(transposedMap, countSmudges);
        }

        private static int GetRowReflection(char[][] map, int countSmudges = 0) {
            for (var i = 1; i < map.Length; i++) {
                var first = map.Take(i).Reverse().ToArray();
                var second = map.Skip(i).ToArray();

                var smudgeCount = 0;
                for (var j = 0; j < Math.Min(first.Length, second.Length); j++) {
                    for (var k = 0; k < Math.Min(first[j].Length, second[j].Length); k++) {
                        if (first[j][k] != second[j][k]) {
                            smudgeCount++;
                        }
                    }
                }

                if (smudgeCount == countSmudges) {
                    return i;
                }
            }

            return 0;
        }

        private static char[][] Transpose(char[][] map) {
            var transposed = new char[map[0].Length][];
            for (var i = 0; i < map[0].Length; i++) {
                transposed[i] = new char[map.Length];
                for (var j = 0; j < map.Length; j++) {
                    transposed[i][j] = map[j][i];
                }
            }
            return transposed;
        }

        private List<char[][]> GetMaps() {
            var maps = new List<char[][]>();
            var map = new List<char[]>();
            foreach (var line in _input) {
                if (string.IsNullOrEmpty(line)) {
                    maps.Add([.. map]);
                    map.Clear();
                }
                else {
                    map.Add(line.ToCharArray());
                }
            }

            maps.Add([.. map]);
            return maps;
        }
    }
}
