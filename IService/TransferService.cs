using HelperContainer;
using ModelDto.TransferResponse;
using Models.Enums;
using Models;
using Response;
using Service.IService;

namespace Service
{
    public class TransferService : ITransferService
    {
        private readonly List<Transfer> _transfer;
        private readonly IAccountService _accountService;
        public TransferService()
        {
            _accountService = new AccountService();
            _transfer = new List<Transfer>();      
        }
        /// <summary>
        /// Cash in 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseApi<TransferResponse> TransferIn(TranferRequest request)
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

            #region check if account source account exist
            //get account from source
            var getAccountSource = _accountService.GetAccountByID(request.SourceAccountId);

            //check if account exist
            bool accountPassingSource = _transfer.FirstOrDefault(x => x.SourceAccountId == getAccountSource.Data.AccountNumber) != null;

            if (accountPassingSource)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }
            #endregion


            #region check if account  destination  account exist
            //get account from destination
            var getAccountDestination = _accountService.GetAccountByID(request.DestinationAccountId);

            //check if account exist
            bool accountPassingDestination = _transfer.FirstOrDefault(x => x.SourceAccountId == getAccountDestination.Data.AccountNumber) != null;

            if (accountPassingDestination)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            #endregion

            var accountTransfer = new Transfer();

            if (request.Type == TransactionType.TransferOut)
            {
                if(request.Amount > getAccountSource.Data.CurrentBalance)
                {
                    response.isSuccess = false;
                    response.Message = $"Request Amount is too high";
                    response.Errors.Add("Invalid Transaction");
                    accountTransfer.Status = Status.Failed.ToString();
                    return response;
                }
                else
                {
                    //pass the source balance into destination balance 
                    getAccountDestination.Data.CurrentBalance = getAccountSource.Data.CurrentBalance -= request.Amount;

                    //Map the request to the transfer model
                    accountTransfer.TransferDate = DateTime.UtcNow;
                    accountTransfer.Status = Status.Completed.ToString();
                    accountTransfer.Reference = HelperReferenceID.GenerateReferenceID();




                    response.isSuccess = true;
                    response.Message = $"Successfully Transfering Money to other Account";
                    response.Data = accountTransfer.GetTransferResponse();
                    return response;
                }
            }
            else
            {
                response.isSuccess = false;
                response.Message = $"Invalid Transaction Type";
                response.Errors.Add("Invalid Request, Please Try Again");
                return response;
            }
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

            #region check if account source account exist
            //get account from source
            var getAccountSource = _accountService.GetAccountByID(request.SourceAccountId);

            //check if account exist
            bool accountPassingSource = _transfer.FirstOrDefault(x => x.SourceAccountId == getAccountSource.Data.AccountNumber) != null;

            if (accountPassingSource)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }
            #endregion


            #region check if account  destination  account exist
            //get account from destination
            var getAccountDestination = _accountService.GetAccountByID(request.DestinationAccountId);

            //check if account exist
            bool accountPassingDestination = _transfer.FirstOrDefault(x => x.SourceAccountId == getAccountDestination.Data.AccountNumber) != null;

            if (accountPassingDestination)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            #endregion

            var accountTransfer = new Transfer();

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
                else
                {
                    //pass the source balance into destination balance 
                    getAccountDestination.Data.CurrentBalance = getAccountSource.Data.CurrentBalance += request.Amount;

                    //Map the request to the transfer model
                    accountTransfer.TransferDate = DateTime.UtcNow;
                    accountTransfer.Status = Status.Completed.ToString();
                    accountTransfer.Reference = HelperReferenceID.GenerateReferenceID();




                    response.isSuccess = true;
                    response.Message = $"Successfully Transfering Money to other Account";
                    response.Data = accountTransfer.GetTransferResponse();
                    return response;
                }
            }
            else
            {
                response.isSuccess = false;
                response.Message = $"Invalid Transaction Type";
                response.Errors.Add("Invalid Request, Please Try Again");
                return response;
            }
        }
    }
}
