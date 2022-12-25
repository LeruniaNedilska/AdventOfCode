namespace AdventOfCode2022 {
    internal class Day25 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day25.txt");
        public static string GetSnafuNumber() {
            return ToSnafu(input.Select(line => FromSnafu(line)).Sum());
        }

        private static string ToSnafu(long number) {
            var result = "";
            while (number > 0) {
                switch (number % 5) {
                    case 0: result += "0"; break;
                    case 1: result += "1"; break;
                    case 2: result += "2"; break;
                    case 3: result += "="; number += 5; break;
                    case 4: result += "-"; number += 5; break;
                }
                number /= 5;
            }

            var charArray = result.Reverse().ToArray();
            return new string(charArray);
        }

        private static long FromSnafu(string line) {
            return line.Select(x => x switch {
                '0' => (long)0,
                '1' => (long)1,
                '2' => (long)2,
                '-' => (long)-1,
                _ => (long)-2,
            }).Aggregate((a, b) => a * 5 + b);
        }
    }
}