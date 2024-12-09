namespace AOC2021;

public class Day09_Smoke_Basin
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        int[,] map = await Initialize();

        // Act
        int result = GetLowPoints(map)
            .Select(point => map[point.X, point.Y] + 1)
            .Sum();

        // Assert
        _ = result.Should().Be(539);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        int[,] map = await Initialize();

        // Act
        int result = GetLowPoints(map)
            .Select(point => GetBasinSize(map, point))
            .OrderByDescending(size => size)
            .Take(3)
            .Aggregate((sum, number) => sum * number);

        // Assert
        _ = result.Should().Be(736920);
    }

    private async Task<int[,]> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day09.txt");
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

    private static IEnumerable<Point> GetLowPoints(int[,] map)
    {
        List<Point> lowPoints = new();
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (map[row, col] == 9) continue;
                int up = row - 1 < 0 ? 9 : map[row - 1, col];
                int down = row + 1 >= map.GetLength(0) ? 9 : map[row + 1, col];
                int right = col + 1 >= map.GetLength(1) ? 9 : map[row, col + 1];
                int left = col - 1 < 0 ? 9 : map[row, col - 1];
                if (map[row, col] < up && map[row, col] < down && map[row, col] < right && map[row, col] < left) lowPoints.Add(new Point(row, col));
            }
        }

        return lowPoints;
    }

    private static int GetBasinSize(int[,] map, Point point)
    {
        if (point.X < 0 || point.X >= map.GetLength(1) || point.Y < 0 || point.Y >= map.GetLength(0) || map[point.Y, point.X] == 9) return 0;
        map[point.Y, point.X] = 9;
        return 1 + GetBasinSize(map, new Point(point.X - 1, point.Y)) + GetBasinSize(map, new Point(point.X + 1, point.Y)) + GetBasinSize(map, new Point(point.X, point.Y - 1)) + GetBasinSize(map, new Point(point.X, point.Y + 1));
    }
}