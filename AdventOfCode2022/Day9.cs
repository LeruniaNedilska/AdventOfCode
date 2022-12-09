namespace AdventOfCode2022 {
    internal class Day9 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day9.txt");
        public static int GetVisitedPositions(int tailIndex) {
            var moves = input.Select(line => line.Split(' '));

            (int x, int y) currentHead = (0, 0);
            (int x, int y)[] tails = moves.Take(tailIndex).Select(x => (0, 0)).ToArray();

            var visitedPositions = new HashSet<(int x, int y)>();
            visitedPositions.Add(tails[tailIndex - 1]);

            foreach (var move in moves) {
                var moveCount = int.Parse(move[1]);
                if (move[0] == "R") {
                    for (var i = 0; i < moveCount; i++) {
                        currentHead = (currentHead.x, currentHead.y + 1);
                        MoveTails(currentHead, tails);
                        visitedPositions.Add(tails[tailIndex - 1]);
                    }
                }

                else if (move[0] == "L") {
                    for (var i = 0; i < moveCount; i++) {
                        currentHead = (currentHead.x, currentHead.y - 1);
                        MoveTails(currentHead, tails);
                        visitedPositions.Add(tails[tailIndex - 1]);
                    }
                }

                else if (move[0] == "U") {
                    for (var i = 0; i < moveCount; i++) {
                        currentHead = (currentHead.x + 1, currentHead.y);
                        MoveTails(currentHead, tails);
                        visitedPositions.Add(tails[tailIndex - 1]);
                    }
                }

                else if (move[0] == "D") {
                    for (var i = 0; i < moveCount; i++) {
                        currentHead = (currentHead.x - 1, currentHead.y);
                        MoveTails(currentHead, tails);
                        visitedPositions.Add(tails[tailIndex - 1]);
                    }
                }
            }

            return visitedPositions.Count();
        }

        private static void MoveTails((int x, int y) currentHead, (int x, int y)[] tails) {
            for (var j = 0; j < tails.Length; j++) {
                var head = j == 0 ? currentHead : tails[j - 1];
                var tail = tails[j];

                if (Math.Abs(head.x - tail.x) <= 1 && Math.Abs(head.y - tail.y) <= 1) {
                    return;
                }

                if (head.x == tail.x) {
                    tail.y = head.y > tail.y ? tail.y + 1 : tail.y - 1;
                }

                else if (head.y == tail.y) {
                    tail.x = head.x > tail.x ? tail.x + 1 : tail.x - 1;
                }

                else {
                    tail.x = head.x > tail.x ? tail.x + 1 : tail.x - 1;
                    tail.y = head.y > tail.y ? tail.y + 1 : tail.y - 1;
                }

                tails[j] = tail;
            }
        }
    }
}
