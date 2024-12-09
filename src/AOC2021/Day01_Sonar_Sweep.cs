namespace AOC2021;

public class Day01_Sonar_Sweep
{
    [Fact]
    public async Task Task1_HowManyMeasurementsAreLargerThanThePreviousMeasurement()
    {
        // Arrange
        int[] data = await Initialize();
        int result = 0;

        // Act
        for (int index = 0; index < data.Length - 1; index++)
        {
            int currentDepth = data[index];
            int nextDepth = data[index + 1];
            if (nextDepth > currentDepth) result++;
        }

        // Assert
        _ = result.Should().Be(1477);
    }

    [Fact]
    public async Task Task2_HowManySumsAreLargerThanThePreviousSum()
    {
        // Arrange
        int[] data = await Initialize();
        int result = 0;

        // Act
        for (int index = 0; index < data.Length - 3; index++)
        {
            int currentDepth = data[index..(index + 3)].Sum();
            int nextDepth = data[(index + 1)..(index + 4)].Sum();
            if (nextDepth > currentDepth) result++;
        }

        // Assert
        _ = result.Should().Be(1523);
    }

    private async Task<int[]> Initialize()
    {
        return (await File.ReadAllLinesAsync(@"Data\Day01.txt"))
            .Select(x => Convert.ToInt32(x))
            .ToArray();
    }
}