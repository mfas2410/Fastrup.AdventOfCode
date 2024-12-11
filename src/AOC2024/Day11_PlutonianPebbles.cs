namespace AOC2024;

public sealed class Day11_PlutonianPebbles
{
    private const string FileName = "Data/Day11.txt";

    [Fact]
    public async Task Test1()
    {
        Dictionary<long, long> pebbles = await GetPebbles();
        for (int count = 0; count < 25; count++) pebbles = Blink(pebbles);
        pebbles.Values.Sum().Should().Be(218_079);
    }

    [Fact]
    public async Task Test2()
    {
        Dictionary<long, long> pebbles = await GetPebbles();
        for (int count = 0; count < 75; count++) pebbles = Blink(pebbles);
        pebbles.Values.Sum().Should().Be(259_755_538_429_618);
    }

    private static async Task<Dictionary<long, long>> GetPebbles() =>
        (await File.ReadAllTextAsync(FileName))
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .GroupBy(number => number)
        .ToDictionary(group => group.Key, group => (long)group.Count());

    private static Dictionary<long, long> Blink(Dictionary<long, long> pebbles)
    {
        Dictionary<long, long> result = [];
        foreach (KeyValuePair<long, long> kvp in pebbles)
        {
            long number = kvp.Key;
            long count = kvp.Value;
            if (number == 0)
            {
                AddOrUpdate(result, 1, count);
            }
            else
            {
                int length = (int)Math.Log10(number) + 1;
                if (length % 2 == 0)
                {
                    long divisor = (long)Math.Pow(10, length / 2D);
                    long newNumber = number % divisor;
                    number /= divisor;
                    AddOrUpdate(result, number, count);
                    AddOrUpdate(result, newNumber, count);
                }
                else
                {
                    number *= 2024;
                    AddOrUpdate(result, number, count);
                }
            }
        }
        return result;
    }

    private static void AddOrUpdate(Dictionary<long, long> dictionary, long key, long value)
    {
        if (!dictionary.TryAdd(key, value)) dictionary[key] += value;
    }
}
