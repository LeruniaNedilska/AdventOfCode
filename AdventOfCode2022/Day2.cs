namespace AdventOfCode2022 {
    internal class Day2 {
        public static GameStrategy Puzzle1 = new GameStrategy() {
            UnconditionalPoints = new Dictionary<string, int>() {
                {"X", 1 }, {"Y", 2 }, {"Z", 3 }
            },
            ConditionalPoints = new Dictionary<string, Dictionary<string, int>>(){
                { "A" ,new Dictionary<string, int> {{ "X", 3 }, { "Y",  6 }, {"Z", 0 } } },
                { "B" ,new Dictionary<string, int> {{ "X", 0 }, { "Y",  3 }, {"Z", 6 } } },
                { "C" ,new Dictionary<string, int> {{ "X", 6 }, { "Y",  0 }, {"Z", 3 } } },
            },
        };

        public static GameStrategy Puzzle2 = new GameStrategy() {
            UnconditionalPoints = new Dictionary<string, int>() {
                {"X", 0 }, {"Y", 3 }, {"Z", 6 }
            },
            ConditionalPoints = new Dictionary<string, Dictionary<string, int>>(){
                { "A" ,new Dictionary<string, int> {{ "X", 3 }, { "Y",  1 }, {"Z", 2 } } },
                { "B" ,new Dictionary<string, int> {{ "X", 1 }, { "Y",  2 }, {"Z", 3 } } },
                { "C" ,new Dictionary<string, int> {{ "X", 2 }, { "Y",  3 }, {"Z", 1 } } },
            },
        };

        public static int GetGameScore(GameStrategy strategy) {
            return File.ReadAllLines(@"inputs\day2.txt").Select(line => {
                string[] round = line.Split(' ');
                return strategy.UnconditionalPoints[round[1]] + strategy.ConditionalPoints[round[0]][round[1]];
            }).Sum();
        }
    }

    internal class GameStrategy {
        public Dictionary<string, int> UnconditionalPoints;
        public Dictionary<string, Dictionary<string, int>> ConditionalPoints;
    }
}
