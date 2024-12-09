namespace AOC2021;

public class Day08_Seven_Segment_Search
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        IEnumerable<(string[], string[] Output)> data = await Initialize();

        // Act
        int result = data.SelectMany(x => x.Output).Count(x => x.Length is 2 or 3 or 4 or 7);

        // Assert
        _ = result.Should().Be(352);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        IEnumerable<(string[], string[])> data = await Initialize();
        int result = 0;

        // Act
        foreach ((string[] digits, string[] output) in data)
        {
            Dictionary<int, char[]> deciferedDigits = new()
            {
                [1] = digits.Single(x => x.Length == 2).ToArray(),
                [4] = digits.Single(x => x.Length == 4).ToArray(),
                [7] = digits.Single(x => x.Length == 3).ToArray(),
                [8] = digits.Single(x => x.Length == 7).ToArray()
            };
            deciferedDigits[3] = digits.Single(x => x.Length == 5 && x.Except(deciferedDigits[7]).Count() == 2).ToArray();
            deciferedDigits[2] = digits.Single(x => x.Length == 5 && x.Except(deciferedDigits[3]).Except(deciferedDigits[4]).Count() == 1).ToArray();
            deciferedDigits[5] = digits.Single(x => x.Length == 5 && x.Except(deciferedDigits[2]).Count() == 2).ToArray();
            deciferedDigits[9] = digits.Single(x => x.Length == 6 && x.Except(deciferedDigits[4]).Except(deciferedDigits[7]).Count() == 1).ToArray();
            deciferedDigits[6] = digits.Single(x => x.Length == 6 && new HashSet<char>(x).SetEquals(deciferedDigits[2].Except(deciferedDigits[3]).Concat(deciferedDigits[5]))).ToArray();
            deciferedDigits[0] = digits.Single(x => !deciferedDigits.Values.Select(y => new HashSet<char>(y)).Any(z => z.SetEquals(x.ToArray()))).ToArray();
            result += output.Select((x, index) => deciferedDigits.Single(y => y.Value.Length == x.Length && new HashSet<char>(y.Value).SetEquals(x.ToArray())).Key * (int)Math.Pow(10, 3 - index)).Sum();
        }

        // Assert
        _ = result.Should().Be(936117);
    }

    private async Task<IEnumerable<(string[], string[])>> Initialize()
    {
        List<(string[], string[])> result = new();
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day08.txt");
        foreach (string line in lines)
        {
            string[] input = line.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] digits = input[..^4];
            string[] output = input[^4..];
            result.Add((digits, output));
        }

        return result;
    }
}