namespace AOC2023;

public sealed class Day08_HauntedWasteland
{
    private const string FileName = "Data/Day08.txt";

    [Fact]
    public async Task Test1()
    {
        int answer = 0;

        Regex lettersRegex = new(@"(?<Combo>\w+)", RegexOptions.Compiled);
        string? instructions = null;
        Dictionary<string, Tuple<string, string>> map = [];
        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            MatchCollection matches = lettersRegex.Matches(line);
            if (matches.Count == 3)
            {
                map[matches[0].Groups["Combo"].Value] = Tuple.Create(matches[1].Groups["Combo"].Value, matches[2].Groups["Combo"].Value);
            }
            else
            {
                instructions ??= line;
            }
        }

        char nextInstruction = instructions![answer];
        string nextNode = "AAA";
        do
        {
            nextNode = nextInstruction == 'L' ? map[nextNode].Item1 : map[nextNode].Item2;
            nextInstruction = instructions[++answer % instructions.Length];
        } while (nextNode != "ZZZ");
        answer.Should().Be(13939);
    }

    [Fact]
    public async Task Test2()
    {
        //long answer = 0;

        //Regex lettersRegex = new(@"(?<Combo>\w+)", RegexOptions.Compiled);
        //string? instructions = null;
        //Dictionary<string, Tuple<string, string>> map = [];
        //await foreach (string line in File.ReadLinesAsync(FileName))
        //{
        //    MatchCollection matches = lettersRegex.Matches(line);
        //    if (matches.Count == 3)
        //    {
        //        map[matches[0].Groups["Combo"].Value] = Tuple.Create(matches[1].Groups["Combo"].Value, matches[2].Groups["Combo"].Value);
        //    }
        //    else
        //    {
        //        instructions ??= line;
        //    }
        //}

        //Dictionary<string, Tuple<int, int>> mapCycles = [];
        //int count = 0;
        //char nextInstruction = instructions![count];
        //string[] startNodes = map.Keys.Where(x => x.EndsWith('A')).ToArray();
        //foreach (string startNode in startNodes)
        //{
        //    HashSet<string> visitedNodes = [];
        //    string nextNode = startNode;
        //    while (visitedNodes.Add(nextNode))
        //    {
        //        nextNode = nextInstruction == 'L' ? map[nextNode].Item1 : map[nextNode].Item2;
        //        nextInstruction = instructions[++count % instructions.Length];
        //    }

        //    List<string> strings = visitedNodes.ToList();
        //    int index = strings.FindIndex(x => x.Equals(nextNode));
        //    mapCycles.Add(startNode, new Tuple<int, int>(index, strings.Count - index));
        //}

        //answer = mapCycles.Select(x => x.Value.Item1).Sum();
        //answer += LeastCommonMultiple.GetLeastCommonMultiple(mapCycles.Select(x => x.Value.Item2).ToArray());
        //answer.Should().BeGreaterThan(33865167436);
    }
}
