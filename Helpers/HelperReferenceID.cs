namespace HelperContainer
{
    public class HelperReferenceID
    {
        /// <summary>
        /// Generates a unique reference ID for transactions.
        /// </summary>
        /// <returns></returns>
        public static string GenerateReferenceID()
        {
            // Format: S009-20250522-ABCDE12345
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var guidPart = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(); // Shorten for readability
            return $"S009-{datePart}-{guidPart}";
        }
    }
}
