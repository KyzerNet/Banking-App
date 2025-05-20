namespace Models
{
    /// <summary>
    /// Transfer Model
    /// </summary>
    public class Transfer
    {
        public string SourceAccountId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // type of Transfer
        public string Reference { get; set; }
        public DateTime TransferDate { get; set; }
        public string Status { get; set; }
        public decimal? NewBalance { get; set; }
    }
}
