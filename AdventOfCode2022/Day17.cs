namespace AdventOfCode2022 {
    internal class Day17 {
        private static readonly char[] input = File.ReadAllText(@"inputs\day17.txt").ToArray();
        private static readonly int[][] rocks = new int[][] {
            new int[] {
                0b0011110,
            },
            new int[] {
                0b0001000,
                0b0011100,
                0b0001000,
            },
            new int[] {
                0b0000100,
                0b0000100,
                0b0011100,
            },
            new int[] {
                0b0010000,
                0b0010000,
                0b0010000,
                0b0010000,
            },
            new int[] {
                0b0011000,
                0b0011000,
            }
        };

        public static long GetTowerHeight(long limit) {
            var chamber = new List<int> {
                0b1111111
            };

            long jetCount = 0;
            var hash = new Dictionary<(long rockIndex, int heightIncrease, long gustCycle), (long index, long fullHeight)>();
            long fasforwardedHeight = 0;
            bool cycleFound = false;

            for (long fallenRockCount = 0; fallenRockCount < limit;) {
                var currentRockSpace = rocks[fallenRockCount % 5].ToList();
                currentRockSpace.AddRange(new[] { 0b0000000, 0b0000000, 0b0000000 });
                currentRockSpace.Reverse();

                var heightIncrease = -1;
                var overlapWithChamber = -3;
                while (heightIncrease < 0) {
                    heightIncrease = MoveRock(currentRockSpace, chamber, input[jetCount % input.Length], overlapWithChamber < 0 ? 0: overlapWithChamber);
                    jetCount++;
                    overlapWithChamber++;
                }

                fallenRockCount++;

                if (!cycleFound) {
                    var key = (fallenRockCount % 5, heightIncrease, jetCount % input.Length);
                    if (hash.TryGetValue(key, out (long index, long fullHeight) value)) {
                        var cycleLength = fallenRockCount - value.index;
                        var cyclesToFastforward = (limit - fallenRockCount) / cycleLength;

                        fallenRockCount += cyclesToFastforward * cycleLength;
                        fasforwardedHeight = cyclesToFastforward * (chamber.Count - value.fullHeight);
                        cycleFound = true;
                    }
                    else {
                        hash.Add(key, (fallenRockCount, chamber.Count));
                    }
                }
            }

            return chamber.Count - 1 + fasforwardedHeight;
        }

        private static int MoveRock(List<int> rock, List<int> chamber, char move, int overlapWithChamber) {
            if (CanMove(rock, chamber, overlapWithChamber, move)) {
                for (var i = 0; i < rock.Count; i++) {
                    rock[i] = move == '<' ? rock[i] << 1 : rock[i] >> 1;
                }
            }

            if (rock.First() == 0) {
                rock.RemoveAt(0);
            }
            else {
                if (!CanFall(rock, chamber, overlapWithChamber)) {
                    return AddRockToChamber(rock, chamber, overlapWithChamber);
                }
            }

            return -1;
        }

        private static bool CanMove(List<int> rock, List<int> chamber, int overlapWithChamber, char move) {
            for (var i = rock.Count - 1; i >= 0; i--) {
                if ((move == '<' && (rock[i] & (1 << 6)) != 0) || (move == '>' && (rock[i] & 1) != 0)) {
                    return false;
                }

                var line = move == '<' ? rock[i] << 1 : rock[i] >> 1;
                var chamberLine = overlapWithChamber - i > 0 ? chamber[chamber.Count - overlapWithChamber + i] : 0b0000000;
                if ((line & chamberLine) != 0) {
                    return false;
                }
            }

            return true;
        }

        private static bool CanFall(List<int> rock, List<int> chamber, int overlapWithChamber) {
            for (var i = 0; i <= Math.Min(overlapWithChamber, rock.Count - 1); i++) {
                var chamberLine = chamber[chamber.Count - overlapWithChamber + i - 1];
                if ((rock[i] & chamberLine) != 0) {
                    return false;
                }
            }

            return true;
        }

        private static int AddRockToChamber(List<int> rock, List<int> chamber, int overlapWithChamber) {
            var initialHeight = chamber.Count;
            for (var i = 0; i < rock.Count; i++) {
                var chamberLine = overlapWithChamber - i > 0 ? chamber[chamber.Count - overlapWithChamber + i] : 0b0000000;
                rock[i] = rock[i] | chamberLine;
            }

            var removeIndex = chamber.Count - overlapWithChamber;
            if (overlapWithChamber > 0) {
                var toRemove = Math.Min(overlapWithChamber, rock.Count);
                chamber.RemoveRange(removeIndex, toRemove);
            }

            chamber.InsertRange(removeIndex, rock);
            return chamber.Count - initialHeight;
        }
    }
}
