namespace AOC2024;

public sealed class Day09_DiskFragmenter
{
    private const string FileName = "Data/Day09.txt";

    [Fact]
    public async Task Test1()
    {
        string diskMapString = await File.ReadAllTextAsync(FileName);
        List<Fragment> fragments = [];
        int fragmentIndexStart = 0;
        for (int index = 0; index < diskMapString.Length; index++)
        {
            int block = int.Parse(diskMapString[index].ToString());
            fragments.Add(new Fragment(index % 2 == 0 ? index / 2 : null, block, fragmentIndexStart));
            fragmentIndexStart += block;
        }
        long answer = 0;
        fragmentIndexStart = 0;
        int fragmentIndexEnd = fragments.Count - 1;
        int newIndex = 0;
        while (true)
        {
            Fragment fragment = fragments[fragmentIndexStart++];
            int elementsToFill = fragment.NewIndex.Count(x => !x.HasValue);
            for (int index = 0; index < elementsToFill; index++)
            {
                if (fragment.NewIndex[index].HasValue) break;
                fragment.NewIndex[index] = newIndex++;
                answer += fragment.NewIndex[index]!.Value * fragment.Id!.Value;
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
                for (int index = fragment.NewIndex.Count(x => !x.HasValue) - 1; index >= 0; index--)
                {
                    if (fragment.NewIndex[index].HasValue) break;
                    fragment.NewIndex[index] = newIndex++;
                    answer += fragment.NewIndex[index]!.Value * fragment.Id!.Value;
                    if (index == 0) fragmentIndexEnd--;
                    if (--numberOfDefrags == 0) break;
                }
            }
        }
        answer.Should().Be(6262891638328);
    }

    [Fact]
    public async Task Test2()
    {
        string diskMapString = await File.ReadAllTextAsync(FileName);
        List<Fragment> fragments = [];
        int fragmentIndexStart = 0;
        for (int index = 0; index < diskMapString.Length; index++)
        {
            int block = int.Parse(diskMapString[index].ToString());
            fragments.Add(new Fragment(index % 2 == 0 ? index / 2 : null, block, fragmentIndexStart));
            fragmentIndexStart += block;
        }
        List<Fragment> dataFragments = fragments.Where(x => x.Id is not null).ToList();
        List<Fragment> emptyFragments = fragments.Where(x => x.Id is null).ToList();
        for (int index = dataFragments.Count - 1; index >= 0; index--)
        {
            Fragment currentFragment = dataFragments[index];
            Fragment? emptyFragment = emptyFragments.FirstOrDefault(emptyFragment => emptyFragment.Free >= currentFragment.Size && emptyFragment.Index < currentFragment.Index);
            if (emptyFragment is null)
            {
                for (int counter = 0; counter < currentFragment.Size; counter++)
                {
                    currentFragment.NewIndex[counter] = currentFragment.Index + counter;
                }
            }
            else
            {
                for (int counter = 0; counter < currentFragment.Size; counter++)
                {
                    currentFragment.NewIndex[counter] = emptyFragment.Index + emptyFragment.Size - emptyFragment.Free;
                    emptyFragment.NewIndex[^emptyFragment.Free] = currentFragment.Id;
                }
            }
        }
        long answer = 0;
        foreach (Fragment fragment in dataFragments)
        {
            for (int index = 0; index < fragment.Size; index++)
            {
                answer += fragment.NewIndex[index]!.Value * fragment.Id!.Value;
            }
        }
        answer.Should().Be(6287317016845);
    }

    private sealed class Fragment(int? id, int size, int index)
    {
        public int? Id { get; } = id;
        public int Size { get; } = size;
        public int Index { get; } = index;
        public int?[] NewIndex { get; } = new int?[size];
        public int Free => NewIndex.Count(x => !x.HasValue);
    }
}
