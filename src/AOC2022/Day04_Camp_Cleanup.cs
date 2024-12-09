namespace AOC2022;

public class Day04_Camp_Cleanup
{
    [Fact]
    public async Task InHowManyAssignmentPairsDoesOneRangeFullyContainTheOther()
    {
        // Arrange
        IReadOnlyList<IReadOnlyList<IReadOnlyList<int>>> input = await Initialize();

        // Act
        int actual = 0;
        foreach (IReadOnlyList<IReadOnlyList<int>> line in input)
        {
            if (!line[0].Except(line[1]).Any() || !line[1].Except(line[0]).Any())
            {
                actual++;
            }
        }

        // Assert
        _ = actual.Should().Be(550);
    }

    [Fact]
    public async Task InHowManyAssignmentPairsDoesOneRangeOverlapTheOther()
    {
        // Arrange
        IReadOnlyList<IReadOnlyList<IReadOnlyList<int>>> input = await Initialize();

        // Act
        int actual = 0;
        foreach (IReadOnlyList<IReadOnlyList<int>> line in input)
        {
            if (line[0].Intersect(line[1]).Any())
            {
                actual++;
            }
        }

        // Assert
        _ = actual.Should().Be(931);
    }

    private static async Task<IReadOnlyList<IReadOnlyList<IReadOnlyList<int>>>> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day04.txt");
        List<List<List<int>>> result = new();
        foreach (string line in lines)
        {
            List<List<int>> lineResult = new();
            result.Add(lineResult);
            foreach (string sectionInterval in line.Split(','))
            {
                string[] sectionsString = sectionInterval.Split('-');
                int firstSection = int.Parse(sectionsString[0]);
                int secondSection = int.Parse(sectionsString[1]);
                lineResult.Add(Enumerable.Range(firstSection, secondSection - firstSection + 1).ToList());
            }
        }

        return result;
    }
}
