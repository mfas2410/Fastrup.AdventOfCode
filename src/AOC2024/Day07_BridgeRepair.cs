namespace AOC2024;

public sealed class Day07_BridgeRepair
{
    private const string FileName = "Data/Day07.txt";

    [Fact]
    public async Task Test1()
    {
        string[] equations = await File.ReadAllLinesAsync(FileName);
        long answer = 0;
        foreach (string equation in equations)
        {
            string[] parts = equation.Split(':');
            long testValue = long.Parse(parts[0]);
            long[] numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            if (CanReachTestValue(testValue, numbers, 0, 0, 0)) answer += testValue;
        }
        answer.Should().Be(4122618559853);
    }

    [Fact]
    public async Task Test2()
    {
        string[] equations = await File.ReadAllLinesAsync(FileName);
        long answer = 0;
        foreach (string equation in equations)
        {
            string[] parts = equation.Split(':');
            long testValue = long.Parse(parts[0]);
            long[] numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            if (CanReachTestValueWithPairing(testValue, numbers, 0, 0, 0)) answer += testValue;
        }
        answer.Should().Be(227615740238334);
    }

    private static bool CanReachTestValue(long testValue, long[] numbers, int index, long currentSum, int usedNumbers)
    {
        if (index == numbers.Length) return currentSum == testValue && usedNumbers == numbers.Length;
        if (CanReachTestValue(testValue, numbers, index + 1, currentSum + numbers[index], usedNumbers + 1)) return true;
        if (CanReachTestValue(testValue, numbers, index + 1, currentSum * numbers[index], usedNumbers + 1)) return true;
        return false;
    }

    private static bool CanReachTestValueWithPairing(long testValue, long[] numbers, int index, long currentSum, int usedNumbers)
    {
        if (index == numbers.Length) return currentSum == testValue && usedNumbers == numbers.Length;
        if (CanReachTestValueWithPairing(testValue, numbers, index + 1, currentSum + numbers[index], usedNumbers + 1)) return true;
        if (CanReachTestValueWithPairing(testValue, numbers, index + 1, currentSum * numbers[index], usedNumbers + 1)) return true;
        if (index < numbers.Length && CanReachTestValueWithPairing(testValue, numbers, index + 1, long.Parse($"{currentSum}{numbers[index]}"), usedNumbers + 1)) return true;
        return false;
    }
}
