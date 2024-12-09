namespace AOC2020;

public class Day01
{
    private readonly ITestOutputHelper _output;

    public Day01(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        int first = 0;
        int second = 0;
        bool numbersFound = false;

        string[] lines = await File.ReadAllLinesAsync(@"Data\Day01.txt");
        List<int> expenses = lines.Select(int.Parse).ToList();

        // Act
        for (int i = 0; i < expenses.Count; i++)
        {
            first = expenses[i];
            for (int j = i + 1; j < expenses.Count; j++)
            {
                second = expenses[j];
                if (first + second == 2020)
                {
                    numbersFound = true;
                    break;
                }
            }

            if (numbersFound)
                break;
        }

        // Assert
        _ = numbersFound.Should().BeTrue();
        _ = (first + second).Should().Be(2020);
        _ = (first * second).Should().Be(898299);
        _output.WriteLine($"{first} * {second} = {first * second}");
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        int first = 0;
        int second = 0;
        int third = 0;
        bool numbersFound = false;

        string[] lines = await File.ReadAllLinesAsync(@"Data\Day01.txt");
        List<int> expenses = lines.Select(int.Parse).ToList();

        // Act
        for (int i = 0; i < expenses.Count; i++)
        {
            first = expenses[i];
            for (int j = i + 1; j < expenses.Count; j++)
            {
                second = expenses[j];
                for (int k = j + 1; k < expenses.Count; k++)
                {
                    third = expenses[k];
                    if (first + second + third == 2020)
                    {
                        numbersFound = true;
                        break;
                    }
                }

                if (numbersFound)
                    break;
            }

            if (numbersFound)
                break;
        }

        // Assert
        _ = numbersFound.Should().BeTrue();
        _ = (first + second + third).Should().Be(2020);
        _ = (first * second * third).Should().Be(143933922);
        _output.WriteLine($"{first} * {second} * {third} = {first * second * third}");
    }
}