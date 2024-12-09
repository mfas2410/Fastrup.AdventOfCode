namespace Common;

public static class ArrayExtensions
{
    public static int CountWords(this string[] lines, string[] words)
    {
        int count = 0;
        Dictionary<char, string> characters = words.ToDictionary(x => x[0]);
        int minLength = words.Min(x => x.Length);
        foreach (string line in lines)
        {
            ReadOnlySpan<char> spanLine = line.AsSpan();
            for (int i = 0; i < spanLine.Length - minLength + 1; i++)
            {
                if (characters.TryGetValue(spanLine[i], out string? word) &&
                    i + word.Length <= spanLine.Length &&
                    spanLine.Slice(i, word.Length).SequenceEqual(word.AsSpan()))
                {
                    count++;
                }
            }
        }
        return count;
    }

    public static string[] Rotate45Degrees(this string[] lines)
    {
        int n = lines.Length;
        int m = lines.Max(x => x.Length);
        int newSize = n + m - 1;
        string[] rotated = new string[newSize];
        for (int i = 0; i < newSize; i++)
        {
            StringBuilder newLine = new();
            for (int j = 0; j < n; j++)
            {
                int k = i - j;
                if (k >= 0 && k < m) newLine.Append(lines[j][k]);
            }
            rotated[i] = newLine.ToString();
        }
        return rotated;
    }

    public static string[] Rotate90Degrees(this string[] lines)
    {
        int n = lines.Length;
        int m = lines.Max(x => x.Length);
        string[] rotated = new string[m];
        for (int i = 0; i < m; i++)
        {
            char[] newLine = new char[n];
            for (int j = 0; j < n; j++)
            {
                newLine[j] = lines[n - j - 1][i];
            }
            rotated[i] = new string(newLine);
        }
        return rotated;
    }
}
