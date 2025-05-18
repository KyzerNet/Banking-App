using Models;

namespace ModelDto.TransactionDto
{
    /// <summary>
    /// Data Transfer Object (DTO) for transaction responses.
    /// </summary>
    public class TransactionResponse
    {
        public string TransactionID { get; set; }
        public string AccountID { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
    /// <summary>
    /// Extension methods for TransactionResponse.
    /// </summary>
    public static class TransactionResponseExtension
    {
        /// <summary>
        /// Maps the Transaction model to a TransactionResponse DTO.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Transaction Model Value</returns>
        public static TransactionResponse GetTransactionResponse(this Transaction transaction)
        {
            return new TransactionResponse
            {
                TransactionID = transaction.TransactionId,
                AccountID = transaction.AccountId,
                Type = transaction.Type.ToString(),
                Amount = transaction.Amount,
                Timestamp = DateTime.Now,
                Status = transaction.Status.ToString()
            };
        }
    }
    
}
