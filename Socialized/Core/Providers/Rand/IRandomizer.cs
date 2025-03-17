namespace Core.Providers.Rand
{
    public interface IRandomizer
    {
        public string CreateHash(int lengthHash);
        public int CreateCode(int lengthCode);
    }
}
