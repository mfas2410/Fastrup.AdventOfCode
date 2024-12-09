namespace AOC2020;

public class Day02
{
    private readonly ITestOutputHelper _output;

    public Day02(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day02.txt");

        // Act
        int numberOfValidPasswords = 0;
        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            int actualOccurrences = parts[2].Count(x => x == parts[1][0]);
            List<int> permittedOccurrences = parts[0].Split('-').Select(int.Parse).ToList();
            if (actualOccurrences >= permittedOccurrences[0] && actualOccurrences <= permittedOccurrences[1])
                numberOfValidPasswords++;
        }

        // Assert
        _ = numberOfValidPasswords.Should().Be(660);
        _output.WriteLine($"{numberOfValidPasswords}");
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day02.txt");

        // Act
        int numberOfValidPasswords = 0;
        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            List<int> positions = parts[0].Split('-').Select(int.Parse).ToList();
            if (positions.Select(x => parts[2][x - 1]).Count(x => x == parts[1][0]) == 1)
                numberOfValidPasswords++;
        }

        // Assert
        _ = numberOfValidPasswords.Should().Be(530);
        _output.WriteLine($"{numberOfValidPasswords}");
    }
}