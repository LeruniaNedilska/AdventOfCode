using AdventOfCode2022;

var day1Result = Day1.GetMaxCaloriesSums();
Console.WriteLine($"Puzzle 1: {day1Result.First()}");
Console.WriteLine($"Puzzle 2: {day1Result.Sum()}");

Console.WriteLine($"Puzzle 3: {Day2.GetGameScore(Day2.Puzzle1)}");
Console.WriteLine($"Puzzle 4: {Day2.GetGameScore(Day2.Puzzle2)}");

Console.WriteLine($"Puzzle 5: {Day3.GetMisplacedItemsSum()}");
Console.WriteLine($"Puzzle 6: {Day3.GetBadgeItemsSum()}");

Console.WriteLine($"Puzzle 7: {Day4.CountFullyOverlappingRanges()}");
Console.WriteLine($"Puzzle 8: {Day4.CountOverlappingRanges()}");

Console.WriteLine($"Puzzle 9: {Day5.GetTopCrates<CrateMover9000>()}");
Console.WriteLine($"Puzzle 10: {Day5.GetTopCrates<CrateMover9001>()}");

Console.WriteLine($"Puzzle 11: {Day6.GetFirstMarkerIndex(4)}");
Console.WriteLine($"Puzzle 12: {Day6.GetFirstMarkerIndex(14)}");

Console.WriteLine($"Puzzle 13: {Day7.GetDirectoriesSizeOver100000Sum()}");
Console.WriteLine($"Puzzle 14: {Day7.GetTheSmallestDirectoryToDelete()}");

Console.WriteLine($"Puzzle 15: {Day8.GetVisibleTreesCount()}");
Console.WriteLine($"Puzzle 16: {Day8.GetMaxTreeScenicScore()}");

Console.WriteLine($"Puzzle 17: {Day9.GetVisitedPositions(1)}");
Console.WriteLine($"Puzzle 18: {Day9.GetVisitedPositions(9)}");

Console.WriteLine($"Puzzle 19: {Day10.GetSignalStrengthSum()}");
Console.WriteLine("Puzzle 20:");
Day10.RenderImage();

Console.WriteLine($"Puzzle 21: {Day11.GetMonkeyBusinessWithoutWorries()}");
Console.WriteLine($"Puzzle 22: {Day11.GetMonkeyBusinessWithWorries()}");

Console.WriteLine($"Puzzle 23: {Day12.GetMinimalDistanceBetweenNodes('S', 'E')}");
Console.WriteLine($"Puzzle 24: {Day12.GetMinimalDistanceBetweenNodes('E', 'a', true)}");

Console.WriteLine($"Puzzle 25: {Day13.GetIndecesSum()}");
Console.WriteLine($"Puzzle 26: {Day13.GetDecoderKey()}");

Console.WriteLine($"Puzzle 27: {Day14.GetSandUnitCountBeforeAbyss()}");
Console.WriteLine($"Puzzle 28: {Day14.GetSandUnitCountBeforeBlocked()}");

Console.WriteLine($"Puzzle 29: {Day15.GetUnsuitablePositionsCount()}");
Console.WriteLine($"Puzzle 30: {Day15.GetTuningFrequency()}");

Console.WriteLine($"Puzzle 31: {Day16.GetMaxPressureRelease()}");
Console.WriteLine($"Puzzle 32: {Day16.GetMaxPressureReleaseWithElephant()}");

Console.WriteLine($"Puzzle 33: {Day17.GetTowerHeight(2022)}");
Console.WriteLine($"Puzzle 34: {Day17.GetTowerHeight(1000000000000)}");

Console.WriteLine($"Puzzle 35: {Day18.GetSurfaceArea()}");
Console.WriteLine($"Puzzle 36: {Day18.GetExteriorSurfaceArea()}");

Console.WriteLine($"Puzzle 37: {Day19.GetQualityLevelSum()}");
Console.WriteLine($"Puzzle 38: {Day19.GetMaxGeodesMultiplication()}");