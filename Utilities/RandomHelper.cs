using System.Text;

namespace MarketPlace.Utilities
{
    public static class RandomHelper
    {
        public static string CreateRandomDigits(Random rnd, int length)
        {
            string charPool = "1234567890";
            StringBuilder rs = new StringBuilder();
            while (length > 0)
            {
                rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);
                length--;
            }
            return rs.ToString();
        }
    }
}
