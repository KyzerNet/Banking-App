namespace Models
{
    /// <summary>
    /// Transaction Model
    /// </summary>
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}
