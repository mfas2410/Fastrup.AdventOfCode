namespace AOC2024;

public sealed class Day12_GardenGroups
{
    private const string FileName = "Data/Day12.txt";

    [Fact]
    public async Task Test1()
    {
        char[,] map = await ReadMapAsync(FileName);
        List<(int perimeter, int size)> groupDetails = OutlineGroups(map);
        int answer = groupDetails.Sum(tuple => tuple.perimeter * tuple.size);
        answer.Should().Be(1431440);
    }

    private static async Task<char[,]> ReadMapAsync(string fileName)
    {
        string[] lines = await File.ReadAllLinesAsync(fileName);
        int rows = lines.Length;
        int cols = lines[0].Length;
        char[,] map = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = lines[i][j];
            }
        }
        return map;
    }

    private static List<(int perimeter, int size)> OutlineGroups(char[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        bool[,] visited = new bool[rows, cols];
        List<(int perimeter, int size)> groupDetails = [];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (visited[i, j]) continue;
                HashSet<(int, int)> group = [];
                FloodFill(map, visited, i, j, map[i, j], group);
                int perimeter = CalculatePerimeter(group, map);
                groupDetails.Add((perimeter, group.Count));
            }
        }
        return groupDetails;
    }

    private static void FloodFill(char[,] map, bool[,] visited, int x, int y, char targetChar, HashSet<(int, int)> group)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        if (x < 0 || x >= rows || y < 0 || y >= cols || visited[x, y] || map[x, y] != targetChar) return;
        visited[x, y] = true;
        group.Add((x, y));
        FloodFill(map, visited, x + 1, y, targetChar, group);
        FloodFill(map, visited, x - 1, y, targetChar, group);
        FloodFill(map, visited, x, y + 1, targetChar, group);
        FloodFill(map, visited, x, y - 1, targetChar, group);
    }

    private static int CalculatePerimeter(HashSet<(int, int)> group, char[,] map)
    {
        int perimeter = 0;
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        foreach ((int x, int y) in group)
        {
            if (x == 0 || map[x - 1, y] != map[x, y]) perimeter++;
            if (x == rows - 1 || map[x + 1, y] != map[x, y]) perimeter++;
            if (y == 0 || map[x, y - 1] != map[x, y]) perimeter++;
            if (y == cols - 1 || map[x, y + 1] != map[x, y]) perimeter++;
        }
        return perimeter;
    }
}
