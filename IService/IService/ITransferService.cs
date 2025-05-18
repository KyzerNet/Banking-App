using ModelDto.TransferResponse;
using Response;

namespace Service.IService
{
    /// <summary>
    /// Transfer Service Interface
    /// </summary>
    public interface ITransferService
    {
         /// <summary>
        /// Transfer your balance from one account to another.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Api Transaction Transfer Account Reponse</returns>
        ResponseApi<TransferResponse> TransferIn(TranferRequest request);
        
        /// <summary>
        /// Transfer Other balance from other account into your another.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Api Transaction Transfer Account Reponse</returns>
        ResponseApi<TransferResponse> TransferOut(TranferRequest request);

    }
}
