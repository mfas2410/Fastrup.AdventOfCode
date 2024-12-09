namespace AOC2024;

public sealed class Day06_GuardGallivant
{
    private const string FileName = "Data/Day06.txt";

    [Fact]
    public async Task Test1()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        HashSet<Point> positions = [];
        Guard guard = new(LocateGuard(map), Direction.North);
        while (true)
        {
            positions.Add(guard.Position);
            Point point = Walk(guard);
            if (point.IsOutOfBounds(map)) break;
            if (map[point.Y][point.X].Equals('#'))
            {
                guard = guard with { Direction = Turn(guard.Direction) };
            }
            else
            {
                guard = guard with { Position = point };
            }
        }
        int answer = positions.Count;
        answer.Should().Be(5404);
    }

    [Fact]
    public async Task Test2()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        HashSet<Point> positions = [];
        Guard guard = new(LocateGuard(map), Direction.North);
        int answer = 0;
        while (true)
        {
            positions.Add(guard.Position);
            Point point = Walk(guard);
            if (point.IsOutOfBounds(map)) break;
            if (map[point.Y][point.X].Equals('#'))
            {
                guard = guard with { Direction = Turn(guard.Direction) };
            }
            else
            {
                if (map[point.Y][point.X].Equals('.') && !positions.Contains(point))
                {
                    map[point.Y] = map[point.Y][..point.X] + '#' + map[point.Y][(point.X + 1)..];
                    answer += TestForLoop(map, guard);
                    map[point.Y] = map[point.Y][..point.X] + '.' + map[point.Y][(point.X + 1)..];
                }
                guard = guard with { Position = point };
            }
        }
        answer.Should().Be(1984);
    }

    private static int TestForLoop(string[] map, Guard guard)
    {
        HashSet<Guard> positions = [];
        while (true)
        {
            if (!positions.Add(guard)) return 1;
            Point point = Walk(guard);
            if (point.IsOutOfBounds(map)) return 0;
            if (map[point.Y][point.X].Equals('#'))
            {
                guard = guard with { Direction = Turn(guard.Direction) };
            }
            else
            {
                guard = guard with { Position = point };
            }
        }
    }

    private static Point LocateGuard(string[] map)
    {
        for (int y = 0; y < map.Length; y++)
        {
            ReadOnlySpan<char> line = map[y];
            int x = line.IndexOf('^');
            if (x != -1) return new Point(x, y);
        }
        throw new Exception("Should never reach this point");
    }

    private static Point Walk(Guard position) =>
        position.Direction switch
        {
            Direction.North => position.Position with { Y = position.Position.Y - 1 },
            Direction.East => position.Position with { X = position.Position.X + 1 },
            Direction.South => position.Position with { Y = position.Position.Y + 1 },
            Direction.West => position.Position with { X = position.Position.X - 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };

    private static Direction Turn(Direction direction) =>
        direction + 1 > Direction.West ? Direction.North : direction + 1;

    private sealed record Guard(Point Position, Direction Direction);

    private enum Direction
    {
        North,
        East,
        South,
        West
    }
}
