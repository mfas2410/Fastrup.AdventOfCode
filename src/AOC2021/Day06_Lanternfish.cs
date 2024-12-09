namespace AOC2021;

public class Day06_Lanternfish
{
    [Theory]
    [InlineData(80, 352872)]
    [InlineData(256, 1604361182149)]
    public async Task GivenInitialBatchOfLanternFish_WhenGrowingForDays_ThenMoreFishHaveSpawned(int days, long expected)
    {
        // Arrange
        FishTank fishTank = new(await Initialize());

        // Act
        long actual = fishTank.Grow(days);

        // Assert
        _ = actual.Should().Be(expected);
    }

    private static async Task<IEnumerable<int>> Initialize()
    {
        return (await File.ReadAllTextAsync(@"Data\Day06.txt"))
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .ToList()
            .ConvertAll(Convert.ToInt32);
    }

    private class FishTank
    {
        private readonly long[] _stages = new long[9];

        public FishTank(IEnumerable<int> initialBatch)
        {
            for (int i = 0; i < _stages.Length; i++)
            {
                _stages[i] = initialBatch.Count(x => x == i);
            }
        }

        public long Grow(int days)
        {
            for (int day = 0; day < days; day++)
            {
                long zeroes = _stages[0];
                for (int i = 1; i < _stages.Length; i++)
                {
                    _stages[i - 1] = _stages[i];
                }

                _stages[^3] += zeroes;
                _stages[^1] = zeroes;
            }

            return _stages.Sum();
        }
    }
}