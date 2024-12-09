using System.Text;

namespace AOC2015;
public class Day04
{
    [Fact]
    public void Example1()
    {
        // Arrange
        string input = "abcdef609043";

        // Act
        string actual = ComputeHash(input);

        // Assert
        _ = actual.Should().Be("000001dbbfa3a5c83a2d506429c7b00e");
    }

    [Fact]
    public void Task1()
    {
        // Arrange
        string key = "ckczppom";
        int actual = 0;

        // Act
        string hash;
        do
        {
            string input = key + ++actual;
            hash = ComputeHash(input);
        } while (!hash.StartsWith("00000"));

        // Assert
        _ = actual.Should().Be(117946);
    }

    [Fact]
    public void Task2()
    {
        // Arrange
        string key = "ckczppom";
        int actual = 0;

        // Act
        string hash;
        do
        {
            string input = key + ++actual;
            hash = ComputeHash(input);
        } while (!hash.StartsWith("000000"));

        // Assert
        _ = actual.Should().Be(3938038);
    }

    private static string ComputeHash(string input)
    {
        return BitConverter.ToString(MD5.Create().ComputeHash(ASCIIEncoding.ASCII.GetBytes(input))).Replace("-", string.Empty).ToLower();
    }
}
