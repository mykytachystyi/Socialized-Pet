namespace Core.Providers.Rand;

public class Randomizer : IRandomizer
{
    private Random random = new Random();
    private readonly string Alphavite = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public string CreateHash(int lengthHash)
    {
        string hash = "";
        for (int i = 0; i < lengthHash; i++)
        {
            hash += Alphavite[random.Next(Alphavite.Length)];
        }
        return hash;
    }
    public int CreateCode(int lengthCode)
    {
        int minValue = 0, maxValue = 0;

        for (int i = 0; i < lengthCode; i++)
        {
            maxValue += (int)(9 * Math.Pow(10, i));
        }
        minValue += (int)Math.Pow(10, lengthCode - 1);
        return random.Next(minValue, maxValue);
    }
}
