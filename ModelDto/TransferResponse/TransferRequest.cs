using Models.Enums;
using Models;
using System.ComponentModel.DataAnnotations;

namespace ModelDto.TransferResponse
{
    /// <summary>
    /// Data Transfer Object (DTO) for transfer requests.
    /// </summary>
    public class TranferRequest
    {
        [Required(ErrorMessage = "SourceAccountId is required.")]
        [StringLength(12, ErrorMessage = "SourceAccountId cannot be longer than 12 characters.")]
        public string SourceAccountId { get; set; } = string.Empty;

        [Required(ErrorMessage = "DestinationAccountId is required.")]
        [StringLength(12, ErrorMessage = "DestinationAccountId cannot be longer than 12 characters.")]
        public string DestinationAccountId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Transaction type is required.")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type.")]
        public TransactionType Type { get; set; }


        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public Transfer MapTranferRequest()
        {
            return new Transfer
            {
                SourceAccountId = this.SourceAccountId,
                DestinationAccountId = this.DestinationAccountId,
                Type = this.Type.ToString(),
                Amount = this.Amount,
            };
        }
    }
}
