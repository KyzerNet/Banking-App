using HelperContainer;
using ModelDto.TransferResponse;
using Models.Enums;
using Models;
using Response;
using Service.IService;
using ModelDto;
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

        public ResponseApi<TransferResponse> TransferIn(string? request)
        {
            // Initialize the response object
            ResponseApi<TransferResponse> response = new();

            // Check if the request is null or whitespace
            if (string.IsNullOrWhiteSpace(request))
            {
                response.isSuccess = false;
                response.Message = $"Invalid request: Request body is missing";
                response.Errors.Add("Null request received");
                return response; // Return early if request is invalid
            }

            // Prepare a response object for mapping transfer data
            var mapTransfer = new ResponseApi<TransferResponse>();

            // Attempt to retrieve the account by ID
            var accountExist = _accountService.GetAccountByID(request);

            // If account does not exist or data is null, return failure
            if (accountExist == null || accountExist.Data == null)
            {
                // Defensive: ensure Data is not null before setting Status
                if (mapTransfer.Data == null)
                    mapTransfer.Data = new TransferResponse();

                mapTransfer.Data.Status = Status.Failed.ToString();
                response.isSuccess = false;
                response.Message = "Validation Failed";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }

            // Map account data to transfer response
            if (mapTransfer.Data == null)
                mapTransfer.Data = new TransferResponse();

            mapTransfer.Data.SourceAccountId = accountExist.Data.AccountNumber;
            mapTransfer.Data.DestinationAccountId = accountExist.Data.AccountNumber;
            mapTransfer.Data.Status = Status.Completed.ToString();
            mapTransfer.Data.Amount = (decimal)(accountExist.Data.CurrentBalance ?? 0);
            mapTransfer.Data.Reference = HelperReferenceID.GenerateReferenceID();
            mapTransfer.Data.TransferDate = DateTime.UtcNow;
            mapTransfer.Data.Type = TransactionType.TransferIn.ToString();

            // Mark the operation as successful
            accountExist.isSuccess = true;
            response.isSuccess = true;
            response.Message = "Successfully Transfering Money to your Account";
            response.Data = mapTransfer.Data;
            return response;
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
            if(getAccountSource == null || getAccountSource.Data == null)
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
            if(getAccountDestination == null || getAccountDestination.Data == null)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
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
