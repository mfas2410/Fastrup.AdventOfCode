namespace AOC2023;

public sealed class Day06_WaitForIt
{
    private const string FileName = "Data/Day06.txt";

    [Fact]
    public async Task Test1()
    {
        long answer = 1;

        Regex numbersRegex = new(@"(?<Number>\d+)", RegexOptions.Compiled);
        string[] lines = await File.ReadAllLinesAsync(FileName);
        ImmutableArray<int> times = numbersRegex.Matches(lines[0]).Select(x => int.Parse(x.Groups["Number"].Value)).ToImmutableArray();
        ImmutableArray<int> distances = numbersRegex.Matches(lines[1]).Select(x => int.Parse(x.Groups["Number"].Value)).ToImmutableArray();

        for (int index = 0; index < times.Length; index++)
        {
            int time = times[index];
            long minTime = GetMinTimeFrom(time, distances[index]);
            answer *= 1 + time - (2 * minTime);
        }

        answer.Should().Be(500346);
    }

    [Fact]
    public async Task Test2()
    {
        long answer = 0;

        Regex numbersRegex = new(@"(?<Number>[\d\s]+)", RegexOptions.Compiled);
        string[] lines = await File.ReadAllLinesAsync(FileName);
        long time = long.Parse(string.Join(string.Empty, numbersRegex.Match(lines[0]).Groups["Number"].Value).Replace(" ", string.Empty));
        long distance = long.Parse(string.Join(string.Empty, numbersRegex.Match(lines[1]).Groups["Number"].Value).Replace(" ", string.Empty));

        long minTime = GetMinTimeFrom(time, distance);
        answer = 1 + time - (2 * minTime);
        answer.Should().Be(42515755);
    }

    private static long GetMinTimeFrom(long time, long distance)
    {
        long minTime = 0;
        do
        {
            minTime++;
        } while ((time - minTime) * minTime < distance);

        return minTime;
    }
}
