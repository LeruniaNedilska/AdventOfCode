namespace AdventOfCode2022 {
    internal class Day21 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day21.txt");

        public static long? GetRootMonkeyNumber() {
            var monkeys = GetMonkeys();
            return GetMonkeyNumber(monkeys, "root");
        }

        public static long? GetHumanNumber() {
            var monkeys = GetMonkeys();
            var human = monkeys.Single(x => x.Name == "humn");
            human.Number = null;

            GetMonkeyNumber(monkeys, "root");
            var rootMonkey = monkeys.Single(x => x.Name == "root");
            var subRootMonkey1 = monkeys.Single(x => x.Name == rootMonkey.Monkey1);
            var subRootMonkey2 = monkeys.Single(x => x.Name == rootMonkey.Monkey2);

            if (subRootMonkey1.Number.HasValue) {
                return GetHumanNumber(monkeys, subRootMonkey2, subRootMonkey1.Number.Value);
            }

            if (subRootMonkey2.Number.HasValue) {
                return GetHumanNumber(monkeys, subRootMonkey1, subRootMonkey2.Number.Value);
            }

            throw new Exception("Unexpected input configuration");
        }

        private static long? GetMonkeyNumber(Monkey[] monkeys, string name) {
            var monkey = monkeys.Single(x => x.Name == name);

            if (!monkey.Number.HasValue && !monkey.Operation.HasValue) {
                return null;
            }

            if (monkey.Number.HasValue) {
                return monkey.Number.Value;
            }

            var monkey1 = GetMonkeyNumber(monkeys, monkey.Monkey1!);
            var monkey2 = GetMonkeyNumber(monkeys, monkey.Monkey2!);
            if (!monkey1.HasValue || !monkey2.HasValue) {
                monkey.Number = null;
                return null;
            }

            monkey.Number = monkey.Operation == '+' ? monkey1 + monkey2 : monkey.Operation == '-' ? monkey1 - monkey2 : monkey.Operation == '*' ? monkey1 * monkey2 : monkey1 / monkey2;
            return monkey.Number;
        }

        private static long GetHumanNumber(Monkey[] monkeys, Monkey monkey, long target) {
            var monkey1 = monkeys.Single(x => x.Name == monkey.Monkey1);
            var monkey2 = monkeys.Single(x => x.Name == monkey.Monkey2);

            long newTarget = 0;
            Monkey targetMonkey = monkey1;
            if (!monkey1.Number.HasValue && monkey2.Number.HasValue) {
                newTarget = monkey.Operation == '+' ? target - monkey2.Number.Value : monkey.Operation == '-' ? target + monkey2.Number.Value : monkey.Operation == '*' ? target / monkey2.Number.Value : target * monkey2.Number.Value;
            }

            if (!monkey2.Number.HasValue && monkey1.Number.HasValue) {
                targetMonkey = monkey2;
                newTarget = monkey.Operation == '+' ? target - monkey1.Number.Value : monkey.Operation == '-' ? monkey1.Number.Value - target : monkey.Operation == '*' ? target / monkey1.Number.Value : monkey1.Number.Value / target;
            }

            if (!monkey1.Number.HasValue && !monkey2.Number.HasValue) {
                throw new Exception("Unexpected input configuration");
            }

            if (targetMonkey.Name == "humn") {
                return newTarget;
            }

            return GetHumanNumber(monkeys, targetMonkey, newTarget);
        }

        private static Monkey[] GetMonkeys() {
            return input
                .Select(line => {
                    var chunks = line.Split(':');
                    var name = chunks[0];
                    long? number = null;
                    char? operation = null;
                    string? monkey1 = null;
                    string? monkey2 = null;
                    if (long.TryParse(chunks[1].Trim(), out var result)) {
                        number = result;
                    }
                    else {
                        monkey1 = chunks[1].Substring(1, 4);
                        operation = chunks[1][6];
                        monkey2 = chunks[1].Substring(8, 4);
                    }
                    return new Monkey { Name = name, Number = number, Operation = operation, Monkey1 = monkey1, Monkey2 = monkey2 };
                })
                .ToArray();
        }

        private class Monkey {
            public string? Name { get; set; }
            public long? Number { get; set; }
            public char? Operation { get; set; }
            public string? Monkey1 { get; set; }
            public string? Monkey2 { get; set; }
        }
    }
}
