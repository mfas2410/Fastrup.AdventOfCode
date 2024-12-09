namespace AOC2021;

public class Day15_Chiton
{
    [Fact]
    public void Task1()
    {
        // Arrange
        //int[,] matrix = await Initialize();

        //// Act
        //int result = Sum(0, 0, matrix) - matrix[0,0];

        //// Assert
        //result.Should().Be(0);
    }

    private int Sum(int row, int col, int[,] matrix)
    {
        if (row == matrix.GetLength(0) - 1 && col == matrix.GetLength(1) - 1) return matrix[row, col];
        return row == matrix.GetLength(0) - 1
            ? matrix[row, col] + Sum(row, col + 1, matrix)
            : col == matrix.GetLength(1) - 1
            ? matrix[row, col] + Sum(row + 1, col, matrix)
            : matrix[row, col] + Math.Min(Sum(row + 1, col, matrix), Sum(row, col + 1, matrix));
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        _ = await Initialize();

        // Act

        // Assert
    }

    private static async Task<int[,]> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day15.txt");
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
}