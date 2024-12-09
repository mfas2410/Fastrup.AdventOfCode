namespace AOC2021;

public class Day11_Dumbo_Octopus
{
    [Fact]
    public async Task Task1_TotalFlashesAfter100Steps()
    {
        // Arrange
        int[,] octopuses = await Initialize();
        int result = 0;

        // Act
        for (int count = 0; count < 100; count++)
        {
            IncreaseEnergyLevel(octopuses);
            result += Flash(octopuses);
        }

        // Assert
        _ = result.Should().Be(1732);
    }

    [Fact]
    public async Task Task2_FirstStepWhereAllOctopusesFlash()
    {
        // Arrange
        int[,] octopuses = await Initialize();
        int result = 0;

        // Act
        do
        {
            IncreaseEnergyLevel(octopuses);
            _ = Flash(octopuses);
            result++;
        } while (!AllFlashed(octopuses));

        // Assert
        _ = result.Should().Be(290);
    }

    private static async Task<int[,]> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day11.txt");
        int[,] result = new int[lines.Length, lines[0].Length];
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                result[row, col] = (int)char.GetNumericValue(lines[row][col]);
            }
        }

        return result;
    }

    private static void IncreaseEnergyLevel(int[,] octopuses)
    {
        for (int row = 0; row < octopuses.GetLength(0); row++)
        {
            for (int col = 0; col < octopuses.GetLength(1); col++)
            {
                octopuses[row, col]++;
            }
        }
    }

    private static int Flash(int[,] octopuses)
    {
        int result = 0;
        for (int row = 0; row < octopuses.GetLength(0); row++)
        {
            for (int col = 0; col < octopuses.GetLength(1); col++)
            {
                if (octopuses[row, col] == 10) result += DoFlash(octopuses, new Point(col, row));
            }
        }

        return result;
    }

    private static int DoFlash(int[,] octopuses, Point point)
    {
        if (point.X < 0 || point.X >= octopuses.GetLength(1) || point.Y < 0 || point.Y >= octopuses.GetLength(0) || octopuses[point.Y, point.X] == 0 || ++octopuses[point.Y, point.X] < 10) return 0;
        octopuses[point.Y, point.X] = 0;
        return 1 +
            DoFlash(octopuses, new Point(point.X - 1, point.Y - 1)) +
            DoFlash(octopuses, new Point(point.X, point.Y - 1)) +
            DoFlash(octopuses, new Point(point.X + 1, point.Y - 1)) +
            DoFlash(octopuses, new Point(point.X - 1, point.Y)) +
            DoFlash(octopuses, new Point(point.X + 1, point.Y)) +
            DoFlash(octopuses, new Point(point.X - 1, point.Y + 1)) +
            DoFlash(octopuses, new Point(point.X, point.Y + 1)) +
            DoFlash(octopuses, new Point(point.X + 1, point.Y + 1));
    }

    private static bool AllFlashed(int[,] octopuses)
    {
        bool result = true;
        foreach (int value in octopuses)
        {
            result &= value == 0;
            if (!result) break;
        }

        return result;
    }
}