using Models;

namespace ModelDto.TransferResponse
{
    /// <summary>
    /// Data Transfer Object (DTO) for transfer responses.
    /// </summary>
    public class TransferResponse
    {
        public string SourceAccountId { get; set; }
        public string DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string Type { get; set; }
        public DateTime TransferDate { get; set; }
        public string Status { get; set; }
    }
    /// <summary>
    /// Extension methods for TransferResponse.
    /// </summary>
    public static class TransferResponseExtension
    {
        /// <summary>
        /// Maps the Transfer model to a TransferResponse DTO.
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static TransferResponse GetTransferResponse(this Transfer transfer)
        {
            return new TransferResponse
            {
                SourceAccountId = transfer.SourceAccountId,
                DestinationAccountId = transfer.DestinationAccountId,
                Amount = transfer.Amount,
                Reference = transfer.Reference,
                Type = transfer.Type,
                TransferDate = transfer.TransferDate,
                Status = transfer.Status.ToString()
            };
        }
    }
}
