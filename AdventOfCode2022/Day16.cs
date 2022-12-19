namespace AdventOfCode2022 {
    using System.Text.RegularExpressions;

    internal class Day16 {
        private static readonly Dictionary<string, (int Rate, string[] Neighbours)> input = File.ReadAllLines(@"inputs\day16.txt")
            .Select(line => {
                var valve = line.Substring(6, 2);
                var rate = int.Parse(Regex.Match(line, @"\d+").Value);
                var tunnels = line.Split(';')[1].Split(new[] { " tunnels lead to valves ", " tunnel leads to valve " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                return (valve, rate, tunnels);
            })
            .ToDictionary(x => x.valve, x => (x.rate, x.tunnels));

        private static Dictionary<string, int> cache = new Dictionary<string, int>();

        public static int GetMaxPressureRelease() {
            var valuableValves = input.Where(x => x.Value.Rate > 0 || x.Key == "AA").Select(x => x.Key);
            var distances = GetDistances(valuableValves);
            var valves = valuableValves.Where(x => x != "AA").ToList();

            return DFS(distances, "AA", valves, 30);
        }

        public static int GetMaxPressureReleaseWithElephant() {
            var valuableValves = input.Where(x => x.Value.Rate > 0 || x.Key == "AA").Select(x => x.Key);
            var distances = GetDistances(valuableValves);
            var valves = valuableValves.Where(x => x!="AA").ToList();

            var result = 0;
            var options = GenerateHalfSets(valves);
            foreach (var option in options) {
                var elephantOptions = valves.Except(option).ToList();
                result = Math.Max(result, DFS(distances, "AA", option, 26) + DFS(distances, "AA", elephantOptions, 26));
            }

            return result;
        }

        private static IEnumerable<List<string>> GenerateHalfSets(List<string> valves) {
            for (int i = 0; i < (1 << valves.Count); i++) {
                List<string> subset = new List<string>();
                for (int j = 0; j < valves.Count; j++) {
                    if ((i & (1 << j)) > 0) {
                        subset.Add(valves[j]);
                    }
                }

                if (subset.Count == valves.Count / 2) {
                    yield return subset;
                }
            }
        }

        private static int DFS(Dictionary<(string, string), int> distance, string current, List<string> options, int minutes) {
            var cacheKey = $"{current} -> {string.Join(", ", options.OrderBy(x => x))} in {minutes}";
            if (cache.TryGetValue(cacheKey, out var lookup)) {
                return lookup;
            }

            var best = 0;
            var reducedOptions = new List<string>(options);
            reducedOptions.Remove(current);

            foreach (var next in reducedOptions) {
                if (distance[(current, next)] < minutes) {
                    var res = (minutes - distance[(current, next)] - 1) * input[next].Rate + DFS(distance, next, reducedOptions, minutes - distance[(current, next)] - 1);
                    if (best < res) {
                        best = res;
                    }
                }
            }

            cache.Add(cacheKey, best);
            return best;
        }

        private static Dictionary<(string, string), int> GetDistances(IEnumerable<string> valves) {
            Dictionary<(string, string), int> dict = new Dictionary<(string, string), int>();
            foreach (var i in valves) {
                foreach (var j in valves) {
                    if (i == j) {
                        continue;
                    }

                    dict.Add((i, j), BFS(i, j));
                }
            }

            return dict;
        }

        private static int BFS(string start, string end) {
            var queue = new Queue<(int distance, string valve)>();
            queue.Enqueue((0, start));
            var visited = new HashSet<string>();
            visited.Add(start);

            while (queue.Count > 0) {
                var node = queue.Dequeue();

                foreach (var nextNode in input[node.valve].Neighbours) {
                    if (visited.Contains(nextNode)) {
                        continue;
                    }

                    if (nextNode == end) {
                        return node.distance + 1;
                    }

                    visited.Add(nextNode);
                    queue.Enqueue((node.distance + 1, nextNode));
                }
            }

            return -1;
        }
    }
}
