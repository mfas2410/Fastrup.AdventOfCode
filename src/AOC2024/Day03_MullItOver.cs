namespace AOC2024;

public sealed class Day03_MullItOver
{
    private const string FileName = "Data/Day03.txt";
    private static readonly Regex MulRegex = new(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
    private static readonly Regex DoDontMulRegex = new(@"(do|don't)\(\)|mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);

    [Fact]
    public async Task Test1() =>
        MulRegex.Matches(await File.ReadAllTextAsync(FileName))
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
            .Should().Be(164_730_528);

    [Fact]
    public async Task Test2()
    {
        int answer = 0;
        bool enabled = true;
        foreach (Match match in DoDontMulRegex.Matches(await File.ReadAllTextAsync(FileName)))
        {
            if (match.Groups[1].Success)
            {
                enabled = match.Groups[1].Value.Equals("do", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                if (enabled) answer += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }
        answer.Should().Be(70_478_672);
    }
}
