namespace AdventOfCode2022 {
    using System.Text.RegularExpressions;

    internal class Day5 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day5.txt");

        public static string GetTopCrates<TCrane>() where TCrane : ICrateMover, new() {
            var crateMover = new TCrane();
            crateMover.LoadCrates(input);
            crateMover.MoveCrates(input);
            return crateMover.GetTopCrates();
        }
    }

    internal interface ICrateMover {
        void LoadCrates(string[] input);
        void MoveCrates(string[] input);
        string GetTopCrates();
    }

    internal class CrateMover : ICrateMover {
        protected Stack<char>[]? stacks;

        public void LoadCrates(string[] input) {
            var crateLines = input
                .Where(line => line.StartsWith('['))
                .Reverse()
                .Select(line => line.Where((item, index) => (index - 1) % 4 == 0).Select((crate, stack) => (crate, stack)));

            stacks = crateLines.First().Select(crate => new Stack<char>()).ToArray();
            foreach (var crates in crateLines) {
                foreach (var item in crates) {
                    if (char.IsLetter(item.crate)) {
                        stacks[item.stack].Push(item.crate);
                    }
                }
            }
        }

        public string GetTopCrates() => stacks == null ? string.Empty : string.Concat(stacks.Select(x => x.Pop()));

        public virtual void MoveCrates(string[] input) { }
    }

    internal class CrateMover9000 : CrateMover {
        public override void MoveCrates(string[] input) {
            if (stacks == null) {
                return;
            }

            var moves = input.Where(line => line.StartsWith('m'));
            foreach (var move in moves) {
                var numbers = Regex.Split(move, @"\D+")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => int.Parse(x))
                    .ToArray();

                for (var i = 0; i < numbers[0]; i++) {
                    var pop = stacks[numbers[1] - 1].Pop();
                    stacks[numbers[2] - 1].Push(pop);
                }
            }
        }
    }

    internal class CrateMover9001 : CrateMover {
        public override void MoveCrates(string[] input) {
            if (stacks == null) {
                return;
            }

            var moves = input.Where(line => line.StartsWith('m'));
            foreach (var move in moves) {
                var numbers = Regex.Split(move, @"\D+")
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => int.Parse(x))
                    .ToArray();

                var movedCrates = new Stack<char>();
                for (var i = 0; i < numbers[0]; i++) {
                    var pop = stacks[numbers[1] - 1].Pop();
                    movedCrates.Push(pop);
                }

                foreach (var pop in movedCrates) {
                    stacks[numbers[2] - 1].Push(pop);
                }
            }
        }
    }
}
