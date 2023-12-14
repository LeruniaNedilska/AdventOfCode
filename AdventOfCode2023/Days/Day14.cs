namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day14(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var map = _input.Select(line => line.ToCharArray()).ToArray();
            TiltNorth(map);
            return GetLoad(map);
        }

        public object SolveSecond() {
            var map = _input.Select(line => line.ToCharArray()).ToArray();
            return GetLoadAfterCycles(map);
        }

        private static void TiltNorth(char[][] map) {
            for (var i = 0; i < map.Length; i++) {
                for (var j = 0; j < map[0].Length; j++) {
                    if (map[i][j] == 'O') {
                        var north = i;
                        while (north > 0 && map[north - 1][j] != '#' && map[north - 1][j] != 'O') {
                            north--;
                        }
                        if (north != i) {
                            map[north][j] = 'O';
                            map[i][j] = '.';
                        }
                    }
                }
            }
        }

        private static void TiltWest(char[][] map) {
            for (var i = 0; i < map.Length; i++) {
                for (var j = 0; j < map[0].Length; j++) {
                    if (map[i][j] == 'O') {
                        var west = j;
                        while (west > 0 && map[i][west - 1] != '#' && map[i][west - 1] != 'O') {
                            west--;
                        }
                        if (west != j) {
                            map[i][west] = 'O';
                            map[i][j] = '.';
                        }
                    }
                }
            }
        }

        private static void TiltSouth(char[][] map) {
            for (var i = map.Length - 1; i >= 0; i--) {
                for (var j = 0; j < map[0].Length; j++) {
                    if (map[i][j] == 'O') {
                        var south = i;
                        while (south < map.Length - 1 && map[south + 1][j] != '#' && map[south + 1][j] != 'O') {
                            south++;
                        }
                        if (south != i) {
                            map[south][j] = 'O';
                            map[i][j] = '.';
                        }
                    }
                }
            }
        }

        private static void TiltEast(char[][] map) {
            for (var i = 0; i < map.Length; i++) {
                for (var j = map[0].Length - 1; j >= 0; j--) {
                    if (map[i][j] == 'O') {
                        var east = j;
                        while (east < map[0].Length - 1 && map[i][east + 1] != '#' && map[i][east + 1] != 'O') {
                            east++;
                        }
                        if (east != j) {
                            map[i][east] = 'O';
                            map[i][j] = '.';
                        }
                    }
                }
            }
        }

        private static int GetLoad(char[][] map) {
            return map
                .Reverse()
                .Select((row, index) => row.Count(c => c == 'O') * (index + 1))
                .Sum();
        }

        private static int GetLoadAfterCycles(char[][] map) {
            var hash = new List<string>();
            var cycleStartIndex = -1;
            while (true) {
                TiltNorth(map);
                TiltWest(map);
                TiltSouth(map);
                TiltEast(map);
                var strMap = string.Join("\n", map.Select(line => new string(line)));
                cycleStartIndex = hash.IndexOf(strMap);
                if (cycleStartIndex != -1) {
                    break;
                }
                hash.Add(strMap);
            }

            var cycleLength = hash.Count - cycleStartIndex;
            var cycleIndex = (1000000000 - cycleStartIndex) % cycleLength;
            cycleIndex = cycleIndex == 0 ? cycleLength : cycleIndex;

            var index = cycleStartIndex + cycleIndex - 1;
            var resultingStrMap = hash[index];
            var resultingMap = resultingStrMap.Split("\n").Select(line => line.ToCharArray()).ToArray();

            return GetLoad(resultingMap);
        }
    }
}
