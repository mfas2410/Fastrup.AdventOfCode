namespace AOC2021;

public class Day03_Binary_Diagnostic
{
    private readonly ITestOutputHelper _output;
    private readonly IReadOnlyList<string> _data;

    public Day03_Binary_Diagnostic(ITestOutputHelper output)
    {
        _output = output;
        _data = File.ReadAllLines(@"Data\Day03.txt");
    }

    [Fact]
    public void Task1_WhatIsThePowerConsumptionOfTheSubmarine()
    {
        // Arrange
        ICollection<int> indices = Enumerable.Range(0, _data.Count).ToList();
        char[] gammaChars = new char[_data[0].Length];
        char[] epsilonChars = new char[_data[0].Length];

        for (int index = 0; index < _data[0].Length; index++)
        {
            char chr = HasMost(index, _data, indices);
            gammaChars[index] = chr == '0' ? '0' : '1';
            epsilonChars[index] = chr == '0' ? '1' : '0';
        }

        // Act
        int gammaRate = ToInt32(gammaChars);
        int epsilonRate = ToInt32(epsilonChars);

        // Assert
        _ = gammaRate.Should().Be(3022);
        _ = epsilonRate.Should().Be(1073);

        int result = gammaRate * epsilonRate;
        _output.WriteLine($"Gamma {gammaRate} * Epsilon {epsilonRate} = {result}");
        _ = result.Should().Be(3242606);
    }

    [Fact]
    public void Task2_WhatIsTheLifeSupportRatingOfTheSubmarine()
    {
        // Arrange
        List<int> oxygenGeneratorIndices = Enumerable.Range(0, _data.Count()).ToList();
        List<int> co2ScrubberIndices = Enumerable.Range(0, _data.Count()).ToList();

        for (int position = 0; position < _data[0].Length; position++)
        {
            if (oxygenGeneratorIndices.Count > 1)
            {
                char chr = HasMost(position, _data, oxygenGeneratorIndices);
                oxygenGeneratorIndices = oxygenGeneratorIndices.Where(index => _data[index][position] == chr).ToList();
            }

            if (co2ScrubberIndices.Count > 1)
            {
                char chr = HasMost(position, _data, co2ScrubberIndices);
                co2ScrubberIndices = co2ScrubberIndices.Where(index => _data[index][position] != chr).ToList();
            }
        }

        // Act
        int oxygenGeneratorRating = ToInt32(_data[oxygenGeneratorIndices[0]].ToArray());
        int co2ScrubberRating = ToInt32(_data[co2ScrubberIndices[0]].ToArray());

        // Assert
        _ = oxygenGeneratorRating.Should().Be(3005);
        _ = co2ScrubberRating.Should().Be(1616);

        int result = oxygenGeneratorRating * co2ScrubberRating;
        _output.WriteLine($"Oxygen {oxygenGeneratorRating} * CO2 {co2ScrubberRating} = {result}");
        _ = result.Should().Be(4856080);
    }

    private static char HasMost(int position, IReadOnlyList<string> data, IEnumerable<int> indices)
    {
        int zeroes = 0;
        int ones = 0;
        foreach (int index in indices)
        {
            string line = data[index];
            if (line[position] == '0')
            {
                zeroes++;
            }
            else
            {
                ones++;
            }
        }

        return zeroes > ones ? '0' : '1';
    }

    private static int ToInt32(char[] chars)
    {
        int result = 0;
        for (int index = 0; index < chars.Length; index++)
        {
            result = (result << 1) + (chars[index] == '0' ? 0 : 1);
        }

        return result;
    }
}