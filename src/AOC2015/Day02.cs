namespace AOC2015;
public class Day02
{
    [Fact]
    public void Examples1()
    {
        // Arrange
        Present present1 = new(2, 3, 4);
        Present present2 = new(1, 1, 10);

        // Act

        // Assert
        _ = present1.GiftWrapSize.Should().Be(58);
        _ = present2.GiftWrapSize.Should().Be(43);
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        IReadOnlyCollection<Present> presents = await Initialize();

        // Act
        int giftWrapSize = presents.Sum(x => x.GiftWrapSize);

        // Assert
        _ = giftWrapSize.Should().Be(1598415);
    }

    [Fact]
    public void Examples2()
    {
        // Arrange
        Present present1 = new(2, 3, 4);
        Present present2 = new(1, 1, 10);

        // Act

        // Assert
        _ = present1.RibbonLength.Should().Be(34);
        _ = present2.RibbonLength.Should().Be(14);
    }

    [Fact]
    public async Task Task2()
    {
        // Arrange
        IReadOnlyCollection<Present> presents = await Initialize();

        // Act
        int ribbonLength = presents.Sum(x => x.RibbonLength);

        // Assert
        _ = ribbonLength.Should().Be(3812909);
    }

    private static async Task<IReadOnlyCollection<Present>> Initialize()
    {
        List<Present> presents = new();
        string[] lines = await File.ReadAllLinesAsync(@"Data\Day02.txt");
        foreach (string line in lines)
        {
            string[] dimensions = line.Split('x');
            presents.Add(new Present(int.Parse(dimensions[0]), int.Parse(dimensions[1]), int.Parse(dimensions[2])));
        }

        return presents;
    }

    private record Present(int Length, int Width, int Height)
    {
        public int GiftWrapSize
            => (2 * Length * Width) + (2 * Width * Height) + (2 * Height * Length) + new[] { End, Side, Top }.Min();

        public int RibbonLength
        {
            get
            {
                int[] minimumSides = new[] { Length, Width, Height }.OrderBy(x => x).Take(2).ToArray();
                return minimumSides[0] + minimumSides[0] + minimumSides[1] + minimumSides[1] + (Length * Width * Height);
            }
        }

        private int End => Width * Height;

        private int Side => Length * Height;

        private int Top => Length * Width;
    }
}
