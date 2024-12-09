namespace AOC2021;

public class Day07_The_Treachery_of_Whales
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        IEnumerable<int> crabPositions = await Initialize();

        // Act
        int result = FuelCost(crabPositions.Min(), crabPositions.Max(), crabPositions).Min();

        // Assert
        _ = result.Should().Be(356922);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        IEnumerable<int> crabPositions = await Initialize();

        // Act
        int result = FuelCostWithConstantIncrease(crabPositions.Min(), crabPositions.Max(), crabPositions).Min();

        // Assert
        _ = result.Should().Be(100347031);
    }

    private static async Task<IEnumerable<int>> Initialize()
    {
        return (await File.ReadAllTextAsync(@"Data\Day07.txt"))
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));
    }

    private static IEnumerable<int> FuelCost(int min, int max, IEnumerable<int> positions)
    {
        return Enumerable.Range(min, max).Select(index => positions.Select(position => Math.Abs(position - index)).Sum());
    }

    private static IEnumerable<int> FuelCostWithConstantIncrease(int min, int max, IEnumerable<int> positions)
    {
        return Enumerable.Range(min, max).Select(index => positions.Select(position => Enumerable.Range(0, Math.Abs(position - index) + 1).Sum()).Sum());
    }
}