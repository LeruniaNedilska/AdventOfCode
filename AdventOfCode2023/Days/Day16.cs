namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day16(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            var map = _input.Select(line => line.ToCharArray()).ToArray();
            return GetEnergizedNodeCount((0, 0, Direction.Right), map);
        }

        public object SolveSecond() {
            var map = _input.Select(line => line.ToCharArray()).ToArray();
            return GetStartingPositions(map)
                .Select(start => GetEnergizedNodeCount(start, map))
                .Max();
        }

        private static int GetEnergizedNodeCount((int x, int y, Direction direction) start, char[][] map) {
            var queue = new Queue<(int x, int y, Direction direction)>();
            queue.Enqueue(start);
            var energized = new HashSet<(int x, int y, Direction direction)> {
                start
            };

            while (queue.Count > 0) {
                var node = queue.Dequeue();

                foreach (var nextNode in GetNextNodes(node, map)) {
                    if (energized.Contains(nextNode)) {
                        continue;
                    }

                    energized.Add(nextNode);
                    queue.Enqueue(nextNode);
                }
            }

            return energized
                .Select(node => (node.x, node.y))
                .Distinct()
                .Count();
        }

        private static IEnumerable<(int x, int y, Direction direction)> GetNextNodes((int x, int y, Direction direction) current, char[][] map) {
            var maxX = map[0].Length;
            var maxY = map.Length;

            if (IsPathClear(current, map)) {
                var (x, y) = GetNextPosition(current);
                if (IsOutOfBounds(x, y, maxX, maxY)) {
                    return [];
                }
                return [(x, y, current.direction)];
            }

            if (map[current.y][current.x] == '|') {
                return GetMoves(current, map, (0, 1, Direction.Down), (0, -1, Direction.Up));
            }

            if (map[current.y][current.x] == '-') {
                return GetMoves(current, map, (1, 0, Direction.Right), (-1, 0, Direction.Left));
            }

            if (map[current.y][current.x] == '/' || map[current.y][current.x] == '\\') {
                return GetDiagonalMove(current, map);
            }

            throw new Exception("Invalid path");
        }

        private static bool IsPathClear((int x, int y, Direction direction) current, char[][] map) {
            return map[current.y][current.x] == '.' ||
                   (map[current.y][current.x] == '|' && (current.direction == Direction.Up || current.direction == Direction.Down)) ||
                   (map[current.y][current.x] == '-' && (current.direction == Direction.Left || current.direction == Direction.Right));
        }

        private static (int x, int y) GetNextPosition((int x, int y, Direction direction) current) {
            return current.direction switch {
                Direction.Right => (current.x + 1, current.y),
                Direction.Left => (current.x - 1, current.y),
                Direction.Up => (current.x, current.y - 1),
                Direction.Down => (current.x, current.y + 1),
                _ => throw new Exception("Invalid direction")
            };
        }

        private static bool IsOutOfBounds(int x, int y, int maxX, int maxY) {
            return x < 0 || x >= maxX || y < 0 || y >= maxY;
        }

        private static List<(int x, int y, Direction direction)> GetMoves((int x, int y, Direction direction) current, char[][] map, params (int x, int y, Direction direction)[] moves) {
            var result = new List<(int x, int y, Direction direction)>();
            foreach (var (x, y, direction) in moves) {
                var (nextX, nextY, nextDirection) = (current.x + x, current.y + y, direction);
                if (!IsOutOfBounds(nextX, nextY, map[0].Length, map.Length)) {
                    result.Add((nextX, nextY, nextDirection));
                }
            }
            return result;
        }
        private static IEnumerable<(int x, int y, Direction direction)> GetDiagonalMove((int x, int y, Direction direction) current, char[][] map) {
            var (x, y, newDirection) = current.direction switch {
                Direction.Right when map[current.y][current.x] == '/' => (current.x, current.y - 1, Direction.Up),
                Direction.Right when map[current.y][current.x] == '\\' => (current.x, current.y + 1, Direction.Down),
                Direction.Left when map[current.y][current.x] == '/' => (current.x, current.y + 1, Direction.Down),
                Direction.Left when map[current.y][current.x] == '\\' => (current.x, current.y - 1, Direction.Up),
                Direction.Up when map[current.y][current.x] == '/' => (current.x + 1, current.y, Direction.Right),
                Direction.Up when map[current.y][current.x] == '\\' => (current.x - 1, current.y, Direction.Left),
                Direction.Down when map[current.y][current.x] == '/' => (current.x - 1, current.y, Direction.Left),
                Direction.Down when map[current.y][current.x] == '\\' => (current.x + 1, current.y, Direction.Right),
                _ => throw new Exception("Invalid direction")
            };

            if (IsOutOfBounds(x, y, map[0].Length, map.Length)) {
                return [];
            }

            return [(x, y, newDirection)];
        }

        private List<(int x, int y, Direction direction)> GetStartingPositions(char[][] map) {
            List<(int x, int y, Direction direction)> starts = [];
            for (var i = 0; i < map.Length - 1; i++) {
                starts.Add((0, i, Direction.Right));
                starts.Add((map[0].Length - 1, i, Direction.Left));
            }
            for (var i = 0; i < map[0].Length - 1; i++) {
                starts.Add((i, 0, Direction.Down));
                starts.Add((i, map.Length - 1, Direction.Up));
            }

            return starts;
        }


        enum Direction {
            Up,
            Down,
            Left,
            Right
        }
    }
}
