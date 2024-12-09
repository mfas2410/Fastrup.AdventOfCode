namespace AOC2023;

public sealed class Day04_Scratchcards
{
    private const string FileName = "Data/Day04.txt";
    private static readonly Regex NumbersRegex = new(@"(?<Number>\d+)", RegexOptions.Compiled);

    [Fact]
    public async Task Test1()
    {
        int answer = 0;

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            ImmutableArray<string> numbers = line.Split(':')[1].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToImmutableArray();
            FrozenDictionary<int, int> winningNumbers = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToFrozenDictionary(x => x);
            ImmutableArray<int> myNumbers = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToImmutableArray();
            int matches = myNumbers.Count(winningNumbers.ContainsKey);
            answer += matches == 0 ? matches : (int)Math.Pow(2, matches - 1);
        }

        answer.Should().Be(32001);
    }

    [Fact]
    public async Task Test2()
    {
        int answer = 0;

        Dictionary<int, int> scratchcards = [];

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            ImmutableArray<string> cardAndNumbers = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToImmutableArray();
            int currentScratchcard = int.Parse(NumbersRegex.Match(cardAndNumbers[0]).Groups["Number"].Value);
            AddOrUpdate(scratchcards, currentScratchcard, 1);

            ImmutableArray<string> numbers = cardAndNumbers[1].Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToImmutableArray();
            FrozenDictionary<int, int> winningNumbers = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToFrozenDictionary(x => x);
            ImmutableArray<int> myNumbers = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToImmutableArray();

            foreach (int scratchCard in Enumerable.Range(currentScratchcard + 1, myNumbers.Count(winningNumbers.ContainsKey)))
            {
                AddOrUpdate(scratchcards, scratchCard, scratchcards[currentScratchcard]);
            }
        }

        answer = scratchcards.Values.Sum();
        answer.Should().Be(5037841);
    }

    private static void AddOrUpdate(Dictionary<int, int> scratchcards, int scratchcard, int increaseBy)
    {
        scratchcards.TryGetValue(scratchcard, out int numberOfScratchcards);
        scratchcards[scratchcard] = numberOfScratchcards + increaseBy;
    }
}
