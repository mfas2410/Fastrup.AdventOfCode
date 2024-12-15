namespace AOC2024;

public sealed class Day13_ClawContraption
{
    private const string FileName = "Data/Day13.txt";

    [Fact]
    public async Task Test1()
    {
        ClawContraption[] clawContraptions = await GetClawContraptions();
        (int a, int b) buttonPresses = (0, 0);
        foreach (ClawContraption clawContraption in clawContraptions)
        {
            (int a, int b) result = FindStepsToTarget(clawContraption);
            buttonPresses.a += result.a;
            buttonPresses.b += result.b;
        }
        int answer = (buttonPresses.a * 3) + buttonPresses.b;
        answer.Should().Be(34787);
    }

    private static async Task<ClawContraption[]> GetClawContraptions()
    {
        List<ClawContraption> result = [];
        Point buttonA = default;
        Point buttonB = default;
        Point prizeAt = default;
        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            if (line.StartsWith("Button A"))
            {
                buttonA = ParsePoint(line, '+');
            }
            else if (line.StartsWith("Button B"))
            {
                buttonB = ParsePoint(line, '+');
            }
            else if (line.StartsWith("Prize"))
            {
                prizeAt = ParsePoint(line, '=');
            }
            else if (line == "")
            {
                result.Add(new ClawContraption(buttonA, buttonB, prizeAt));
            }
        }
        result.Add(new ClawContraption(buttonA, buttonB, prizeAt));
        return result.ToArray();
    }

    private static Point ParsePoint(string line, char split)
    {
        int xStart = line.IndexOf(split) + 1;
        int x = int.Parse(line[xStart..line.IndexOf(',', xStart)]);
        int yStart = line.IndexOf(split, xStart) + 1;
        int y = int.Parse(line[yStart..]);
        return new Point(x, y);
    }

    private static (int a, int b) FindStepsToTarget(ClawContraption clawContraption)
    {
        for (int a = 0; a <= clawContraption.PrizeAt.X / clawContraption.ButtonA.X; a++)
        {
            for (int b = 0; b <= clawContraption.PrizeAt.Y / clawContraption.ButtonB.Y; b++)
            {
                if ((clawContraption.ButtonA.X * a) + (clawContraption.ButtonB.X * b) == clawContraption.PrizeAt.X && (clawContraption.ButtonA.Y * a) + (clawContraption.ButtonB.Y * b) == clawContraption.PrizeAt.Y)
                {
                    return (a, b);
                }
            }
        }
        return (0, 0);
    }

    private sealed record ClawContraption(Point ButtonA, Point ButtonB, Point PrizeAt);
}
