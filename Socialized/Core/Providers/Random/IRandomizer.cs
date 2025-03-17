namespace Core.Providers.Random
{
    public interface IRandomizer
    {
        public string CreateHash(int lengthHash);
        public int CreateCode(int lengthCode);
    }
}
