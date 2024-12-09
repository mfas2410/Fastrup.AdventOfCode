namespace AOC2022;

public class Day02_Rock_Paper_Scissors
{
    [Fact]
    public async Task TotalScoreByShape()
    {
        // Arrange
        List<(char, char)> input = await Initialize();

        // Act
        List<RockPaperScissorsSolver1> solvers = input.Select(x => new RockPaperScissorsSolver1(x.Item1, x.Item2)).ToList();
        int actual = solvers.Sum(x => x.Score);

        // Assert
        _ = actual.Should().Be(15572);
    }

    [Fact]
    public async Task TotalScoreByOutcome()
    {
        // Arrange
        List<(char, char)> input = await Initialize();

        // Act
        List<RockPaperScissorsSolver2> solvers = input.Select(x => new RockPaperScissorsSolver2(x.Item1, x.Item2)).ToList();
        int actual = solvers.Sum(x => x.Score);

        // Assert
        _ = actual.Should().Be(16098);
    }

    private static async Task<List<(char, char)>> Initialize()
    {
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day02.txt");
        List<(char, char)> input = new();
        foreach (string line in lines)
        {
            string[] strings = line.Split(' ');
            input.Add((strings[0][0], strings[1][0]));
        }

        return input;
    }

    private sealed class RockPaperScissorsSolver1
    {
        private readonly Dictionary<char, Shapes> shapeConversions = new()
        {
            { 'A', Shapes.Rock },
            { 'B', Shapes.Paper },
            { 'C', Shapes.Scissors },
            { 'X', Shapes.Rock },
            { 'Y', Shapes.Paper },
            { 'Z', Shapes.Scissors }
        };

        public enum Shapes
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        public enum Outcomes
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }

        public RockPaperScissorsSolver1(char input1, char input2)
        {
            Shape1 = shapeConversions[input1];
            Shape2 = shapeConversions[input2];
        }

        public Shapes Shape1 { get; }

        public Shapes Shape2 { get; }

        public bool IsWin =>
            (Shape2 == Shapes.Rock && Shape1 == Shapes.Scissors) ||
            (Shape2 == Shapes.Scissors && Shape1 == Shapes.Paper) ||
            (Shape2 == Shapes.Paper && Shape1 == Shapes.Rock);

        public bool IsDraw => Shape1 == Shape2;

        public Outcomes Outcome => IsWin ? Outcomes.Win : IsDraw ? Outcomes.Draw : Outcomes.Lose;

        public int Score => (int)Outcome + (int)Shape2;
    }

    private sealed class RockPaperScissorsSolver2
    {
        private readonly Dictionary<char, Shapes> shapeConversions = new()
        {
            { 'A', Shapes.Rock },
            { 'B', Shapes.Paper },
            { 'C', Shapes.Scissors },
            { 'X', Shapes.Rock },
            { 'Y', Shapes.Paper },
            { 'Z', Shapes.Scissors }
        };

        public enum Shapes
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        private readonly Dictionary<char, Outcomes> outcomeConversions = new()
        {
            { 'X', Outcomes.Lose },
            { 'Y', Outcomes.Draw },
            { 'Z', Outcomes.Win }
        };

        public enum Outcomes
        {
            Lose = 0,
            Draw = 3,
            Win = 6
        }

        public RockPaperScissorsSolver2(char input1, char input2)
        {
            Shape1 = shapeConversions[input1];
            Outcome = outcomeConversions[input2];
            Shape2 = Outcome switch
            {
                Outcomes.Lose => Shape1 == Shapes.Rock ? Shapes.Scissors : Shape1 == Shapes.Paper ? Shapes.Rock : Shapes.Paper,
                Outcomes.Draw => Shape1,
                Outcomes.Win => Shape1 == Shapes.Rock ? Shapes.Paper : Shape1 == Shapes.Paper ? Shapes.Scissors : Shapes.Rock,
                _ => throw new NotImplementedException(),
            };
        }

        public Shapes Shape1 { get; }

        public Shapes Shape2 { get; }

        public Outcomes Outcome { get; }

        public int Score => (int)Outcome + (int)Shape2;
    }
}
