namespace AOC2024;

public sealed class Day02_RedNosedReports
{
    private const string FileName = "Data/Day02.txt";

    [Fact]
    public async Task Test1() =>
        (await File.ReadAllLinesAsync(FileName))
        .Select(line => line.Split(' ').Select(int.Parse).ToArray())
        .Count(report => IsSafe(report))
        .Should().Be(379);

    [Fact]
    public async Task Test2() =>
        (await File.ReadAllLinesAsync(FileName))
        .Select(line => line.Split(' ').Select(int.Parse).ToArray())
        .Count(report => IsSafe(report) || report.Where((_, index) => IsSafe(report, index)).Any())
        .Should().Be(430);

    private static bool IsSafe(int[] report, int skipIndex = -1)
    {
        if (report.Length < 2) return true;
        bool? isAscendingOrder = null;
        for (int index = 1; index < report.Length; index++)
        {
            if (index == skipIndex) continue;
            int previous = index - 1 - (skipIndex == index - 1 ? 1 : 0);
            if (previous < 0) continue;
            int difference = report[index] - report[previous];
            isAscendingOrder ??= difference > 0;
            if (Math.Abs(difference) is < 1 or > 3 || (isAscendingOrder == true && difference < 0) || (isAscendingOrder == false && difference > 0)) return false;
        }
        return true;
    }
}
