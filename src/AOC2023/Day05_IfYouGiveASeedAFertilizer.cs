namespace AOC2023;

public sealed class Day05_IfYouGiveASeedAFertilizer
{
    private const string FileName = "Data/Day05.txt";
    private static readonly Regex NumbersRegex = new(@"(?<Number>\d+)", RegexOptions.Compiled);

    [Fact]
    public async Task Test1()
    {
        long answer = 0;

        int mapIndex = -1;
        List<List<Range>> maps = [];
        ImmutableArray<long> seeds = [];

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (string.IsNullOrEmpty(line)) continue;

            if (line.EndsWith(" map:"))
            {
                mapIndex++;
                continue;
            }

            MatchCollection matches = NumbersRegex.Matches(line);
            if (line.Contains(':'))
            {
                seeds = matches.Select(x => long.Parse(x.Groups["Number"].Value)).ToImmutableArray();
            }
            else
            {
                if (mapIndex == maps.Count) maps.Add([]);
                maps[mapIndex].AddRange(matches.Select(x => long.Parse(x.Groups["Number"].Value)).Chunk(3).Select(range => new Range(range[1], range[1] + range[2] - 1, range[0] - range[1])));
            }
        }

        answer = seeds.Select(seed => maps.Aggregate(seed, (currentSeed, map) => currentSeed + (map.FirstOrDefault(mapping => mapping.Contains(currentSeed))?.Adjustment ?? 0))).Min();
        answer.Should().Be(177942185);
    }

    [Fact]
    public async Task Test2()
    {
        long answer = 0;

        int mapIndex = -1;
        List<List<Range>> maps = [];
        List<Range> seeds = [];

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (string.IsNullOrEmpty(line)) continue;

            if (line.EndsWith(" map:"))
            {
                mapIndex++;
                continue;
            }

            MatchCollection matches = NumbersRegex.Matches(line);
            if (line.Contains(':'))
            {
                seeds = matches.Select(x => long.Parse(x.Groups["Number"].Value)).Chunk(2).Select(range => new Range(range[0], range[0] + range[1] - 1)).ToList();
            }
            else
            {
                if (mapIndex == maps.Count) maps.Add([]);
                maps[mapIndex].AddRange(matches.Select(x => long.Parse(x.Groups["Number"].Value)).Chunk(3).Select(range => new Range(range[1], range[1] + range[2] - 1, range[0] - range[1])));
            }
        }

        maps.ForEach(x => x.Sort());

        // Solution not by me (Range split algorithm)
        foreach (List<Range> map in maps)
        {
            List<Range> newSeeds = [];
            foreach (Range seed in seeds)
            {
                Range copy = seed with { };
                foreach (Range mapping in map)
                {
                    if (copy.From < mapping.From)
                    {
                        newSeeds.Add(new Range(copy.From, Math.Min(copy.To, mapping.From - 1)));
                        copy = copy with { From = mapping.From };
                        if (copy.From > copy.To) break;
                    }

                    if (copy.From <= mapping.To)
                    {
                        newSeeds.Add(new Range(copy.From + mapping.Adjustment, Math.Min(copy.To, mapping.To) + mapping.Adjustment));
                        copy = copy with { From = mapping.To + 1 };
                        if (copy.From > copy.To) break;
                    }
                }

                if (copy.From <= copy.To) newSeeds.Add(seed);
            }

            seeds = newSeeds;
        }

        answer = seeds.Min(seed => seed.From);
        answer.Should().Be(69841803);
    }

    private sealed record Range(long From, long To, long Adjustment = 0) : IComparable
    {
        public int CompareTo(object? obj)
        {
            if (obj is not Range other) return 1;
            return From < other.From ? -1 : From > other.From ? 1 : 0;
        }

        public bool Contains(long value) =>
            value >= From && value <= To;
    }
}
