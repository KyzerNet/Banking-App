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
        /// Transfer Other balance from other account into your another.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response Api Transaction Transfer Account Reponse</returns>
        ResponseApi<TransferResponse> TransferOut(TranferRequest request);

        /// <summary>
        /// Transfer Other balance from your account into another.
        /// </summary>
        /// <returns></returns>
        ResponseApi<List<TransferResponse>> TransferHistory();

    }
}
