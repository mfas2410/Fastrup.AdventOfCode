namespace AOC2022;

public class Day03_Rucksack_Reorganization
{
    [Fact]
    public async Task TotalPriorityForItemInBothRucksackCompartments()
    {
        // Arrange
        string[] input = await Initialize();

        // Act
        int actual = 0;
        foreach (string line in input)
        {
            int length = line.Length;
            char[] rucksack1 = line.ToCharArray()[..(length / 2)];
            char[] rucksack2 = line.ToCharArray()[(length / 2)..];
            char item = rucksack1.Intersect(rucksack2).Single();
            actual += GetPriority(item);
        }

        // Assert
        _ = actual.Should().Be(7553);
    }

    [Fact]
    public async Task TotalPriorityForItemIn3Rucksacks()
    {
        // Arrange
        string[] input = await Initialize();

        // Act
        int actual = 0;
        for (int index = 0; index < input.Length; index += 3)
        {
            char[] rucksack1 = input[index].ToCharArray();
            char[] rucksack2 = input[index + 1].ToCharArray();
            char[] rucksack3 = input[index + 2].ToCharArray();
            char item = rucksack1.Intersect(rucksack2).Intersect(rucksack3).Single();
            actual += GetPriority(item);
        }

        // Assert
        _ = actual.Should().Be(2758);
    }

    private static int GetPriority(char chr)
    {
        return chr - (char.IsUpper(chr) ? 'A' : 'a') + (char.IsUpper(chr) ? 27 : 1);
    }

    private static async Task<string[]> Initialize()
    {
        return await File.ReadAllLinesAsync(@"Data\Day03.txt");
    }
}
