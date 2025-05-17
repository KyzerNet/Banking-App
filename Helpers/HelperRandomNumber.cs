namespace HelperContainer
{
    public class HelperRandomNumber
    {
        readonly static Random _random = new();
        private static string randomId { get; set; } = string.Empty;

        public static string GenerateRandomId()
        {
            randomId = "CA";

            for (int i = 0; i < 10; i++)
            {
                randomId +=   _random.Next(0, 10).ToString();
            }
            return randomId;
        }
    }
}
