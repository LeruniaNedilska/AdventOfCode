namespace AdventOfCode2023.Days {
    using System.Collections.Generic;
    using System.Linq;

    public class Day7(string[] input) : IPuzzle {
        private readonly string[] _input = input;

        public object SolveFirst() {
            return Solve(countJoker: false);
        }

        public object SolveSecond() {
            return Solve(countJoker: true);
        }

        private int Solve(bool countJoker) {
            var lines = GetInput().ToArray();
            var dict = Enum.GetValues(typeof(HandType))
                           .Cast<HandType>()
                           .ToDictionary(handType => handType, _ => new List<(string hand, int bid)>());

            foreach (var (hand, bid) in lines) {
                var handType = GetHandType(hand, countJoker);
                dict[handType].Add((hand, bid));
            }

            var total = 0;
            var rank = 1;
            foreach (var (handType, list) in dict) {
                list.Sort(new HandComparer(countJoker));

                foreach (var (hand, bid) in list) {
                    total += bid * rank;
                    rank++;
                }
            }

            return total;
        }

        private HandType GetHandType(string hand, bool countJoker = false) {
            var ranks = hand.Select(c => c switch {
                'T' => 10,
                'J' => countJoker ? 1 : 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => int.Parse(c.ToString())
            }).OrderByDescending(r => r).ToArray();

            var rankCounts = GetRankCounts(ranks, countJoker);
            if (rankCounts[0] == 5) {
                return HandType.FiveOfAKind;
            }

            if (rankCounts[0] == 4) {
                return HandType.FourOfAKind;
            }

            if (rankCounts[0] == 3 && rankCounts[1] == 2) {
                return HandType.FullHouse;
            }

            if (rankCounts[0] == 3) {
                return HandType.ThreeOfAKind;
            }

            if (rankCounts[0] == 2 && rankCounts[1] == 2) {
                return HandType.TwoPair;
            }

            if (rankCounts[0] == 2) {
                return HandType.OnePair;
            }

            return HandType.HighCard;
        }

        private int[] GetRankCounts(int[] ranks, bool countJoker = false) {
            var rankCounts = ranks
                .Where(r => r != 1)
                .GroupBy(r => r)
                .Select(g => g.Count())
                .OrderByDescending(c => c)
                .ToArray();

            if (countJoker) {
                var numberOfJokers = ranks.Count(r => r == 1);
                if (numberOfJokers == 5) {
                    return [5];
                }

                rankCounts[0] += numberOfJokers;
            }

            return rankCounts;
        }

        private IEnumerable<(string hand, int bid)> GetInput() {
            return _input.Select(line => {
                var parts = line.Split(' ');
                return (parts[0], int.Parse(parts[1]));
            });
        }

        enum HandType {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }

        class HandComparer(bool countJoker = false) : IComparer<(string hand, int bid)> {
            public int Compare((string hand, int bid) x, (string hand, int bid) y) {
                for (var i = 0; i < 5; i++) {
                    var c1 = GetCardRank(x.hand[i]);
                    var c2 = GetCardRank(y.hand[i]);
                    if (c1 == c2) {
                        continue;
                    }

                    return c1.CompareTo(c2);
                }

                return 0;
            }

            private int GetCardRank(char c) {
                return c switch {
                    'T' => 10,
                    'J' => countJoker ? 1 : 11,
                    'Q' => 12,
                    'K' => 13,
                    'A' => 14,
                    _ => int.Parse(c.ToString())
                };
            }
        }
    }
}
