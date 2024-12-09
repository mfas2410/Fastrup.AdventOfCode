namespace Common;

public static class LeastCommonMultiple
{
    public static long GetLeastCommonMultiple(params int[] ns)
    {
        List<int> lcmList = [];
        List<int[]> primeFactors = [];
        primeFactors.AddRange(ns.Select(PrimeFactorization.GetPrimeFactors));
        foreach (int[] primeFactor in primeFactors)
        {
            List<IGrouping<int, int>> existingPrimeFactors = lcmList.GroupBy(x => x).ToList();
            List<IGrouping<int, int>> newPrimeFactors = primeFactor.GroupBy(x => x).ToList();
            foreach (IGrouping<int, int> newPrimeFactor in newPrimeFactors)
            {
                int existing = existingPrimeFactors.Count(x => x.Key == newPrimeFactor.Key);
                lcmList.AddRange(Enumerable.Repeat(newPrimeFactor.Key, newPrimeFactor.Count() - existing));
            }
        }

        return lcmList.ConvertAll(x => (long)x).Aggregate((x, y) => x * y);
    }
}
