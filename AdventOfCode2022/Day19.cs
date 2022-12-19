namespace AdventOfCode2022 {
    using System.Text.RegularExpressions;

    internal class Day19 {
        private static readonly Blueprint[] input = File.ReadAllLines(@"inputs\day19.txt")
            .Select(line => {
                var sublines = line.Split(':', '.');
                var bluePrintId = int.Parse(Regex.Match(sublines[0], @"\d+").Value);
                var oreRobotCost = int.Parse(Regex.Match(sublines[1], @"\d+").Value);
                var clayRobotCost = int.Parse(Regex.Match(sublines[2], @"\d+").Value);
                var obsidianRobotCost = Regex.Matches(sublines[3], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
                var geodeRobotCost = Regex.Matches(sublines[4], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
                return new Blueprint(bluePrintId, oreRobotCost, clayRobotCost, (obsidianRobotCost[0], obsidianRobotCost[1]), (geodeRobotCost[0], geodeRobotCost[1]));
            })
            .ToArray();

        public static int GetQualityLevelSum() {
            var result = 0;
            var initialState = new RobotFactoryState(1, 0, 0, 0, 0, 0, 0, 0);
            foreach (var blueprint in input) {
                var factory = new RobotFactory(blueprint);
                var geodes = factory.CrackGeodes(initialState, 24, true, true);
                result += geodes * blueprint.BlueprintId;
            }
            return result;
        }

        public static int GetMaxGeodesMultiplication() {
            var result = 1;
            var initialState = new RobotFactoryState(1, 0, 0, 0, 0, 0, 0, 0);
            foreach (var blueprint in input.Take(3)) {
                var factory = new RobotFactory(blueprint);
                result *= factory.CrackGeodes(initialState, 32, true, true);
            }
            return result;
        }

        private class RobotFactory {
            public RobotFactory(Blueprint blueprint) {
                Blueprint = blueprint;
            }
            private Blueprint Blueprint { get; }

            public int CrackGeodes(RobotFactoryState state, int minutes, bool allowCreatingClayRobots, bool allowCreatingOreRobots) {
                if (minutes == 1) {
                    return state.GeodeCount + state.GeodeRobotCount;
                }

                if (CanCreateGeodeRobot(state)) {
                    var newOreCount = state.OreCount + state.OreRobotCount - Blueprint.GeodeRobotCost.Ore;
                    var newObsidianCount = state.ObsidianCount + state.ObsidianRobotCount - Blueprint.GeodeRobotCost.Obsidian;
                    return CrackGeodes(new RobotFactoryState(
                        state.OreRobotCount,
                        state.ClayRobotCount,
                        state.ObsidianRobotCount,
                        state.GeodeRobotCount + 1,
                        newOreCount,
                        state.ClayCount + state.ClayRobotCount,
                        newObsidianCount,
                        state.GeodeCount + state.GeodeRobotCount),
                        minutes - 1,
                        true,
                        true);
                }

                if (CanCreateObsidianRobot(state)) {
                    var newOreCount = state.OreCount + state.OreRobotCount - Blueprint.ObsidianRobotCost.Ore;
                    var newClayCount = state.ClayCount + state.ClayRobotCount - Blueprint.ObsidianRobotCost.Clay;
                    return CrackGeodes(new RobotFactoryState(
                       state.OreRobotCount,
                       state.ClayRobotCount,
                       state.ObsidianRobotCount + 1,
                       state.GeodeRobotCount,
                       newOreCount,
                       newClayCount,
                       state.ObsidianCount + state.ObsidianRobotCount,
                       state.GeodeCount + state.GeodeRobotCount),
                       minutes - 1,
                       true,
                       true);
                }

                var ways = new List<int>();
                var canCreateClayRobot = CanCreateClayRobot(state);
                if (canCreateClayRobot && Blueprint.ObsidianRobotCost.Clay > state.ClayRobotCount && allowCreatingClayRobots) {
                    ways.Add(CrackGeodes(new RobotFactoryState(
                       state.OreRobotCount,
                       state.ClayRobotCount + 1,
                       state.ObsidianRobotCount,
                       state.GeodeRobotCount,
                       state.OreCount + state.OreRobotCount - Blueprint.ClayRobotCost,
                       state.ClayCount + state.ClayRobotCount,
                       state.ObsidianCount + state.ObsidianRobotCount,
                       state.GeodeCount + state.GeodeRobotCount),
                       minutes - 1,
                       true,
                       true));
                }
                var canCreateOreRobot = CanCreateOreRobot(state);
                if (canCreateOreRobot && GetMostExpensiveRobotByOre() > state.OreRobotCount && allowCreatingOreRobots) {
                    ways.Add(CrackGeodes(new RobotFactoryState(
                       state.OreRobotCount + 1,
                       state.ClayRobotCount,
                       state.ObsidianRobotCount,
                       state.GeodeRobotCount,
                       state.OreCount + state.OreRobotCount - Blueprint.OreRobotCost,
                       state.ClayCount + state.ClayRobotCount,
                       state.ObsidianCount + state.ObsidianRobotCount,
                       state.GeodeCount + state.GeodeRobotCount),
                       minutes - 1,
                       true,
                       true));
                }
                ways.Add(CrackGeodes(new RobotFactoryState(
                       state.OreRobotCount,
                       state.ClayRobotCount,
                       state.ObsidianRobotCount,
                       state.GeodeRobotCount,
                       state.OreCount + state.OreRobotCount,
                       state.ClayCount + state.ClayRobotCount,
                       state.ObsidianCount + state.ObsidianRobotCount,
                       state.GeodeCount + state.GeodeRobotCount),
                       minutes - 1,
                       !canCreateClayRobot,
                       !canCreateOreRobot));

                return ways.Max();
            }           

            private bool CanCreateOreRobot(RobotFactoryState state) {
                return Blueprint.OreRobotCost <= state.OreCount;
            }

            private bool CanCreateClayRobot(RobotFactoryState state) {
                return Blueprint.ClayRobotCost <= state.OreCount;
            }

            private bool CanCreateObsidianRobot(RobotFactoryState state) {
                return Blueprint.ObsidianRobotCost.Ore <= state.OreCount && Blueprint.ObsidianRobotCost.Clay <= state.ClayCount;
            }

            private bool CanCreateGeodeRobot(RobotFactoryState state) {
                return Blueprint.GeodeRobotCost.Ore <= state.OreCount && Blueprint.GeodeRobotCost.Obsidian <= state.ObsidianCount;
            }

            private int GetMostExpensiveRobotByOre() {
                return new[] { Blueprint.OreRobotCost, Blueprint.ClayRobotCost, Blueprint.ObsidianRobotCost.Ore, Blueprint.GeodeRobotCost.Ore }.Max();
            }
        }

        private class RobotFactoryState {
            public RobotFactoryState(int oreRobotCount, int clayRobotCount, int obsidianRobotCount, int geodeRobotCount, int oreCount, int clayCount, int obsidianCount, int geodeCount) {
                OreRobotCount = oreRobotCount;
                ClayRobotCount = clayRobotCount;
                ObsidianRobotCount = obsidianRobotCount;
                GeodeRobotCount = geodeRobotCount;
                OreCount = oreCount;
                ClayCount = clayCount;
                ObsidianCount = obsidianCount;
                GeodeCount = geodeCount;
            }

            public int OreRobotCount { get; set; }
            public int ClayRobotCount { get; set; }
            public int ObsidianRobotCount { get; set; }
            public int GeodeRobotCount { get; set; }

            public int OreCount { get; set; }
            public int ClayCount { get; set; }
            public int ObsidianCount { get; set; }
            public int GeodeCount { get; set; }
        }

        private class Blueprint {
            public Blueprint(int blueprintId, int oreRobotCost, int clayRobotCost, (int Ore, int Clay) obsidianRobotCost, (int Ore, int Obsidian) geodeRobotCost) {
                BlueprintId = blueprintId;
                OreRobotCost = oreRobotCost;
                ClayRobotCost = clayRobotCost;
                ObsidianRobotCost = obsidianRobotCost;
                GeodeRobotCost = geodeRobotCost;
            }

            public int BlueprintId { get; }
            public int OreRobotCost { get; }
            public int ClayRobotCost { get; }
            public (int Ore, int Clay) ObsidianRobotCost { get; }
            public (int Ore, int Obsidian) GeodeRobotCost { get; }
        }
    }
}
