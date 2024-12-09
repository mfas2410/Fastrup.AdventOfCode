namespace AOC2020;

public class Day04
{
    private readonly ITestOutputHelper _output;

    public Day04(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Task1()
    {
        // Arrange
        string[] requiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
        string[] data = await File.ReadAllLinesAsync(@"Data\Day04.txt");
        List<Dictionary<string, string>> passports = new();

        // Act
        Dictionary<string, string> currentPassport = new();
        foreach (string line in data)
        {
            if (string.IsNullOrEmpty(line))
            {
                passports.Add(currentPassport);
                currentPassport = new Dictionary<string, string>();
            }
            else
            {
                string[] parts = line.Split(' ');
                foreach (string part in parts)
                {
                    string[] passportEntry = part.Split(':');
                    currentPassport.Add(passportEntry[0], passportEntry[1]);
                }
            }
        }

        passports.Add(currentPassport);

        int numberOfValidPassports = 0;
        int numberOfInvalidPassports = 0;
        foreach (Dictionary<string, string> passport in passports)
        {
            bool isValid = true;
            foreach (string key in requiredFields)
            {
                if (!passport.ContainsKey(key))
                {
                    _output.WriteLine($"Invalid passport (missing {key}): {string.Join(':', passport.Keys)}");
                    numberOfInvalidPassports++;
                    isValid = false;
                    break;
                }
            }

            if (isValid)
                numberOfValidPassports++;
        }

        // Assert
        _output.WriteLine($"{numberOfValidPassports} + {numberOfInvalidPassports} / {passports.Count}");
        _ = numberOfValidPassports.Should().Be(216);
    }

    [Fact]
    public void Task2()
    {
        // Arrange

        // Act

        // Assert
    }
}