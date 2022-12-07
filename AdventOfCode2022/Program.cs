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