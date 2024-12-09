namespace AOC2023;

public sealed class Day01_Trebuchet
{
    private const string FileName = "Data/Day01.txt";
    private readonly FrozenDictionary<string, int> _digits = new Dictionary<string, int> { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } }.ToFrozenDictionary();
    private readonly Regex _digitsRegex = new("([1-9])", RegexOptions.Compiled);
    private readonly Regex _digitsSpelledRegex = new("(?=(one|two|three|four|five|six|seven|eight|nine|[1-9]))", RegexOptions.Compiled);

    [Fact]
    public async Task Test1()
    {
        int answer = 0;

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            answer += GetNumber(_digitsRegex.Matches(line));
        }

        answer.Should().Be(54634);
    }

    [Fact]
    public async Task Test2()
    {
        int answer = 0;

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            answer += GetNumber(_digitsSpelledRegex.Matches(line));
        }

        answer.Should().Be(53855);
    }

    private int GetNumber(MatchCollection matchCollection) =>
        (GetDigit(matchCollection.First().Groups.Values.Last().Value) * 10) +
        GetDigit(matchCollection.Last().Groups.Values.Last().Value);

    private int GetDigit(string input) =>
        input.Length == 1 ? int.Parse(input) : _digits[input];
}
