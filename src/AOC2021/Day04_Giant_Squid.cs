namespace AOC2021;

public class Day04_Giant_Squid
{
    private readonly ITestOutputHelper _output;

    public Day04_Giant_Squid(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1_SumOfAllUnmarkedNumbersOfFirstWinningBoard()
    {
        // Arrange
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day04.txt");
        IEnumerable<int> drawNumbers = lines[0].Split(',').ToList().ConvertAll(Convert.ToInt32);
        IReadOnlyList<Board> boards = GenerateBoards(lines);

        // Act
        int winningNumber = 0;
        Board? winningBoard = null;
        foreach (int number in drawNumbers)
        {
            foreach (Board current in boards)
            {
                if (current.Mark(number))
                {
                    winningNumber = number;
                    winningBoard = current;
                    break;
                }
            }

            if (winningBoard is not null) break;
        }

        int sum = winningBoard!.SumUnmarked();

        // Assert
        int result = sum * winningNumber;
        _output.WriteLine($"Sum {sum} * Winning number {winningNumber} = {result}");
        _ = result.Should().Be(46920);
    }

    [Fact]
    public async Task Task2_SumOfAllUnmarkedNumbersOfLastWinningBoard()
    {
        // Arrange
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day04.txt");
        IEnumerable<int> drawNumbers = lines[0].Split(',').ToList().ConvertAll(Convert.ToInt32);
        IReadOnlyList<Board> boards = GenerateBoards(lines);
        List<int> boardIndices = Enumerable.Range(0, boards.Count).ToList();

        // Act
        int winningNumber = 0;
        Board? winningBoard = null;
        foreach (int number in drawNumbers)
        {
            List<int> hasBingo = new();
            foreach (int index in boardIndices)
            {
                Board current = boards[index];
                if (current.Mark(number)) hasBingo.Add(index);
            }

            boardIndices = boardIndices.Except(hasBingo).ToList();
            if (boardIndices.Count == 0)
            {
                winningNumber = number;
                winningBoard = boards[hasBingo[0]];
                break;
            }
        }

        int sum = winningBoard!.SumUnmarked();

        // Assert
        int result = sum * winningNumber;
        _output.WriteLine($"Sum {sum} * Winning number {winningNumber} = {result}");
        _ = result.Should().Be(12635);
    }

    private static IReadOnlyList<Board> GenerateBoards(string[] lines)
    {
        List<Board> boards = new();
        Board board = new();
        for (int index = 2; index < lines.Length; index++)
        {
            string line = lines[index];
            if (string.IsNullOrEmpty(line))
            {
                boards.Add(board);
                board = new();
            }
            else
            {
                board.AddRow(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(Convert.ToInt32));
            }
        }

        boards.Add(board);
        return boards;
    }

    private class Board
    {
        private readonly List<List<Number>> _board = new();

        public void AddRow(List<int> row)
        {
            _board.Add(Enumerable.Range(0, row.Count).Select(index => new Number(row[index], index)).ToList());
        }

        public bool Mark(int value)
        {
            for (int rowNumber = 0; rowNumber < _board.Count; rowNumber++)
            {
                List<Number> row = _board[rowNumber];
                Number? number = row.SingleOrDefault(x => x.Value == value);
                if (number is not null)
                {
                    number.Mark();
                    return HasBingo(rowNumber, number.Column);
                }
            }

            return false;
        }

        public int SumUnmarked()
        {
            return _board.SelectMany(x => x).Where(x => !x.Marked).Select(x => x.Value).Sum();
        }

        private bool HasBingo(int row, int column)
        {
            return _board[row].All(x => x.Marked) || _board.SelectMany(x => x).Where(x => x.Column == column).All(x => x.Marked);
        }
    }

    private class Number
    {
        public Number(int value, int column)
        {
            (Value, Column) = (value, column);
        }

        public int Column { get; }
        public bool Marked { get; private set; }
        public int Value { get; }

        public void Mark()
        {
            Marked = true;
        }

        public override string ToString()
        {
            return $"{Value} ({(Marked ? "X" : string.Empty)})";
        }
    }
}