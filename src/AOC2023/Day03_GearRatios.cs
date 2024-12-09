namespace AOC2023;

public sealed class Day03_GearRatios
{
    private const string FileName = "Data/Day03.txt";
    private static readonly Regex GearsRegex = new(@"(?<Symbol>\*)", RegexOptions.Compiled);
    private static readonly Regex NumbersRegex = new(@"(?<Number>\d+)", RegexOptions.Compiled);
    private readonly Regex _symbolsRegex = new(@"(?<Symbol>(@|#|\$|%|&|/|=|\+|\*|-))", RegexOptions.Compiled);

    [Fact]
    public async Task Test1()
    {
        int answer = 0;

        List<FrozenSet<int>> symbolIndices = new(3);
        string numberLine = string.Empty;
        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (symbolIndices.Count > 2) symbolIndices.RemoveAt(0);
            symbolIndices.Add(_symbolsRegex.Matches(line).Select(x => x.Index).ToFrozenSet());
            answer += GetLineTotal(numberLine, symbolIndices.SelectMany(x => x.Items).ToFrozenSet());
            numberLine = line;
        }

        symbolIndices.RemoveAt(0);
        answer += GetLineTotal(numberLine, symbolIndices.SelectMany(x => x.Items).ToFrozenSet());

        answer.Should().Be(553825);
    }

    [Fact]
    public async Task Test2()
    {
        int answer = 0;

        List<MatchCollection> numberMatches = new(3);
        string gearLine = string.Empty;
        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (numberMatches.Count > 2) numberMatches.RemoveAt(0);
            numberMatches.Add(NumbersRegex.Matches(line));
            answer += GetGearTotal(gearLine, numberMatches.SelectMany(x => x.ToList()).ToList());
            gearLine = line;
        }

        numberMatches.RemoveAt(0);
        answer += GetGearTotal(gearLine, numberMatches.SelectMany(x => x.ToList()).ToList());

        answer.Should().Be(93994191);
    }

    private static int GetLineTotal(string line, IEnumerable<int> symbolIndices) =>
        NumbersRegex.Matches(line)
            .Where(match => HasSymbol(match.Index, match.Length, symbolIndices))
            .Sum(match => int.Parse(match.Groups["Number"].Value));

    private static bool HasSymbol(int numberIndex, int numberLength, IEnumerable<int> symbolIndices) =>
        symbolIndices.Any(index => index >= numberIndex - 1 && index <= numberIndex + numberLength);

    private static int GetGearTotal(string line, IReadOnlyCollection<Match> numberMatches)
    {
        int result = 0;
        foreach (Match gearMatch in GearsRegex.Matches(line))
        {
            ImmutableArray<int> numbers = numberMatches
                .Where(match => gearMatch.Index >= match.Index - 1 && gearMatch.Index <= match.Index + match.Length)
                .Select(x => int.Parse(x.Value))
                .ToImmutableArray();
            if (numbers.Length == 2) result += numbers.Aggregate((x, y) => x * y);
        }

        return result;
    }
}
