namespace AOC2021;

public class Day12_Passage_Pathing
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        IReadOnlyList<Cave> caves = await Initialize();
        List<List<Cave>> paths = new();

        // Act
        FindPaths(caves.First(x => x.Name == "start"), new List<Cave>(), paths);

        // Assert
        _ = paths.Should().HaveCount(4749);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        IEnumerable<Cave> caves = await Initialize();
        List<List<Cave>> paths = new();

        // Act
        FindPaths2(caves.First(x => x.Name == "start"), new List<Cave>(), paths);

        // Assert
        _ = paths.Should().HaveCount(123054);
    }

    private static async Task<IReadOnlyList<Cave>> Initialize()
    {
        HashSet<Cave> caves = new();
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day12.txt");
        foreach (string line in lines)
        {
            string[] names = line.Split('-');
            Cave? cave1 = caves.FirstOrDefault(c => c.Name == names[0]) ?? new Cave(names[0]);
            _ = caves.Add(cave1);
            Cave? cave2 = caves.FirstOrDefault(c => c.Name == names[1]) ?? new Cave(names[1]);
            _ = caves.Add(cave2);
            cave1.AddExit(cave2);
            cave2.AddExit(cave1);
        }

        return caves.ToList();
    }

    private static void FindPaths(Cave cave, List<Cave> path, List<List<Cave>> paths)
    {
        if (cave.Name == "end")
        {
            path.Add(cave);
            paths.Add(path);
            return;
        }

        if (cave.Size == CaveSize.Small && path.Contains(cave)) return;

        path.Add(cave);
        foreach (Cave exit in cave.Exits)
        {
            FindPaths(exit, path.ToList(), paths);
        }
    }

    private static void FindPaths2(Cave cave, List<Cave> path, List<List<Cave>> paths)
    {
        if (path.Count > 0 && cave.Name == "start") return;

        if (cave.Name == "end")
        {
            path.Add(cave);
            paths.Add(path);
            return;
        }

        if (cave.Size == CaveSize.Small && path.Contains(cave) && path.Any(x => x.Size == CaveSize.Small && path.Count(y => y.Name == x.Name) > 1)) return;

        path.Add(cave);
        foreach (Cave exit in cave.Exits)
        {
            FindPaths2(exit, path.ToList(), paths);
        }
    }

    private enum CaveSize
    {
        Small,
        Large
    }

    private sealed class Cave : IEquatable<Cave>
    {
        private readonly HashSet<Cave> _exits = new();

        public Cave(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public CaveSize Size => Name.ToUpper() == Name ? CaveSize.Large : CaveSize.Small;
        public IEnumerable<Cave> Exits => _exits;

        public void AddExit(Cave cave)
        {
            _ = _exits.Add(cave);
        }

        public bool Equals(Cave? other)
        {
            return ReferenceEquals(this, other) || Name.Equals(other?.Name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Cave);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override string ToString()
        {
            return $"{Name} ({Size}) {Exits.Count()} exits";
        }
    }
}