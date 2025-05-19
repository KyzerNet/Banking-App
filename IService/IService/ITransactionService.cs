using ModelDto.TransactionDto;
using Response;

namespace Service.IService
{
    /// <summary>
    /// Transaction Service Interface
    /// </summary>
    public interface ITransactionService
    {
         /// <summary>
        /// Deposit Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Api Transaction Deposit Account Reponse</returns>
        ResponseApi<TransactionResponse> DepositAccount(TransactionRequest request);

        /// <summary>
        /// Withdraw Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Api Transaction Withdraw Account Reponse</returns>
        ResponseApi<TransactionResponse> WithdrawAccount(TransactionRequest request);

       
    }
}
