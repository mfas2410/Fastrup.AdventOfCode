namespace AOC2021;

public class Day05_Hydrothermal_Venture
{
    private readonly ITestOutputHelper _output;

    public Day05_Hydrothermal_Venture(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        IEnumerable<Line> lines = (await Initialize()).Where(x => x.Type != LineType.Diagonal);
        Matrix matrix = new(1000, 1000);

        // Act
        foreach (Line line in lines)
        {
            matrix.Apply(line);
        }

        // Assert
        int result = matrix.Values.Count(x => x > 1);
        _output.WriteLine($"Intersections > 1 = {result}");
        _ = result.Should().Be(6283);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        IEnumerable<Line> lines = await Initialize();
        Matrix matrix = new(1000, 1000);

        // Act
        foreach (Line line in lines)
        {
            matrix.Apply(line);
        }

        // Assert
        int result = matrix.Values.Count(x => x > 1);
        _output.WriteLine($"Intersections > 1 = {result}");
        _ = result.Should().Be(18864);
    }

    private static async Task<IEnumerable<Line>> Initialize()
    {
        List<Line> result = new();
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day05.txt");
        foreach (string line in lines)
        {
            List<Point> points = new();
            string[] stringLines = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
            foreach (string stringLine in stringLines)
            {
                List<int> numbers = stringLine.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Convert.ToInt32);
                points.Add(new Point(numbers[0] - 1, numbers[1] - 1));
            }

            result.Add(new Line(points[0], points[1]));
        }

        return result;
    }

    private class Matrix
    {
        private readonly int[,] _grid;

        public Matrix(int columns, int rows)
        {
            _grid = new int[columns, rows];
        }

        public void Apply(Line line)
        {
            foreach (Point point in line.Points)
            {
                _grid[point.X, point.Y] += 1;
            }
        }

        public IEnumerable<int> Values
        {
            get
            {
                foreach (int value in _grid)
                {
                    yield return value;
                }
            }
        }
    }

    private enum LineType
    {
        Horisontal,
        Vertical,
        Diagonal
    }

    private class Line
    {
        private readonly Point _from;
        private readonly Point _to;

        public Line(Point from, Point to)
        {
            (_from, _to) = (from, to);
        }

        public LineType Type => _from.Y == _to.Y ? LineType.Horisontal : _from.X == _to.X ? LineType.Vertical : LineType.Diagonal;

        public IEnumerable<Point> Points => Type switch
        {
            LineType.Horisontal => Enumerable.Range(Math.Min(_from.X, _to.X), Math.Abs(_from.X - _to.X) + 1).Select(x => new Point(x, _from.Y)),
            LineType.Vertical => Enumerable.Range(Math.Min(_from.Y, _to.Y), Math.Abs(_from.Y - _to.Y) + 1).Select(y => new Point(_from.X, y)),
            LineType.Diagonal => DiagonalPoints,
            _ => throw new NotImplementedException()
        };

        private IEnumerable<Point> DiagonalPoints
        {
            get
            {
                IReadOnlyList<int> xValues = Enumerable.Range(Math.Min(_from.X, _to.X), Math.Abs(_from.X - _to.X) + 1).ToList();
                if (_from.X > _to.X) xValues = xValues.Reverse().ToList();
                IReadOnlyList<int> yValues = Enumerable.Range(Math.Min(_from.Y, _to.Y), Math.Abs(_from.Y - _to.Y) + 1).ToList();
                if (_from.Y > _to.Y) yValues = yValues.Reverse().ToList();
                for (int i = 0; i < xValues.Count; i++)
                {
                    yield return new Point(xValues[i], yValues[i]);
                }
            }
        }
    }
}