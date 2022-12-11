namespace AdventOfCode2022 {
    internal class Day11 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day11.txt");

        public static long GetMonkeyBusinessWithoutWorries() {
            var monkeys = GetMonkeys();
            IterateThroughRounds(20, monkeys, (item) => item, (item) => item / 3);
            return GetMonkeyBusiness(monkeys);
        }

        public static long GetMonkeyBusinessWithWorries() {
            var monkeys = GetMonkeys();
            int modulus = monkeys.Select(x => x.TestCondition).Aggregate((a, b) => a * b);
            IterateThroughRounds(10000, monkeys, (item) => item % modulus, (item) => item);
            return GetMonkeyBusiness(monkeys);
        }

        private static long GetMonkeyBusiness(Monkey[] monkeys) {
            return monkeys.Select(x => x.Counter).OrderByDescending(x => x).Take(2).Aggregate((a, b) => a * b);
        }

        private static void IterateThroughRounds(int roundsCount, Monkey[] monkeys, Func<long, long> decreaseWorryLevelsBeforeInspection, Func<long, long> decreaseWorryLevelsAfterInspection) {
            for (var i = 0; i < roundsCount; i++) {
                foreach (var monkey in monkeys) {
                    foreach (var item in monkey.Items) {
                        var worryLevel = decreaseWorryLevelsBeforeInspection(item);
                        if (monkey.Operation == '*') {
                            worryLevel = monkey.Operand == -1 ? worryLevel * worryLevel : worryLevel * monkey.Operand;
                        }
                        else {
                            worryLevel = monkey.Operand == -1 ? worryLevel + worryLevel : worryLevel + monkey.Operand;
                        }

                        worryLevel = decreaseWorryLevelsAfterInspection(worryLevel);
                        if (worryLevel % monkey.TestCondition == 0) {
                            monkeys[monkey.NextMonkeyIfTrue].Items.Add(worryLevel);
                        }
                        else {
                            monkeys[monkey.NextMonkeyIfFalse].Items.Add(worryLevel);
                        }

                        monkey.Counter++;
                    }

                    monkey.Items.Clear();
                }
            }
        }

        private static Monkey[] GetMonkeys() {
            var monkeys = new List<Monkey>();
            Monkey? currentMonkey = null;
            foreach (var line in input.Select((line, index) => (line: line.Trim(), index))) {
                switch (line.index % 7) {
                    case 0:
                        currentMonkey = new Monkey();
                        monkeys.Add(currentMonkey);
                        break;
                    case 1:
                        var items = line.line.Split(' ', ':', ',').Where(x => !string.IsNullOrWhiteSpace(x)).Skip(2).Select(x => long.Parse(x));
                        currentMonkey!.Items.AddRange(items);
                        break;
                    case 2:
                        var operations = line.line.Trim().Substring(16);
                        var operand = operations.Split(' ', '*', '+').Last();
                        currentMonkey!.Operation = operations.Contains('*') ? '*' : '+';
                        currentMonkey!.Operand = operand == "old" ? -1 : int.Parse(operand);
                        break;
                    case 3:
                        currentMonkey!.TestCondition = int.Parse(line.line.Substring(19));
                        break;
                    case 4:
                        currentMonkey!.NextMonkeyIfTrue = int.Parse(line.line.Substring(25));
                        break;
                    case 5:
                        currentMonkey!.NextMonkeyIfFalse = int.Parse(line.line.Substring(26));
                        break;
                    default:
                        break;
                }
            }
            return monkeys.ToArray();
        }

        private class Monkey {
            public List<long> Items { get; } = new List<long>();
            public char Operation { get; set; }
            public int Operand { get; set; }
            public int TestCondition { get; set; }
            public int NextMonkeyIfTrue { get; set; }
            public int NextMonkeyIfFalse { get; set; }
            public long Counter { get; set; }
        }
    }
}
