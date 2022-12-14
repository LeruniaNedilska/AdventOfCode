namespace AdventOfCode2022 {
    internal class Day14 {
        private static readonly string[][] input = File.ReadAllLines(@"inputs\day14.txt")
            .Select(line => line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        public static int GetSandUnitCountBeforeAbyss() {
            var occupiedCoordinates = GetRocks();
            var maxY = occupiedCoordinates.Max(coord => coord.y);

            var result = 0;
            (int x, int y) sand = (500, 0);
            while (sand.y <= maxY) {
                var movedSand = MoveSand(occupiedCoordinates, sand);
                if (movedSand == sand) {
                    occupiedCoordinates.Add(sand);
                    sand = (500, 0);
                    result++;
                }
                else {
                    sand = movedSand;
                }
            }

            return result;
        }

        public static int GetSandUnitCountBeforeBlocked() {
            var occupiedCoordinates = GetRocks();
            var floorY = occupiedCoordinates.Max(coord => coord.y) + 2;

            var result = 0;
            (int x, int y) sand = (500, 0);
            while (!occupiedCoordinates.Contains((500,0))) {
                if (sand.y + 1 >= floorY) {
                    occupiedCoordinates.Add(sand);
                    sand = (500, 0);
                    result++;
                }

                var movedSand = MoveSand(occupiedCoordinates, sand);
                if (movedSand == sand) {
                    occupiedCoordinates.Add(sand);
                    sand = (500, 0);
                    result++;
                }
                else {
                    sand = movedSand;
                }
            }

            return result;
        }

        private static (int x, int y) MoveSand(HashSet<(int x, int y)> occupiedCoordinates, (int x, int y) sand) {
            if (occupiedCoordinates.Contains((sand.x, sand.y + 1))) {
                if (occupiedCoordinates.Contains((sand.x - 1, sand.y + 1))) {
                    if (occupiedCoordinates.Contains((sand.x + 1, sand.y + 1))) {
                        return sand;
                    }
                    else {
                        return (sand.x + 1, sand.y + 1);
                    }
                }
                else {
                    return (sand.x - 1, sand.y + 1);
                }
            }
            else {
                return (sand.x, sand.y + 1);
            }
        }

        private static HashSet<(int x, int y)> GetRocks() {
            var result = new HashSet<(int x, int y)>();
            foreach (var line in input) {
                for (var n = 0; n < line.Length - 1; n++) {
                    var coord1 = line[n].Split(',').Select(x => int.Parse(x)).ToArray();
                    var coord2 = line[n + 1].Split(',').Select(x => int.Parse(x)).ToArray();

                    if (coord1[0] == coord2[0]) {
                        for (var m = Math.Min(coord1[1], coord2[1]); m <= Math.Max(coord1[1], coord2[1]); m++) {
                            result.Add((coord1[0], m));                         
                        }
                    }
                    else {
                        for (var m = Math.Min(coord1[0], coord2[0]); m <= Math.Max(coord1[0], coord2[0]); m++) {
                            result.Add((m, coord1[1]));
                        }
                    }
                }
            }

            return result;
        }
    }
}
