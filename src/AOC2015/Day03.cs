namespace AOC2015;
public class Day03
{
    [Fact]
    public async Task Task1()
    {
        // Arrange
        string directions = await Initialize();

        // Act
        Coordinate current = new(0, 0);
        Dictionary<Coordinate, int> coordinates = new() { { current, 1 } };
        for (int index = 0; index < directions.Length; index++)
        {
            current = UpdateFromDirection(directions[index], current, coordinates);
        }

        // Assert
        _ = coordinates.Count.Should().Be(2081);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        string directions = await Initialize();

        // Act
        Coordinate santa = new(0, 0);
        Coordinate roboSanta = new(0, 0);
        Dictionary<Coordinate, int> coordinates = new() { { santa, 2 } };
        for (int index = 0; index < directions.Length; index += 2)
        {
            santa = UpdateFromDirection(directions[index], santa, coordinates);
            roboSanta = UpdateFromDirection(directions[index + 1], roboSanta, coordinates);
        }

        // Assert
        _ = coordinates.Count.Should().Be(2341);
    }

    private static Coordinate UpdateFromDirection(char direction, Coordinate coordinate, Dictionary<Coordinate, int> coordinates)
    {
        coordinate = CreateCoordinate(direction, coordinate);
        if (!coordinates.ContainsKey(coordinate)) coordinates[coordinate] = 0;
        coordinates[coordinate] += 1;
        return coordinate;
    }

    private static Coordinate CreateCoordinate(char direction, Coordinate current)
    {
        switch (direction)
        {
            case '^':
                {
                    current = current with { Y = current.Y + 1 };
                    break;
                }
            case '>':
                {
                    current = current with { X = current.X + 1 };
                    break;
                }
            case 'v':
                {
                    current = current with { Y = current.Y - 1 };
                    break;
                }
            case '<':
                {
                    current = current with { X = current.X - 1 };
                    break;
                }
        }

        return current;
    }

    private static async Task<string> Initialize()
    {
        return await File.ReadAllTextAsync(@"Data/Day03.txt");
    }

    private record Coordinate(int X, int Y);
}
