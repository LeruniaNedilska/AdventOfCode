namespace AdventOfCode2022 {
    internal class Day12 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day12.txt");
        private static readonly char[][] map = input
            .Select(l => l.ToArray())
            .ToArray();

        public static int GetMinimalDistanceBetweenNodes(char startNode, char endNode, bool reverse = false) {
            (int x, int y) start = (0, 0);

            foreach (var line in input.Select((line, index) => (line, index))) {
                if (line.line.Contains(startNode)) {
                    start.x = line.index;
                    start.y = line.line.IndexOf(startNode);
                }
            }

            Func<char, char, bool> isUnreachable = reverse ? (a, b) => GetNodeValue(b) - GetNodeValue(a) > 1 : (a, b) => GetNodeValue(a) - GetNodeValue(b) > 1;
            return BFS(start, endNode, isUnreachable);
        }

        private static int BFS((int x, int y) start, char end, Func<char, char, bool> isUnreachable) {
            var queue = new Queue<QueueItem>();
            queue.Enqueue(new QueueItem(0, start));
            var visited = new HashSet<(int x, int y)>();
            visited.Add(start);

            while (queue.Count > 0) {
                var node = queue.Dequeue();

                foreach (var nextNode in GetAdjacentNodes(node.Node, isUnreachable)) {
                    if (visited.Contains(nextNode)) {
                        continue;
                    }

                    if (map[nextNode.x][nextNode.y] == end) {
                        return node.Distance + 1;
                    }

                    visited.Add(nextNode);
                    queue.Enqueue(new QueueItem(node.Distance + 1, nextNode));
                }
            }

            return -1;
        }

        private static IEnumerable<(int x, int y)> GetAdjacentNodes((int x, int y) node, Func<char, char, bool> isUnreachable) {
            (int x, int y)[] moves = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            foreach (var move in moves) {
                (int x, int y) adjacentNode = (node.x + move.x, node.y + move.y);
                if (adjacentNode.x >= 0 && adjacentNode.y >= 0 && adjacentNode.x < map.Length && adjacentNode.y < map[0].Length && !isUnreachable(map[adjacentNode.x][adjacentNode.y], map[node.x][node.y])) {
                    yield return adjacentNode;
                }
            }
        }

        private static char GetNodeValue(char node) {
            return node == 'S' ? 'a' : node == 'E' ? 'z' : node;
        }

        private class QueueItem {
            public QueueItem(int distance, (int x, int y) node) {
                Distance = distance;
                Node = node;
            }

            public int Distance { get; }

            public (int x, int y) Node { get; }
        }
    }
}
