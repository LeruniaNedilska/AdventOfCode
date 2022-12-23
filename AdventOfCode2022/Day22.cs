namespace AdventOfCode2022 {
    using System.Text.RegularExpressions;

    internal class Day22 {
        private static readonly string input = File.ReadAllText(@"inputs\day22.txt");

        public static int GetFinalPassword() {
            (char[][] map, PathDescription[] instructions) = GetInputData();
            (int x, int y) currentPosition = (0, map[0].Select((x, index) => (x, index)).First(x => x.x == '.').index);
            var currentDirection = Direction.Right;

            foreach (var instruction in instructions) {
                currentPosition = Move(map, currentPosition, currentDirection, instruction);
                currentDirection = Rotate(currentDirection, instruction);
            }

            return 1000 * (currentPosition.x + 1) + 4 * (currentPosition.y + 1) + (int)currentDirection;
        }

        private static (int x, int y) Move(char[][] map, (int x, int y) currentPosition, Direction currentDirection, PathDescription path) {
            var position = currentPosition;
            switch (currentDirection) {
                case Direction.Right:
                    for (var i = 1; i <= path.Movement; i++) {
                        if (position.y + 1 < map[position.x].Length && map[position.x][position.y + 1] != ' ') {
                            if (map[position.x][position.y + 1] == '#') {
                                return position;
                            }
                            position.y += 1;
                        }
                        else {
                            var firstColumn = GetFirstColumn(map, position.x);
                            if (firstColumn.value == '#') {
                                return position;
                            }
                            position.y = firstColumn.index;
                        }
                    }
                    break;
                case Direction.Left:
                    for (var i = 1; i <= path.Movement; i++) {
                        if (position.y - 1 >= 0 && map[position.x][position.y - 1] != ' ') {
                            if (map[position.x][position.y - 1] == '#') {
                                return position;
                            }
                            position.y -= 1;
                        }
                        else {
                            var lastColumn = GetLastColumn(map, position.x);
                            if (lastColumn.value == '#') {
                                return position;
                            }
                            position.y = lastColumn.index;
                        }
                    }
                    break;
                case Direction.Down:
                    for (var i = 1; i <= path.Movement; i++) {
                        if (position.x + 1 < map.Length && map[position.x + 1][position.y] != ' ') {
                            if (map[position.x + 1][position.y] == '#') {
                                return position;
                            }
                            position.x += 1;
                        }
                        else {
                            var firstRow = GetFirstRow(map, position.y);
                            if (firstRow.value == '#') {
                                return position;
                            }
                            position.x = firstRow.index;
                        }
                    }
                    break;
                case Direction.Up:
                    for (var i = 1; i <= path.Movement; i++) {
                        if (position.x - 1 >= 0 && map[position.x - 1][position.y] != ' ') {
                            if (map[position.x - 1][position.y] == '#') {
                                return position;
                            }
                            position.x -= 1;
                        }
                        else {
                            var lastRow = GetLastRow(map, position.y);
                            if (lastRow.value == '#') {
                                return position;
                            }
                            position.x = lastRow.index;
                        }
                    }
                    break;
                default:
                    break;
            }
            return position;
        }

        private static Direction Rotate(Direction currentDirection, PathDescription path) {
            return path.DirectionChange switch {
                "R" => RotateRight(currentDirection),
                "L" => RotateLeft(currentDirection),
                _ => currentDirection,
            };
        }

        private static Direction RotateRight(Direction currentDirection) {
            return currentDirection switch {
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => Direction.Right,
            };
        }

        private static Direction RotateLeft(Direction currentDirection) {
            return currentDirection switch {
                Direction.Right => Direction.Up,
                Direction.Up => Direction.Left,
                Direction.Left => Direction.Down,
                _ => Direction.Right,
            };
        }

        private static (char value, int index) GetFirstColumn(char[][] map, int row) {
            return map[row].Select((x, index) => (x, index)).First(x => x.x != ' ');
        }

        private static (char value, int index) GetLastColumn(char[][] map, int row) {
            return map[row].Select((x, index) => (x, index)).Last(x => x.x != ' ');
        }

        private static (char value, int index) GetFirstRow(char[][] map, int column) {
            for (var x = 0; x < map.Length; x++) {
                if (map[x][column] != ' ') {
                    return (map[x][column], x);
                }
            }

            throw new Exception("Unexpected input configuration");
        }

        private static (char value, int index) GetLastRow(char[][] map, int column) {
            for (var x = map.Length - 1; x >= 0; x--) {
                if (map[x][column] != ' ') {
                    return (map[x][column], x);
                }
            }

            throw new Exception("Unexpected input configuration");
        }

        private static (char[][] map, PathDescription[] instructions) GetInputData() {
            var data = input.Split("\r\n\r\n");

            var stringArray = data[0].Split("\r\n");
            var height = stringArray.Length;
            var width = stringArray.Max(x => x.Length);

            char[][] map = new char[height][];
            for (var x = 0; x < height; x++) {
                var line = stringArray[x];
                map[x] = new char[width];
                for (var y = 0; y < line.Length; y++) {
                    map[x][y] = line[y];
                }
                for (var y = line.Length; y < width; y++) {
                    map[x][y] = ' ';
                }
            }

            Regex re = new Regex(@"(\d+)([A-Z])");
            var instructions = re.Matches(data[1])
                .Select(x => new PathDescription { Movement = int.Parse(x.Groups[1].Value), DirectionChange = x.Groups[2].Value })
                .ToList();

            Regex re2 = new Regex(@"\d+");
            var lastInstruction = new PathDescription { Movement = int.Parse(re2.Matches(data[1]).Last().Value) };
            instructions.Add(lastInstruction);

            return (map, instructions.ToArray());
        }

        private class PathDescription {
            public int Movement { get; set; }

            public string? DirectionChange { get; set; }
        }

        private enum Direction {
            Right,
            Down,
            Left,
            Up
        }
    }
}
