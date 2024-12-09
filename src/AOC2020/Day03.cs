namespace AOC2020;

public class Day03
{
    private readonly ITestOutputHelper _output;

    public Day03(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        string[] data = await File.ReadAllLinesAsync(@"Data\Day03.txt");

        // Act
        long numberOfTrees = CountTrees(3, 1, data);

        // Assert
        _output.WriteLine($"{numberOfTrees}");
        _ = numberOfTrees.Should().Be(207);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        string[] data = await File.ReadAllLinesAsync(@"Data\Day03.txt");
        IEnumerable<(int, int)> paths = new List<(int, int)> { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };

        // Act
        IEnumerable<long> treesPerPath = paths.Select(x => CountTrees(x.Item1, x.Item2, data));
        long result = treesPerPath.Aggregate((x, y) => x * y);

        // Assert
        _output.WriteLine($"{string.Join(" * ", treesPerPath)} = {result}");
        _ = result.Should().Be(2655892800);
    }

    private static long CountTrees(int right, int down, string[] lines)
    {
        long numberOfTrees = 0;
        int index = right;
        for (int i = down; i < lines.Length; i += down)
        {
            string line = lines[i];
            index = index >= line.Length ? index % line.Length : index;
            if (line[index] == '#')
                numberOfTrees++;
            index += right;
        }

        return numberOfTrees;
    }
}