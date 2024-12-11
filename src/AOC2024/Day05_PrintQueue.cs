namespace AOC2024;

public sealed class Day05_PrintQueue
{
    private const string FileName = "Data/Day05.txt";

    [Fact]
    public async Task Test1()
    {
        (Dictionary<string, HashSet<string>> rules, List<string[]> updates) = await GetData();
        List<int> updateIndicesInRightOrder = [];
        for (int index = 0; index < updates.Count; index++)
        {
            string[] update = updates[index];
            if (IsInRightOrder(update, rules)) updateIndicesInRightOrder.Add(index);
        }
        int answer = 0;
        foreach (int index in updateIndicesInRightOrder)
        {
            string[] update = updates[index];
            answer += int.Parse(update[update.Length / 2]);
        }
        answer.Should().Be(5_588);
    }

    [Fact]
    public async Task Test2()
    {
        (Dictionary<string, HashSet<string>> rules, List<string[]> updates) = await GetData();
        List<int> updateIndicesInWrongOrder = [];
        for (int index = 0; index < updates.Count; index++)
        {
            string[] update = updates[index];
            if (!IsInRightOrder(update, rules)) updateIndicesInWrongOrder.Add(index);
        }
        int answer = 0;
        foreach (int index in updateIndicesInWrongOrder)
        {
            string[] update = updates[index];
            Array.Sort(update, (a, b) => rules.TryGetValue(b, out HashSet<string>? rule) && rule.Contains(a) ? -1 : 1);
            answer += int.Parse(update[update.Length / 2]);
        }
        answer.Should().Be(5_331);
    }

    private static async Task<(Dictionary<string, HashSet<string>> rules, List<string[]> updates)> GetData()
    {
        string[] lines = await File.ReadAllLinesAsync(FileName);
        Dictionary<string, HashSet<string>> rules = [];
        List<string[]> updates = [];
        foreach (string line in lines)
        {
            if (line.Length == 0) continue;
            if (line.Contains('|'))
            {
                string[] parts = line.Split('|');
                if (!rules.TryGetValue(parts[0], out _)) rules[parts[0]] = [];
                rules[parts[0]].Add(parts[1]);
            }
            else
            {
                updates.Add(line.Split(','));
            }
        }
        return (rules, updates);
    }

    private static bool IsInRightOrder(string[] update, Dictionary<string, HashSet<string>> rules)
    {
        for (int i = 0; i < update.Length; i++)
        {
            for (int j = i + 1; j < update.Length; j++)
            {
                if (rules.TryGetValue(update[j], out HashSet<string>? rule) && rule.Contains(update[i])) return false;
            }
        }
        return true;
    }
}
