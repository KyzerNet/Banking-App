using Models;
using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ModelDto.TransactionDto
{
    /// <summary>
    /// Data Transfer Object (DTO) for transaction requests.
    /// </summary>
    public class TransactionRequest
    {
        [Required(ErrorMessage = "TransactionID is required.")]
        [StringLength(12, ErrorMessage = "TransactionID cannot be longer than 12 characters.")]
        public string TransactionID { get; set; } = string.Empty;

        [Required(ErrorMessage = "AccountID is required.")]
        [StringLength(12, ErrorMessage = "AccountID cannot be longer than 12 characters.")]
        public string AccountID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Transaction type is required.")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type.")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Maps the TransactionRequest to a Transaction model.
        /// </summary>
        /// <returns>Transaction model</returns>
        public Transaction MapTransactionRequest()
        {
            return new Transaction
            {
                TransactionId = this.TransactionID,
                AccountId = this.AccountID,
                Type = this.Type.ToString(),
                Amount = this.Amount,
            };
        }
    }
}
