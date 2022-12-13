namespace AdventOfCode2022 {
    using Newtonsoft.Json.Linq;

    internal class Day13 {
        private static readonly List<string> input = File.ReadAllLines(@"inputs\day13.txt").Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
        public static int GetIndecesSum() {
            var packetPairs = input.Select((x, index) => (x, index))
                .GroupBy(x => x.index / 2)
                .Select(x => x.Select(v => v.x).ToList());

            var result = 0;
            foreach (var packet in packetPairs.Select((pair, index) => (pair, index))) {
                var data = packet.pair.ToArray();
                if (ComparePackets(JToken.Parse(data[0]), JToken.Parse(data[1])) < 0) {
                    result += packet.index + 1;
                }
            }

            return result;
        }

        public static int GetDecoderKey() {
            input.Add("[[2]]");
            input.Add("[[6]]");
            input.Sort((a, b) => ComparePackets(JToken.Parse(a), JToken.Parse(b)));      
            return (input.IndexOf("[[2]]") + 1) * (input.IndexOf("[[6]]") + 1);
        }

        private static int ComparePackets(JToken a, JToken b) {
            if (a.Type == JTokenType.Integer && b.Type == JTokenType.Integer) {
                return a.ToObject<int>() - b.ToObject<int>();
            }

            if (a.Type == JTokenType.Integer) {
                return ComparePackets(JToken.Parse($"[{a}]"), b);
            }

            if (b.Type == JTokenType.Integer) {
                return ComparePackets(a, JToken.Parse($"[{b}]"));
            }

            var arrayA = (JArray)a;
            var arrayB = (JArray)b;
            for(var i = 0; i < Math.Min(arrayA.Count, arrayB.Count); i++) {
                var result = ComparePackets(arrayA[i], arrayB[i]);
                if (result != 0) {
                    return result;
                };
            }

            return arrayA.Count - arrayB.Count;
        }
    }
}
