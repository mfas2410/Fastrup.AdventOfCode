namespace AOC2023;

public sealed class Day07_CamelCards
{
    private const string FileName = "Data/Day07.txt";

    [Fact]
    public async Task Test1()
    {
        long answer = 0;

        SortedSet<Hand> hands = new(new HandComparer());

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            string[] strings = line.Split(' ');
            hands.Add(new Hand(strings[0], int.Parse(strings[1])));
        }

        answer = hands.Select((hand, index) => hand.Bid * (index + 1)).Aggregate((x, y) => x + y);
        answer.Should().Be(253603890);
    }

    [Fact]
    public async Task Test2()
    {
        long answer = 0;

        SortedSet<Hand> hands = new(new HandJokerComparer());

        await foreach (string line in File.ReadLinesAsync(FileName))
        {
            string[] strings = line.Split(' ');
            hands.Add(new Hand(strings[0], int.Parse(strings[1])));
        }

        answer = hands.Select((hand, index) => hand.Bid * (index + 1)).Aggregate((x, y) => x + y);
        answer.Should().Be(253630098);
    }

    private sealed record Hand
    {
        public enum Strengths
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        private readonly Dictionary<char, int> _jokerValues = new() { { 'J', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'T', 10 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 } };
        private readonly Dictionary<char, int> _values = new() { { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 }, { 'T', 10 }, { 'J', 11 }, { 'Q', 12 }, { 'K', 13 }, { 'A', 14 } };

        public Hand(string Cards, int Bid)
        {
            this.Cards = Cards;
            this.Bid = Bid;

            ImmutableArray<IGrouping<char, char>> groups = Cards.GroupBy(x => x).OrderByDescending(x => x.Count()).ThenByDescending(x => _values[x.Key]).ToImmutableArray();
            Strength = GetStrength(groups);

            if (Cards.Contains('J'))
            {
                string cards = Cards.Equals("JJJJJ") ? "AAAAA" : Cards.Replace('J', groups.First(y => y.Key != 'J').Key);
                JokerStrength = GetStrength(cards.GroupBy(x => x).OrderByDescending(x => x.Count()).ThenByDescending(x => _values[x.Key]).ToImmutableArray());
            }
            else
            {
                JokerStrength = Strength;
            }

            CardValues = Cards.Select(x => _values[x]).ToImmutableArray();
            JokerCardValues = Cards.Select(x => _jokerValues[x]).ToImmutableArray();

            static Strengths GetStrength(ImmutableArray<IGrouping<char, char>> groups) =>
                groups.Length switch
                {
                    1 => Strengths.FiveOfAKind,
                    2 when groups.Any(x => x.Count() == 4) => Strengths.FourOfAKind,
                    2 when groups.Any(x => x.Count() == 3) => Strengths.FullHouse,
                    3 when groups.Count(x => x.Count() == 2) == 2 => Strengths.TwoPair,
                    _ => groups.Any(x => x.Count() == 3)
                        ? Strengths.ThreeOfAKind
                        : groups.Any(x => x.Count() == 2)
                            ? Strengths.OnePair
                            : Strengths.HighCard
                };
        }

        public Strengths Strength { get; }

        public ImmutableArray<int> CardValues { get; }

        public Strengths JokerStrength { get; }

        public ImmutableArray<int> JokerCardValues { get; }

        public string Cards { get; }

        public int Bid { get; }

        public override string ToString() =>
            $"{Cards} ({string.Join(',', CardValues.Select(x => x.ToString("00")))} {Strength} {Bid}";
    }

    private sealed class HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? hand1, Hand? hand2)
        {
            if (hand1!.Strength > hand2!.Strength) return 1;
            if (hand1.Strength < hand2.Strength) return -1;
            for (int index = 0; index < hand1.CardValues.Length; index++)
            {
                int hand1Value = hand1.CardValues[index];
                int hand2Value = hand2.CardValues[index];
                if (hand1Value > hand2Value) return 1;
                if (hand1Value < hand2Value) return -1;
            }

            throw new Exception("Hands are equal");
        }
    }

    private sealed class HandJokerComparer : IComparer<Hand>
    {
        public int Compare(Hand? hand1, Hand? hand2)
        {
            if (hand1!.JokerStrength > hand2!.JokerStrength) return 1;
            if (hand1.JokerStrength < hand2.JokerStrength) return -1;
            for (int index = 0; index < hand1.JokerCardValues.Length; index++)
            {
                int hand1Value = hand1.JokerCardValues[index];
                int hand2Value = hand2.JokerCardValues[index];
                if (hand1Value > hand2Value) return 1;
                if (hand1Value < hand2Value) return -1;
            }

            throw new Exception("Hands are equal");
        }
    }
}
