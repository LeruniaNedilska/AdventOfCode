namespace AdventOfCode2022 {
    internal class Day8 {
        private static readonly int[][] forest = File.ReadAllLines(@"inputs\day8.txt")
                   .Select(l => l.Select(i => int.Parse(i.ToString())).ToArray())
                   .ToArray();
        public static int GetVisibleTreesCount() {
            var result = 0;
            for (var i = 0; i < forest.Length; i++) {
                for (var j = 0; j < forest[i].Length; j++) {
                    if (i == 0 || j == 0 || i == forest.Length - 1 || j == forest[i].Length - 1) {
                        result++;
                        continue;
                    }

                    var tree = forest[i][j];
                    var x = 0;
                    while (x < i && forest[x][j] < tree) {
                        x++;
                    }
                    if (x == i) {
                        result++;
                        continue;
                    }

                    x = i + 1;
                    while (x < forest.Length && forest[x][j] < tree) {
                        x++;
                    }
                    if (x == forest.Length) {
                        result++;
                        continue;
                    }

                    x = 0;
                    while (x < j && forest[i][x] < tree) {
                        x++;
                    }
                    if (x == j) {
                        result++;
                        continue;
                    }

                    x = j + 1;
                    while(x < forest[i].Length && forest[i][x] < tree) {
                        x++;
                    }
                    if (x == forest[i].Length) {
                        result++;
                    }
                }
            }

            return result;
        }


        public static int GetMaxTreeScenicScore() {
            var result = 0;
            for (var i = 1; i < forest.Length; i++) {
                for (var j = 1; j < forest[i].Length; j++) {
                    var score = 1;
                    for (var x = i - 1; x >= 0; x--) {
                        if (x == 0 || forest[i][j] <= forest[x][j]) {
                            score*= i - x;
                            break;
                        }
                    }

                    for (var x = i + 1; x < forest.Length; x++) {
                        if (x == forest.Length - 1 || forest[i][j] <= forest[x][j]) {
                            score *= x - i;
                            break;
                        }
                    }


                    for (var x = j - 1; x >= 0; x--) {
                        if (x == 0 || forest[i][j] <= forest[i][x]) {
                            score *= j - x;
                            break;
                        }
                    }

                    for (var x = j + 1; x < forest[i].Length; x++) {
                        if (x == forest[i].Length - 1 || forest[i][j] <= forest[i][x]) {
                            score *= x - j;
                            break;
                        }
                    }

                    result = result > score ? result : score;
                }
            }

            return result;
        }
    }
}
