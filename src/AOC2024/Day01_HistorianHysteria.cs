namespace AOC2024;

public sealed class Day01_HistorianHysteria
{
    private const string FileName = "Data/Day01.txt";

    [Fact]
    public async Task Test1()
    {
        (int[] left, int[] right) = await GetData();
        Array.Sort(left);
        Array.Sort(right);
        int answer = 0;
        for (int i = 0; i < left.Length; i++) answer += Math.Abs(left[i] - right[i]);
        answer.Should().Be(2904518);
    }

    [Fact]
    public async Task Test2()
    {
        (int[] left, int[] right) = await GetData();
        FrozenDictionary<int, int> rightCounts = right.GroupBy(x => x).ToFrozenDictionary(x => x.Key, x => x.Count());
        int answer = 0;
        foreach (IGrouping<int, int> group in left.GroupBy(x => x))
        {
            if (rightCounts.TryGetValue(group.Key, out int count)) answer += group.Count() * group.Key * count;
        }
        answer.Should().Be(18650129);
    }

    private static async ValueTask<(int[] Left, int[] Right)> GetData()
    {
        int[] left = new int[1000];
        int[] right = new int [1000];
        int index = 0;
        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int separatorIndex = span.IndexOf("   ");
            left[index] = int.Parse(span[..separatorIndex]);
            right[index++] = int.Parse(span[(separatorIndex + 3)..]);
        }
        return (left, right);
    }
}
