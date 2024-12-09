namespace AOC2024;

public sealed class Day08_ResonantCollinearity
{
    private const string FileName = "Data/Day08.txt";

    [Fact]
    public async Task Test1()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        int mapWidth = map[0].Length;
        int mapHeight = map.Length;
        Dictionary<char, List<Point>> antennas = GetAntennas(map, mapWidth, mapHeight);
        HashSet<Point> antinodes = [];
        foreach (List<Point> locations in antennas.Values)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                Point p1 = locations[i];
                for (int j = i + 1; j < locations.Count; j++)
                {
                    Point p2 = locations[j];
                    Size distance = new(Point.Subtract(p1, new Size(p2)));
                    Point antinode1 = Point.Add(p1, distance);
                    Point antinode2 = Point.Subtract(p2, distance);
                    if (!antinode1.IsOutOfBounds(map)) antinodes.Add(antinode1);
                    if (!antinode2.IsOutOfBounds(map)) antinodes.Add(antinode2);
                }
            }
        }
        int answer = antinodes.Count;
        answer.Should().Be(354);
    }

    [Fact]
    public async Task Test2()
    {
        string[] map = await File.ReadAllLinesAsync(FileName);
        int mapWidth = map[0].Length;
        int mapHeight = map.Length;
        Dictionary<char, List<Point>> antennas = GetAntennas(map, mapWidth, mapHeight);
        HashSet<Point> antinodes = [];
        foreach (List<Point> locations in antennas.Values)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                Point p1 = locations[i];
                for (int j = i + 1; j < locations.Count; j++)
                {
                    Point p2 = locations[j];
                    Size distance = new(Point.Subtract(p1, new Size(p2)));
                    bool isOutOfBounds = false;
                    Point antinode = p1;
                    while (!isOutOfBounds)
                    {
                        antinodes.Add(antinode);
                        antinode = Point.Add(antinode, distance);
                        isOutOfBounds = antinode.IsOutOfBounds(map);
                    }
                    isOutOfBounds = false;
                    antinode = p2;
                    while (!isOutOfBounds)
                    {
                        antinodes.Add(antinode);
                        antinode = Point.Subtract(antinode, distance);
                        isOutOfBounds = antinode.IsOutOfBounds(map);
                    }
                }
            }
        }
        int answer = antinodes.Count;
        answer.Should().Be(1263);
    }

    private static Dictionary<char, List<Point>> GetAntennas(string[] map, int mapWidth, int mapHeight)
    {
        Dictionary<char, List<Point>> antennas = [];
        for (int y = 0; y < mapHeight; y++)
        {
            string line = map[y];
            for (int x = 0; x < mapWidth; x++)
            {
                char c = line[x];
                if (c.Equals('.')) continue;
                if (!antennas.TryGetValue(c, out _)) antennas[c] = [];
                antennas[c].Add(new Point(x, y));
            }
        }
        return antennas;
    }
}
