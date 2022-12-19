namespace AdventOfCode2022 {
    internal class Day18 {
        private static readonly HashSet<(int x, int y, int z)> input = File.ReadAllLines(@"inputs\day18.txt")
            .Select(line => {
                var coordinates = line.Split(',');
                return (int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
            })
            .ToHashSet();

        public static int GetSurfaceArea(HashSet<(int x, int y, int z)>? waterCubes = null) {
            (int x, int y, int z)[] moves = new[] { (1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1) };
            var result = 0;
            foreach (var cube in input) {
                result += 6;
                foreach (var move in moves) {
                    if (input.Contains((cube.x + move.x, cube.y + move.y, cube.z + move.z)) || (!waterCubes?.Contains((cube.x + move.x, cube.y + move.y, cube.z + move.z)) ?? false)) {
                        result--;
                    }
                }
            }

            return result;
        }

        public static int GetExteriorSurfaceArea() {
            (int maxX, int maxY, int maxZ) maxBounds = (input.MaxBy(c => c.x).x + 1, input.MaxBy(c => c.y).y + 1, input.MaxBy(c => c.z).z + 1);
            (int minX, int minY, int minZ) minBounds = (input.MinBy(c => c.x).x - 1, input.MinBy(c => c.y).y - 1, input.MinBy(c => c.z).z - 1);

            var waterCubes = new HashSet<(int x, int y, int z)>();
            DFS(minBounds, waterCubes, maxBounds, minBounds);

            return GetSurfaceArea(waterCubes);
        }

        private static void DFS((int x, int y, int z) current, HashSet<(int x, int y, int z)> waterCubes, (int maxX, int maxY, int maxZ) maxBounds, (int minX, int minY, int minZ) minBounds) {
            var stack = new Stack<(int x, int y, int z)>();
            (int x, int y, int z)[] moves = new[] { (1, 0, 0), (-1, 0, 0), (0, 1, 0), (0, -1, 0), (0, 0, 1), (0, 0, -1) };
            stack.Push(current);

            while(stack.Count > 0) {
                (int x, int y, int z) currentCube = stack.Pop();
                foreach (var move in moves) {
                    (int x, int y, int z) checkedCube = (currentCube.x + move.x, currentCube.y + move.y, currentCube.z + move.z);
                    if (checkedCube.x < minBounds.minX || checkedCube.x > maxBounds.maxX ||
                        checkedCube.y < minBounds.minY || checkedCube.y > maxBounds.maxY ||
                        checkedCube.z < minBounds.minZ || checkedCube.z > maxBounds.maxZ) {
                        continue;
                    }

                    if (!waterCubes.Contains(checkedCube) && !input.Contains(checkedCube)) {
                        waterCubes.Add(checkedCube);
                        stack.Push(checkedCube);
                    }
                }
            }
        }
    }
}
