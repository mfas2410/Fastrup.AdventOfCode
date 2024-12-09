namespace Common;

public static class PointExtensions
{
    public static bool IsOutOfBounds(this Point point, string[] lines) =>
        point.X < 0 || point.X >= lines[0].Length || point.Y < 0 || point.Y >= lines.Length;
}
