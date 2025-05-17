namespace HelperContainer
{
    public class HelperReferenceID
    {
        public static string GenerateReferenceID()
        {
            // Generate a unique reference ID using a GUID
            return Guid.NewGuid().ToString();
        }
    }
}
