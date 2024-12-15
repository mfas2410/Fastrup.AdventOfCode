namespace AOC2024;

public sealed class Day15_WarehouseWoes
{
    private const string FileName = "Data/Day15.txt";

    [Fact]
    public async Task Test1()
    {
        (char[,] map, Point position, char[] moves) = await GetData();
        ExecuteMoves(map, moves, position);
        int answer = 0;
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 'O') answer += (y * 100) + x;
            }
        }
        answer.Should().Be(1436690);
    }

    private static async Task<(char[,] map, Point position, char[] moves)> GetData()
    {
        string[] lines = await File.ReadAllLinesAsync(FileName);
        int height = Array.FindIndex(lines, string.IsNullOrWhiteSpace);
        int width = lines[0].Length;
        char[,] map = new char[height, width];
        Point position = default;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[y, x] = lines[y][x];
                if (map[y, x] == '@') position = new Point(x, y);
            }
        }
        string concatenatedLines = string.Concat(lines.Skip(height + 1));
        char[] moves = concatenatedLines.ToCharArray();
        return (map, position, moves);
    }

    private static void ExecuteMoves(char[,] map, char[] moves, Point position)
    {
        Dictionary<char, Point> directions = new()
        {
            { '^', new Point(0, -1) },
            { '>', new Point(1, 0) },
            { '<', new Point(-1, 0) },
            { 'v', new Point(0, 1) }
        };
        foreach (char move in moves)
        {
            Point direction = directions[move];
            Point next = new(position.X + direction.X, position.Y + direction.Y);
            while (map[next.Y, next.X] == 'O')
            {
                next = new Point(next.X + direction.X, next.Y + direction.Y);
            }
            if (map[next.Y, next.X] == '#') continue;
            do
            {
                Point previous = new(next.X - direction.X, next.Y - direction.Y);
                map[next.Y, next.X] = map[previous.Y, previous.X];
                position = next;
                next = previous;
            } while (map[next.Y, next.X] != '@');
            map[next.Y, next.X] = '.';
        }
    }
}
