namespace AdventOfCode2022 {

    internal class Day7 {
        private static readonly string[] input = File.ReadAllLines(@"inputs\day7.txt");

        public static int GetDirectoriesSizeOver100000Sum() => GetDirectorySizes()
            .Where(x => x <= 100000)
            .Sum();

        public static int GetTheSmallestDirectoryToDelete() {
            var sizes = GetDirectorySizes();
            var neededSpace = 30000000 - 70000000 + sizes.Max(x => x);
            return sizes.Where(x => x >= neededSpace).OrderBy(x => x).First();
        }

        private static IEnumerable<int> GetDirectorySizes() {
            var root = new DirectoryDescriptor("/");
            var currentDirectory = root;
            var directories = new List<DirectoryDescriptor>();
            directories.Add(root);

            foreach (var line in input.Skip(1)) {
                if (line.StartsWith("$ ls")) {
                    continue;
                }

                if (char.IsDigit(line[0])) {
                    currentDirectory.Size += int.Parse(line.Split(' ')[0]);
                    continue;
                }

                if (line.StartsWith("dir")) {
                    var dirData = line.Split(' ');
                    var dir = new DirectoryDescriptor(dirData[1], currentDirectory);
                    currentDirectory.Directories.Add(dir);
                    directories.Add(dir);
                    continue;
                }

                if (line.EndsWith("..")) {
                    currentDirectory = currentDirectory.Parent;
                    continue;
                }

                if (line.StartsWith("$ cd")) {
                    var dirName = line.Substring(5);
                    var dir = currentDirectory.Directories.Single(x => x.Name == dirName);
                    currentDirectory = dir;
                    continue;
                }
            }

            foreach (var dir in directories) {
                yield return GetDirectorySize(dir);
            }
        }

        private static int GetDirectorySize(DirectoryDescriptor dir) {
            int size = dir.Size;

            foreach (var d in dir.Directories) {
                size += GetDirectorySize(d);
            }

            return size;
        }

        internal class DirectoryDescriptor {
            public DirectoryDescriptor(string name, DirectoryDescriptor? parent = null) {
                Name = name;
                Parent = parent;
                Directories = new List<DirectoryDescriptor>();
            }
            public string Name { get; set; }
            public DirectoryDescriptor? Parent { get; }
            public List<DirectoryDescriptor> Directories { get; }
            public int Size { get; set; }
        }
    }
}
