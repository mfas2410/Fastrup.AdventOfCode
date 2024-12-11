namespace AOC2024;

public sealed class Day10_HoofIt
{
    private const string FileName = "Data/Day10.txt";

    [Fact]
    public async Task Test1()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        LocateStartingPositions(map)
            .Sum(start => TraverseMapUnique(map, start))
            .Should()
            .Be(778);
    }

    [Fact]
    public async Task Test2()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        LocateStartingPositions(map)
            .Sum(start => TraverseMapAll(map, start))
            .Should()
            .Be(1_925);
    }

    private static List<Point> LocateStartingPositions(string[] map)
    {
        List<Point> startingPositions = [];
        for (int y = 0; y < map.Length; y++)
        {
            ReadOnlySpan<char> line = map[y];
            int x = 0;
            int index;
            while ((index = line[x..].IndexOf('0')) != -1)
            {
                startingPositions.Add(new Point(x + index, y));
                x += index + 1;
            }
        }
        return startingPositions;
    }

    private static int TraverseMapUnique(string[] map, Point start)
    {
        int mapHeight = map.Length;
        int mapWidth = map[0].Length;
        HashSet<Point> uniqueEndpoints = [];
        DepthFirstSearch(map, start, [], mapHeight, mapWidth, uniqueEndpoints);
        return uniqueEndpoints.Count;
    }

    private static int TraverseMapAll(string[] map, Point start)
    {
        int mapHeight = map.Length;
        int mapWidth = map[0].Length;
        return DepthFirstSearch(map, start, [], mapHeight, mapWidth, []);
    }

    private static int DepthFirstSearch(string[] array, Point current, List<Point> visited, int rows, int cols, HashSet<Point> uniqueEndpoints)
    {
        int currentValue = array[current.Y][current.X] - '0';
        if (currentValue == 9)
        {
            uniqueEndpoints.Add(current);
            return 1;
        }
        visited.Add(current);
        int pathCount = 0;
        foreach (Point neighbor in GetNeighbors(current, rows, cols))
        {
            if (visited.Contains(neighbor)) continue;
            int neighborValue = array[neighbor.Y][neighbor.X] - '0';
            if (neighborValue == currentValue + 1) pathCount += DepthFirstSearch(array, neighbor, [..visited], rows, cols, uniqueEndpoints);
        }
        return pathCount;
    }

    private static IEnumerable<Point> GetNeighbors(Point point, int rows, int cols)
    {
        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];
        for (int index = 0; index < 4; index++)
        {
            int newX = point.X + dx[index];
            int newY = point.Y + dy[index];
            if (newX >= 0 && newX < cols && newY >= 0 && newY < rows) yield return new Point(newX, newY);
        }
    }
}
