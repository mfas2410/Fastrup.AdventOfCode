namespace AOC2021;

public class Day02_Dive
{
    private readonly ITestOutputHelper _output;
    private readonly IEnumerable<(string Action, int Value)> _data;

    public Day02_Dive(ITestOutputHelper output)
    {
        _output = output;
        _data = File.ReadAllLines(@"Data\Day02.txt").Select(line =>
        {
            string[] command = line.Split(' ');
            return (command[0], Convert.ToInt32(command[1]));
        });
    }

    [Fact]
    public void Task1_CalculateHorizontalPositionAndDepth()
    {
        // Arrange
        int depth = 0;
        int position = 0;

        // Act
        foreach ((string Action, int Value) line in _data)
        {
            if (line.Action.Equals("up")) depth -= line.Value;
            if (line.Action.Equals("down")) depth += line.Value;
            if (line.Action.Equals("forward")) position += line.Value;
        }

        // Assert
        int result = depth * position;
        _output.WriteLine($"Depth {depth} * Position {position} = {result}");
        _ = result.Should().Be(1815044);
    }

    [Fact]
    public void Task2_CalculateRevisedHorizontalPositionAndDepth()
    {
        // Arrange
        int aim = 0;
        int depth = 0;
        int position = 0;

        // Act
        foreach ((string Action, int Value) line in _data)
        {
            if (line.Action.Equals("up")) aim -= line.Value;
            if (line.Action.Equals("down")) aim += line.Value;
            if (line.Action.Equals("forward"))
            {
                depth += aim * line.Value;
                position += line.Value;
            }
        }

        // Assert
        int result = depth * position;
        _output.WriteLine($"Depth {depth} * Position {position} = {result}");
        _ = result.Should().Be(1739283308);
    }
}