namespace AOC2021;

public class Day13_Transparent_Origami
{
    private readonly ITestOutputHelper _output;

    public Day13_Transparent_Origami(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        (IEnumerable<Point> points, FoldInstruction[] foldInstructions) = await Initialize();
        int[,] matrix = CreateMatrix(points);

        // Act
        matrix = Fold(matrix, foldInstructions.First());

        int result = 0;
        foreach (int value in matrix)
        {
            if (value > 0) result++;
        }

        // Assert
        _ = result.Should().Be(807);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        (IEnumerable<Point> points, FoldInstruction[] foldInstructions) = await Initialize();
        int[,] matrix = CreateMatrix(points);

        // Act
        matrix = Fold(matrix, foldInstructions);

        // Assert
        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            string line = string.Empty;
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                line += matrix[row, col] > 0 ? 'X' : ' ';
            }

            _output.WriteLine(line); // LGHEGUEJ
        }
    }

    private static async Task<(IEnumerable<Point>, FoldInstruction[])> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day13.txt");
        List<Point> points = new();
        int row = 0;
        for (; row < lines.Length; row++)
        {
            string line = lines[row];
            if (string.IsNullOrEmpty(line)) break;
            List<int> numbers = line.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Convert.ToInt32);
            points.Add(new Point(numbers[0], numbers[1]));
        }

        List<FoldInstruction> foldInstructions = new();
        row++;
        for (; row < lines.Length; row++)
        {
            string[] line = lines[row][11..].Split('=');
            foldInstructions.Add(new FoldInstruction(line[0][0], Convert.ToInt32(line[1])));
        }

        return (points, foldInstructions.ToArray());
    }

    private static int[,] CreateMatrix(IEnumerable<Point> points)
    {
        int[,] matrix = new int[points.Select(y => y.Y).Max() + 1, points.Select(x => x.X).Max() + 1];
        foreach (Point point in points)
        {
            matrix[point.Y, point.X] = 1;
        }

        return matrix;
    }

    private static int[,] Fold(int[,] matrix, params FoldInstruction[] foldInstructions)
    {
        int[,] result = (int[,])matrix.Clone();
        foreach (FoldInstruction foldInstruction in foldInstructions)
        {
            switch (foldInstruction.Axis)
            {
                case 'x':
                    {
                        for (int row = 0; row < result.GetLength(0); row++)
                        {
                            for (int col = 1; col <= foldInstruction.Index; col++)
                            {
                                result[row, foldInstruction.Index - col] += result[row, foldInstruction.Index + col];
                            }
                        }

                        result = Crop(result, result.GetLength(0), foldInstruction.Index);
                        break;
                    }
                case 'y':
                    {
                        for (int row = 1; row <= foldInstruction.Index; row++)
                        {
                            for (int col = 0; col < result.GetLength(1); col++)
                            {
                                result[foldInstruction.Index - row, col] += result[foldInstruction.Index + row, col];
                            }
                        }

                        result = Crop(result, foldInstruction.Index, result.GetLength(1));
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        return result;
    }

    private static int[,] Crop(int[,] matrix, int rows, int columns)
    {
        rows = Math.Min(matrix.GetLength(0), rows);
        columns = Math.Min(matrix.GetLength(1), columns);
        int[,] result = new int[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                result[row, col] = matrix[row, col];
            }
        }

        return result;
    }

    private sealed record FoldInstruction(char Axis, int Index);
}
