using System.Text;

namespace HelperContainer
{
    /// <summary>
    /// Helper class to generate random numbers
    /// </summary>
    public class HelperRandomNumber
    {
        readonly static Random _random = new();

        public static string GenerateRandomId()
        {
            var accountId = "CA" + GenerateDigits(10);
            return accountId;
        }
        private static string GenerateDigits(int length)
        {
            var sb = new StringBuilder();

            lock(_random)
            {
                for (int i = 0; i < length; i++)
                {
                    sb.Append(_random.Next(0, 10));
                }
            }
            return sb.ToString();
        }
    }
}
