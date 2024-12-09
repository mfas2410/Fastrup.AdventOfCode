namespace AOC2021;

public class Day14_Extended_Polymerization
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        (LinkedList<char> polymer, Dictionary<string, char> polymerizationRules) = await Initialize();

        // Act
        for (int i = 0; i < 10; i++)
        {
            BruteForce(polymerizationRules, polymer);
        }

        var groups = polymer.GroupBy(x => x).Select(x => new { Character = x.Key, Count = x.Count() }).OrderBy(x => x.Count).ToList();
        int result = groups.Last().Count - groups.First().Count;

        // Assert
        _ = result.Should().Be(2194);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        (LinkedList<char> polymer, Dictionary<string, char> polymerizationRules) = await Initialize();

        // Act
        Dictionary<string, long> pairCounts = new();
        Dictionary<char, long> elementCounts = new();

        for (int i = 1; i < polymer.Count; i++)
        {
            string pair = string.Empty + polymer.Skip(i - 1).Take(1).First() + polymer.Skip(i).Take(1).First();
            pairCounts[pair] = pairCounts.ContainsKey(pair) ? pairCounts[pair] + 1 : 1;
        }

        Dictionary<string, long> state = new(pairCounts);

        foreach (char elem in polymer)
        { // Initialise element counts
            elementCounts[elem] = elementCounts.ContainsKey(elem) ? elementCounts[elem] + 1 : 1;
        }

        for (int i = 0; i < 40; i++)
        { // Do polymerization
            state = FastStep(state, elementCounts, polymerizationRules);
        }

        long max = elementCounts.Values.Max();
        long min = elementCounts.Values.Min();
        long result = max - min;

        // Assert
        _ = result.Should().Be(2360298895777);
    }

    private async Task<(LinkedList<char>, Dictionary<string, char>)> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day14.txt");
        LinkedList<char> initialPolymer = new(lines[0].ToArray());
        Dictionary<string, char> polymerizationRules = new();
        for (int index = 2; index < lines.Length; index++)
        {
            string[] rule = lines[index].Split(" -> ");
            polymerizationRules.Add(rule[0], rule[1][0]);
        }

        return (initialPolymer, polymerizationRules);
    }

    private static void BruteForce(Dictionary<string, char> polymerizationRules, LinkedList<char> polymer)
    {
        int index = 0;
        LinkedListNode<char>? node = polymer.First;
        while (index < polymer.Count - 1)
        {
            string combination = string.Join(string.Empty, polymer.Skip(index).Take(2));
            char reaction = polymerizationRules[combination];
            _ = polymer.AddAfter(node!, reaction);
            node = node!.Next;
            node = node!.Next;
            index += 2;
        }
    }

    private static Dictionary<string, long> FastStep(Dictionary<string, long> currPairs, Dictionary<char, long> elemCounts, Dictionary<string, char> rules)
    {
        Dictionary<string, long> result = new();
        foreach (KeyValuePair<string, long> kvp in currPairs)
        {
            // Update element counts, every pair will produce one of the new element.
            char newElem = rules[kvp.Key];
            elemCounts[newElem] = elemCounts.ContainsKey(newElem) ?
                elemCounts[newElem] + kvp.Value :
                kvp.Value;

            // Create the two pairs and put them in the resulting dictionary with the new values
            string pair1 = string.Empty + kvp.Key[0] + rules[kvp.Key];
            string pair2 = string.Empty + rules[kvp.Key] + kvp.Key[1];
            result[pair1] = result.ContainsKey(pair1) ? result[pair1] + kvp.Value : kvp.Value;
            result[pair2] = result.ContainsKey(pair2) ? result[pair2] + kvp.Value : kvp.Value;
        }

        return result;
    }
}
