namespace AOC2024;

public sealed class Day14_RestroomRedoubt
{
    private const string FileName = "Data/Day14.txt";

    [Fact]
    public async Task Test1()
    {
        Robot[] robots = await GetRobots();
        int width = 101;
        int height = 103;
        Robot[] movedRobots = MoveRobots(robots, 100, width, height);
        int x = width / 2;
        int y = height / 2;
        int quadrant1 = movedRobots.Count(robot => robot.Position.X < x && robot.Position.Y < y);
        int quadrant2 = movedRobots.Count(robot => robot.Position.X > x && robot.Position.Y < y);
        int quadrant3 = movedRobots.Count(robot => robot.Position.X < x && robot.Position.Y > y);
        int quadrant4 = movedRobots.Count(robot => robot.Position.X > x && robot.Position.Y > y);
        int answer = quadrant1 * quadrant2 * quadrant3 * quadrant4;
        answer.Should().Be(222062148);
    }

    private static async Task<Robot[]> GetRobots()
    {
        string[] lines = await File.ReadAllLinesAsync(FileName);
        Robot[] robots = new Robot[lines.Length];
        for (int index = 0; index < lines.Length; index++)
        {
            string line = lines[index];
            string[] parts = line.Split(' ');
            int splitAt = parts[0].IndexOf(',');
            int x = int.Parse(parts[0][2..splitAt]);
            int y = int.Parse(parts[0][(splitAt + 1)..]);
            Point startPosition = new(x, y);
            splitAt = parts[1].IndexOf(',');
            x = int.Parse(parts[1][2..splitAt]);
            y = int.Parse(parts[1][(splitAt + 1)..]);
            Point velocity = new(x, y);
            robots[index] = new Robot(startPosition, velocity);
        }
        return robots;
    }

    private static Robot[] MoveRobots(Robot[] robots, int iterations, int width, int height)
    {
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < robots.Length; j++)
            {
                robots[j] = MoveRobot(robots[j], width, height);
            }
        }
        return robots;
    }

    private static Robot MoveRobot(Robot robot, int width, int height)
    {
        int newX = (robot.Position.X + robot.Velocity.X) % width;
        int newY = (robot.Position.Y + robot.Velocity.Y) % height;
        if (newX < 0) newX += width;
        if (newY < 0) newY += height;
        robot.Position = new Point(newX, newY);
        return robot;
    }

    private sealed class Robot(Point position, Point velocity)
    {
        public Point Position { get; set; } = position;
        public Point Velocity { get; } = velocity;
    }
}
