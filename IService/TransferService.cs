using HelperContainer;
using ModelDto.TransferResponse;
using Models.Enums;
using Models;
using Response;
using Service.IService;
using ModelDto;
using ModelDto.TransactionDto;
namespace Service
{
    public class TransferService : ITransferService
    {
        private readonly List<Transfer> _transfer;
        private readonly IAccountService _accountService;
        public TransferService(IAccountService accountService)
        {
            _accountService = accountService;
            _transfer = new List<Transfer>();      
        }

        private ResponseApi<TransactionResponse> TransactionHistory(string accountId,decimal amount, decimal newBalance)
        {
            return new ResponseApi<TransactionResponse>
            {
                isSuccess = true,
                Message = "Transfer History",
                Data = new TransactionResponse
                {
                    TransactionID = HelperReferenceID.GenerateReferenceID(),
                    AccountID = accountId,
                    Amount = amount,
                    Type = TransactionType.TransferOut.ToString(),
                    Status = Status.Completed.ToString(),
                    Timestamp = DateTime.UtcNow,
                }
            };
        }
        /// <summary>
        /// Cash out
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseApi<TransferResponse> TransferOut(TranferRequest request)
        {      
             ResponseApi<TransferResponse> response = new();
            //check if null
            if(request == null)
            {
                response.isSuccess = false;
                response.Message = $"Invalid request: Request body is missing";
                response.Errors.Add("Null request received");
                return response;
            }

            // Validate model using helper
            var modelValidation = HelperValidation.ModelValidationResponse(request);

            if (modelValidation.Any())
            {
                response.isSuccess = false;
                response.Message = "Validation Failed";
                // Filter out nulls and add only non-null error messages
                response.Errors.AddRange(modelValidation.Where(e => !string.IsNullOrEmpty(e))!);
                return response;
            }

            #region check if account source account exist getAccountSource
            //get account from source
            var getAccountSource = _accountService.GetAccountByID(request.SourceAccountId);

            //check if account exist
            if(getAccountSource == null || getAccountSource.Data == null)
            {
                response.isSuccess = false;
                response.Message = $"Validation Failed";
                response.Errors.Add("Source Account Id Cannot be Found");
                return response;
            }
            #endregion


            #region check if account  destination  account exist getAccountDestination
            //get account from destination
            var getAccountDestination = _accountService.GetAccountByID(request.DestinationAccountId);

            //check if account exist
            if(getAccountDestination == null || getAccountDestination.Data == null)
            {
                response.isSuccess = false;
                response.Message = $"Validation Failed";
                response.Errors.Add("Source Account Id Cannot be Found");
                return response;
            }

            #endregion

            #region Check if account is Account Source is same as Account Destination

            if (getAccountSource.Data.AccountNumber.Equals(getAccountDestination.Data.AccountNumber))
            {
                response.isSuccess = false;
                response.Message = "Invalid Request";
                response.Errors.Add("Source and Destination Account Cannot be the same");
                return response;
            }
            #endregion

            var accountTransfer = request.MapTranferRequest();

            //Validate reques
            if (request.Type == TransactionType.TransferOut && 
                getAccountDestination.Data.AccountNumber != getAccountSource.Data.AccountNumber)
            {
                if(request.Amount > getAccountSource.Data.CurrentBalance)
                {
                    response.isSuccess = false;
                    response.Message = $"Request Amount is too high";
                    response.Errors.Add("Invalid Transaction");
                    accountTransfer.Status = Status.Failed.ToString();
                    return response;
                }
                else if(request.Amount > 100_000)
                {
                    response.isSuccess = false;
                    response.Message = $"Invalid Request";
                    response.Errors.Add("Transferring amount is too high, cannot exceed 100,000 per day");
                    accountTransfer.Status = Status.Failed.ToString();
                    return response;
                }
                else
                {
                    //pass the source balance into destination balance 
                    var newBalance = getAccountSource.Data.CurrentBalance += request.Amount;
                    
                    var NewBalance = _accountService.UpdateBalance(getAccountSource.Data.AccountNumber, (decimal)newBalance);

                    var transactionHistory = TransactionHistory(getAccountDestination.Data.AccountNumber, request.Amount,(decimal)newBalance);

                    response.isSuccess = true;
                    response.Message = $"Successfully Transfering Money to other Account";
                    response.Data = accountTransfer.GetTransferResponse();
                    return response;
                }
            }
            else
            {
                response.isSuccess = false;
                response.Message = $"Invalid Request";
                response.Errors.Add("Invalid Transaction Type, Please Try Again");
                return response;
            }
        }
    }
}
