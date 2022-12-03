using AdventOfCode2022;

var day1Result = Day1.GetMaxCaloriesSums();
Console.WriteLine($"Puzzle 1: {day1Result.First()}");
Console.WriteLine($"Puzzle 2: {day1Result.Sum()}");

Console.WriteLine($"Puzzle 3: {Day2.GetGameScore(Day2.Puzzle1)}");
Console.WriteLine($"Puzzle 4: {Day2.GetGameScore(Day2.Puzzle2)}");

Console.WriteLine($"Puzzle 5: {Day3.GetMisplacedItemsSum()}");
Console.WriteLine($"Puzzle 6: {Day3.GetBadgeItemsSum()}");
