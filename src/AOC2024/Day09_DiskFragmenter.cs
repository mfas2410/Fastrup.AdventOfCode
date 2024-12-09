namespace AOC2024;

public sealed class Day09_DiskFragmenter
{
    private const string FileName = "Data/Day09.txt";

    [Fact]
    public async Task Test1()
    {
        string diskMapString = await File.ReadAllTextAsync(FileName);
        List<Fragment> fragments = [];
        for (int index = 0; index < diskMapString.Length; index++)
        {
            int block = int.Parse(diskMapString[index].ToString());
            fragments.Add(new Fragment(index % 2 == 0 ? index / 2 : null, block));
        }
        long answer = 0;
        int fragmentIndexStart = 0;
        int fragmentIndexEnd = fragments.Count - 1;
        int newIndex = 0;
        while (true)
        {
            Fragment fragment = fragments[fragmentIndexStart++];
            int elementsToFill = fragment.NewIndex.Count(x => !x.HasValue);
            for (int i = 0; i < elementsToFill; i++)
            {
                if (fragment.NewIndex[i].HasValue) break;
                fragment.NewIndex[i] = newIndex++;
                answer += fragment.NewIndex[i]!.Value * fragment.Id!.Value;
            }
            if (fragmentIndexStart >= fragmentIndexEnd) break;
            int numberOfDefrags = fragments[fragmentIndexStart++].Size;
            while (numberOfDefrags > 0)
            {
                fragment = fragments[fragmentIndexEnd];
                if (!fragment.Id.HasValue)
                {
                    fragmentIndexEnd--;
                    continue;
                }
                for (int i = fragment.NewIndex.Count(x => !x.HasValue) - 1; i >= 0; i--)
                {
                    if (fragment.NewIndex[i].HasValue) break;
                    fragment.NewIndex[i] = newIndex++;
                    answer += fragment.NewIndex[i]!.Value * fragment.Id!.Value;
                    if (i == 0) fragmentIndexEnd--;
                    if (--numberOfDefrags == 0) break;
                }
            }
        }
        answer.Should().Be(6262891638328);
    }

    private sealed record Fragment(int? Id, int Size)
    {
        public int Size { get; } = Size;
        public int?[] NewIndex { get; } = new int?[Size];
    }
}
