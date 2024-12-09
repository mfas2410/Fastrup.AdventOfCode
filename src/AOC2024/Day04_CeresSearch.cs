namespace AOC2024;

public sealed class Day04_CeresSearch
{
    private const string FileName = "Data/Day04.txt";

    [Fact]
    public async Task Test1()
    {
        string[] words = ["XMAS", "SAMX"];
        string[] lines = await File.ReadAllLinesAsync(FileName);
        string[] lines90 = lines.Rotate90Degrees();
        int answer = lines.CountWords(words) +
                     lines.Rotate45Degrees().CountWords(words) +
                     lines90.CountWords(words) +
                     lines90.Rotate45Degrees().CountWords(words);
        answer.Should().Be(2718);
    }

    [Fact]
    public async Task Test2()
    {
        string[] lines = await File.ReadAllLinesAsync(FileName);
        int answer = 0;
        for (int i = 1; i < lines.Length - 1; i++)
        {
            ReadOnlySpan<char> previousLine = lines[i - 1].AsSpan();
            ReadOnlySpan<char> line = lines[i].AsSpan();
            ReadOnlySpan<char> nextLine = lines[i + 1].AsSpan();
            for (int j = 1; j < line.Length - 1; j++)
            {
                if (line[j] != 'A') continue;
                char upperLeft = previousLine[j - 1];
                char upperRight = previousLine[j + 1];
                char lowerLeft = nextLine[j - 1];
                char lowerRight = nextLine[j + 1];
                if ((upperLeft == 'M' && lowerRight == 'S' && lowerLeft == 'M' && upperRight == 'S') ||
                    (upperLeft == 'S' && lowerRight == 'M' && lowerLeft == 'M' && upperRight == 'S') ||
                    (upperLeft == 'S' && lowerRight == 'M' && lowerLeft == 'S' && upperRight == 'M') ||
                    (upperLeft == 'M' && lowerRight == 'S' && lowerLeft == 'S' && upperRight == 'M'))
                {
                    answer++;
                }
            }
        }
        answer.Should().Be(2046);
    }
}
