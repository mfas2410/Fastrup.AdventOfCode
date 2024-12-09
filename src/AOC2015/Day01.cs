namespace AOC2015;
public sealed class Day01
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        string data = await Initialize();

        // Act
        int floor = 0;
        for (int index = 0; index < data.Length; index++)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            floor += data[index] switch
            {
                '(' => 1,
                ')' => -1
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
        }

        // Assert
        _ = floor.Should().Be(138);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        string data = await Initialize();

        // Act
        int floor = 0;
        int index;
        for (index = 0; index < data.Length; index++)
        {
            floor += data[index] switch
            {
                '(' => 1,
                ')' => -1
            };

            if (floor == -1)
            {
                index++;
                break;
            }
        }

        // Assert
        _ = index.Should().Be(1771);
    }

    private static async Task<string> Initialize()
    {
        return await File.ReadAllTextAsync(@"Data/Day01.txt");
    }
}
