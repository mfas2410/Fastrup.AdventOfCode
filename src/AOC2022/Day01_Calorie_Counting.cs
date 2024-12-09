namespace AOC2022;

public class Day01_Calorie_Counting
{
    [Fact]
    public async Task FindTheElfCarryingTheMostCalories()
    {
        // Arrange
        List<int> caloriesPerElf = await Initialize();

        // Act
        int actual = caloriesPerElf.Max();

        // Assert
        _ = actual.Should().Be(69883);
    }

    [Fact]
    public async Task FindTheTopThreeElvesCarryingTheMostCalories()
    {
        // Arrange
        List<int> caloriesPerElf = await Initialize();

        // Act
        int actual = caloriesPerElf.OrderByDescending(x => x).Take(3).Sum();

        // Assert
        _ = actual.Should().Be(207576);
    }

    private static async Task<List<int>> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day01.txt");
        List<int> calories = new();
        int currentCalorieCount = 0;
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                calories.Add(currentCalorieCount);
                currentCalorieCount = 0;
            }
            else
            {
                currentCalorieCount += int.Parse(line);
            }
        }

        return calories;
    }
}
