namespace AdventOfCode2023 {
    public class PuzzleFactory {
        public IPuzzle? CreatePuzzle(int dayOfMonth) {
            string className = $"AdventOfCode2023.Days.Day{dayOfMonth}";
            Type? type = Type.GetType(className);

            if (type == null) {
                return null;
            }

            string[] lines = File.ReadAllLines($"Inputs\\day{dayOfMonth}.txt");
            return Activator.CreateInstance(type, [lines]) as IPuzzle;
        }
    }
}
