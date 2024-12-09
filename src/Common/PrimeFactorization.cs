namespace Common;

public static class PrimeFactorization
{
    public static int[] GetPrimeFactors(int n)
    {
        List<int> factors = [];

        if (n == 1)
        {
            factors.Add(n);
        }
        else
        {
            int factor = 2;
            int step = 2;
            while (factor * factor <= n)
            {
                switch (n % factor)
                {
                    case 0:
                    {
                        factors.Add(factor);
                        n /= factor;
                        break;
                    }
                    default:
                    {
                        switch (factor)
                        {
                            case < 3:
                            {
                                factor++;
                                break;
                            }
                            case < 5:
                            {
                                factor += 2;
                                break;
                            }
                            default:
                            {
                                factor += step;
                                step = 6 - step;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            factors.Add(n);
        }

        return factors.ToArray();
    }
}
