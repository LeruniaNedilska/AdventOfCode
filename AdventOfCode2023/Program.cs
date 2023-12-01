using AdventOfCode2023;

int day = DateTime.Today.Day;
if (args.Length > 0 && int.TryParse(args[0], out day)) {
    if (day < 0 || day > 25) {
        Console.WriteLine("Unknown day");
        return;
    }
}

var puzzleFactory = new PuzzleFactory();
var puzzle = puzzleFactory.CreatePuzzle(day);
if (puzzle == null) {
    Console.WriteLine("Unknown day");
    return;
}

Console.WriteLine(puzzle.SolveFirst());
Console.WriteLine(puzzle.SolveSecond());