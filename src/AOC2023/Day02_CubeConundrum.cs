namespace AOC2023;

public sealed class Day02_CubeConundrum
{
    private const string FileName = "Data/Day02.txt";
    private readonly Regex _cubeCombinationsRegex = new("(?<Number>\\d+) (?<Color>[a-z]+)", RegexOptions.Compiled);

    [Fact]
    public async Task Test1()
    {
        int answer = 0;

        Regex gameIdRegex = new("(?<GameId>\\d+):", RegexOptions.Compiled);
        FrozenDictionary<string, int> maxCubesAllowed = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } }.ToFrozenDictionary();

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (_cubeCombinationsRegex
                .Matches(line)
                .GroupBy(x => x.Groups["Color"].Value, x => int.Parse(x.Groups["Number"].Value))
                .ToDictionary(x => x.Key, x => x.Max())
                .All(x => maxCubesAllowed[x.Key] >= x.Value)
               )
            {
                answer += int.Parse(gameIdRegex.Match(line).Groups["GameId"].Value);
            }
        }

        answer.Should().Be(2600);
    }

    [Fact]
    public async Task Test2()
    {
        int answer = 0;

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            answer += _cubeCombinationsRegex
                .Matches(line)
                .GroupBy(x => x.Groups["Color"].Value, x => int.Parse(x.Groups["Number"].Value))
                .Select(x => x.Max())
                .Aggregate((x, y) => x * y);
        }

        answer.Should().Be(86036);
    }
}
