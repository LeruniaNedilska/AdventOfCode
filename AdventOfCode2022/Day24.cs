namespace AdventOfCode2022 {
    internal class Day24 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day24.txt");

        public static int GetNumberOfMinutesToReachGoal() {
            int height = input.Length - 2;
            int width = input[0].Length - 2;

            (int x, int y) start = (-1, 0);
            (int x, int y) end = (height, width - 1);

            var map = input.SelectMany((line, x) => line.Select((cell, y) => (cell, (x - 1, y - 1))))
                .Where(y => y.cell != '#' && y.cell != '.')
                .ToArray();

            return BFS(0, start, end, map);
        }

        public static int GetNumberOfMinutesToReachGoalTwice() {
            int height = input.Length - 2;
            int width = input[0].Length - 2;

            (int x, int y) start = (-1, 0);
            (int x, int y) end = (height, width - 1);

            var map = input.SelectMany((line, x) => line.Select((cell, y) => (cell, (x - 1, y - 1))))
                .Where(y => y.cell != '#' && y.cell != '.')
                .ToArray();

            var res = BFS(0, start, end, map);
            res = BFS(res, end, start, map);
            res = BFS(res, start, end, map);
            return res;
        }

        private static int BFS(int startTime, (int x, int y) start, (int x, int y) end, (char cell, (int x, int y) coordinates)[] map) {
            var cache = new Dictionary<int, HashSet<(int x, int y)>>();
            var queue = new PriorityQueue<(int time, (int x, int y) point), int>();
            queue.Enqueue((startTime, start), startTime);
            var visited = new HashSet<(int time, (int x, int y))>();

            while (queue.Count > 0) {
                var item = queue.Dequeue();

                foreach (var nextNode in GetAdjacentNodes(item.point, end, start, item.time, map, cache)) {
                    if (nextNode == end) {
                        return item.time + 1;
                    }

                    if (!visited.Contains((item.time + 1, nextNode))) {
                        visited.Add((item.time + 1, nextNode));
                        queue.Enqueue((item.time + 1, nextNode), item.time + 1);
                    }
                }
            }

            return -1;
        }

        private static HashSet<(int x, int y)> GetBlizzards((char cell, (int x, int y) coordinates)[] map, int time, Dictionary<int, HashSet<(int x, int y)>> cache) {
            if (cache.TryGetValue(time, out var lookup)) {
                return lookup;
            }

            var result = new HashSet<(int x, int y)>();
            for (var i = 0; i < map.Length; i++) {
                map[i] = (map[i].cell, GetNewBlizzardCoordinates(map[i].cell, map[i].coordinates));
                result.Add(map[i].coordinates);
            }

            cache.Add(time, result);
            return result;
        }

        private static IEnumerable<(int x, int y)> GetAdjacentNodes((int x, int y) node, (int x, int y) end, (int x, int y) start, int time, (char cell, (int x, int y) coordinates)[] map, Dictionary<int, HashSet<(int x, int y)>> cache) {
            (int x, int y)[] moves = new[] { (1, 0), (-1, 0), (0, 1), (0, -1), (0, 0) };
            var blizzards = GetBlizzards(map, time, cache);
            foreach (var move in moves) {
                (int x, int y) adjacentNode = (node.x + move.x, node.y + move.y);
                if (adjacentNode == end || adjacentNode == start) {
                    yield return adjacentNode;
                }

                if (adjacentNode.x >= 0 && adjacentNode.y >= 0 && adjacentNode.x < input.Length - 2 && adjacentNode.y < input[0].Length - 2 && !blizzards.Contains(adjacentNode)) {
                    yield return adjacentNode;
                }
            }
        }

        private static (int x, int y) GetNewBlizzardCoordinates(char direction, (int x, int y) coordinates) {
            var result = direction switch {
                '>' => (coordinates.x, coordinates.y + 1),
                '<' => (coordinates.x, coordinates.y - 1),
                'v' => (coordinates.x + 1, coordinates.y),
                _ => (coordinates.x - 1, coordinates.y),
            };

            return Wrap(result);
        }

        private static (int x, int y) Wrap((int x, int y) point) {
            return ((point.x + input.Length - 2) % (input.Length - 2), (point.y + input[0].Length - 2) % (input[0].Length - 2));
        }

        private class QueueItem {
            public QueueItem(int time, (int x, int y) node) {
                Time = time;
                Node = node;
            }
            public int Time { get; }
            public (int x, int y) Node { get; }
        }
    }
}
