namespace AOC2021;

public class Day10_Syntax_Scoring
{
    [Fact]
    public async Task Task1_TotalSyntaxErrorScore()
    {
        // Arrange
        string[] lines = await Initialize();
        int result = 0;

        // Act
        foreach (string line in lines)
        {
            result += new LineParser(line).Corrupt switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => 0
            };
        }

        // Assert
        _ = result.Should().Be(366027);
    }

    [Fact]
    public async Task Task2_AutocompleteAndScore()
    {
        // Arrange
        string[] lines = await Initialize();
        List<long> results = new();

        // Act
        foreach (string line in lines)
        {
            LineParser parser = new(line);
            if (parser.Corrupt.HasValue) continue;

            long score = 0;
            foreach (char c in parser.Ending)
            {
                score = (score * 5) + (c switch
                {
                    ')' => 1,
                    ']' => 2,
                    '}' => 3,
                    '>' => 4,
                    _ => throw new NotImplementedException()
                });
            }

            results.Add(score);
        }

        long result = results.OrderBy(size => size).Skip(results.Count / 2).First();

        // Assert
        _ = result.Should().Be(1118645287);
    }

    private async Task<string[]> Initialize()
    {
        return await File.ReadAllLinesAsync(@"Data\Day10.txt");
    }

    private class LineParser
    {
        private readonly string _input;
        private char? _corrupt;
        private string _ending = string.Empty;
        private bool _isCompleted;
        private bool _isParsed;
        private readonly Stack<char> _stack = new();

        public LineParser(string line)
        {
            _input = line;
        }

        public char? Corrupt
        {
            get
            {
                if (!_isParsed) Parse();
                return _corrupt;
            }
        }

        public string Ending
        {
            get
            {
                if (!_isParsed) Parse();
                if (Corrupt.HasValue) return string.Empty;
                if (!_isCompleted) Complete();
                return _ending;
            }
        }

        private void Parse()
        {
            Dictionary<char, char> chunks = new() { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
            foreach (char c in _input)
            {
                if (!chunks.ContainsKey(c))
                {
                    _stack.Push(c);
                }
                else
                {
                    if (_stack.Peek() != chunks[c])
                    {
                        _corrupt = c;
                        break;
                    }

                    _ = _stack.Pop();
                }
            }

            _isParsed = true;
        }

        private void Complete()
        {
            char[] ending = new char[_stack.Count];
            Dictionary<char, char> chunks = new() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };
            int index = 0;
            while (_stack.TryPop(out char outChar))
            {
                ending[index++] = chunks[outChar];
            }

            _ending = new string(ending);
            _isCompleted = true;
        }
    }
}