namespace AdventOfCode2022 {
    internal class Day23 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day23.txt");

        public static int GetNumberOfEmptyTiles(int rounds) {
            var elves = GetElves();
            for(var i = 0; i < rounds; i++) {
                PerformRound(elves, i);
            }

            return (elves.Max(x => x.x) - elves.Min(x => x.x) + 1) * (elves.Max(x => x.y) - elves.Min(x => x.y) + 1) - elves.Count;
        }

        public static int GetNumberOfRounds() {
            var elves = GetElves();
            var done = false;
            var i = 0;
            while(!done) {
                var inPlaceElvesCount = PerformRound(elves, i);
                if (inPlaceElvesCount == elves.Count) {
                    done = true;
                }

                i++;
            }
            return i;
        }

        private static int PerformRound(HashSet<(int x, int y)> elves, int i) {
            var validProposals = new Dictionary<(int x, int y), (int x, int y)>();
            var duplicatedProposals = new HashSet<(int x, int y)>();
            var inPlaceElves = new HashSet<(int x, int y)>();

            foreach (var elf in elves) {
                (int x, int y)? proposal = null;
                for (var j = 0; j < 4; j++) {
                    proposal = GetProposal(elves, elf, (Direction)((i + j) % 4));
                    if (proposal.HasValue) {
                        break;
                    }
                }

                if (proposal.HasValue) {
                    if (duplicatedProposals.Contains(proposal.Value)) {
                        inPlaceElves.Add(elf);
                        continue;
                    }

                    if (validProposals.ContainsKey(proposal.Value)) {
                        var prevElf = validProposals[proposal.Value];
                        inPlaceElves.Add(prevElf);
                        inPlaceElves.Add(elf);

                        validProposals.Remove(proposal.Value);
                        duplicatedProposals.Add(proposal.Value);
                    }
                    else {
                        validProposals.Add(proposal.Value, elf);
                    }
                }
                else {
                    inPlaceElves.Add(elf);
                }
            }

            elves.Clear();
            elves.UnionWith(inPlaceElves);
            elves.UnionWith(validProposals.Keys);

            return inPlaceElves.Count;
        }

        private static HashSet<(int x, int y)> GetElves() {
            var elves = new HashSet<(int x, int y)>();
            foreach (var line in input.Select((x, index) => (x, index))) {
                foreach (var c in line.x.Select((x, index) => (x, index))) {
                    if (c.x == '#') {
                        elves.Add((line.index, c.index));
                    }
                }
            }
            return elves;
        }

        private static (int x, int y)? GetProposal(HashSet<(int x, int y)> elves, (int x, int y) elf, Direction direction) {
            if (new[] { (-1, -1), (-1, 0), (-1, 1), (1, -1), (1, 0), (1, 1), (0, -1), (0, 1) }.All(move => !elves.Contains((elf.x + move.Item1, elf.y + move.Item2)))) {
                return null;
            }

            switch (direction) {
                case Direction.North:
                    if (new[] { (-1, -1), (-1, 0), (-1, 1) }.All(move => !elves.Contains((elf.x + move.Item1, elf.y + move.Item2)))) {
                        return (elf.x - 1, elf.y);
                    }
                    break;
                case Direction.South:
                    if (new[] { (1, -1), (1, 0), (1, 1) }.All(move => !elves.Contains((elf.x + move.Item1, elf.y + move.Item2)))) {
                        return (elf.x + 1, elf.y);
                    }
                    break;
                case Direction.West:
                    if (new[] { (-1, -1), (0, -1), (1, -1) }.All(move => !elves.Contains((elf.x + move.Item1, elf.y + move.Item2)))) {
                        return (elf.x, elf.y - 1);
                    }
                    break;
                case Direction.East:
                    if (new[] { (-1, 1), (0, 1), (1, 1) }.All(move => !elves.Contains((elf.x + move.Item1, elf.y + move.Item2)))) {
                        return (elf.x, elf.y + 1);
                    }
                    break;
            }

            return null;
        }

        private enum Direction {
            North,
            South,
            West,
            East,
        }
    }
}
