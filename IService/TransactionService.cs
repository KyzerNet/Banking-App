using HelperContainer;
using ModelDto.TransactionDto;
using Models;
using Models.Enums;
using Response;
using Service.IService;

namespace Service
{
    public class TransactionService : ITransactionService
    {
        private readonly List<Transaction> _transactions;
        private readonly IAccountService _accountService;
        public TransactionService()
        {
            _accountService = new AccountService();
            _transactions = new List<Transaction>();
        }
        public ResponseApi<TransactionResponse> DepositAccount(TransactionRequest request)
        {
            ResponseApi<TransactionResponse> response = new();
            //check if null
            if (request == null)
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

            //get account id
            var getAccount = _accountService.GetAccountByID(request.TransactionID);

            //check if account exist
            if(getAccount == null || getAccount.Data == null)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            //Map request
            var transaction = request.MapTransactionRequest();
            if (request.Type == TransactionType.Deposit)
            {
                if (request.Amount > 100_000)
                {
                    transaction.Status = Status.Failed.ToString();
                    response.isSuccess = false;
                    response.Message = $"Invalid Deposit";
                    response.Errors.Add("Cannot Deposit Greater than 100_000");
                    return response;
                }
                else
                {                    
                    getAccount.Data.CurrentBalance += request.Amount;
                    transaction.TransactionId = HelperReferenceID.GenerateReferenceID(); //generating random ID
                    transaction.Status = Status.Completed.ToString();
                    transaction.Timestamp = DateTime.UtcNow;

                    response.isSuccess = true;
                    response.Message = $"Successfully Deposit the desire Amount";                   
                    response.Data = transaction.GetTransactionResponse();
                    return response;
                }
            }
            else
            {
                transaction.Status = Status.Pending.ToString();
                response.isSuccess = false;
                response.Message = $"Invalid Transaction Type";
                response.Errors.Add("Transaction Type Not Found");
                return response;
            }



        }

        public ResponseApi<TransactionResponse> WithdrawAccount(TransactionRequest request)
        {
            ResponseApi<TransactionResponse> response = new();
            //check if null
            if (request == null)
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

            //get account id
            var getAccount = _accountService.GetAccountByID(request.TransactionID);

            //check if account exist
            if(getAccount == null || getAccount.Data == null)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            //Map request
            var transaction = request.MapTransactionRequest();
            if (request.Type == TransactionType.Withdrawal)
            {
                if (request.Amount > getAccount.Data.CurrentBalance  )
                {
                    response.isSuccess = false;
                    response.Message = $"Invalid Withrawal";
                    response.Errors.Add("Cannot withraw amount greater than your current Balance.");
                    return response;
                }
                else if (getAccount.Data.CurrentBalance - request.Amount < 100)
                {
                    response.isSuccess = false;
                    response.Message = $"Invalid Withdrawal";
                    response.Errors.Add("You must keep a minimum balance of 100 after withdrawal.");
                    return response;
                }
                else
                {
                    getAccount.Data.CurrentBalance -= request.Amount;
                    transaction.TransactionId = HelperReferenceID.GenerateReferenceID();
                    transaction.Status = Status.Completed.ToString();
                    transaction.Timestamp = DateTime.UtcNow;

                    response.isSuccess = true;
                    response.Message = $"Successfully withdrew the desired amount";
                    response.Data = transaction.GetTransactionResponse();
                    return response;
                }
               
            }
            else
            {
                response.isSuccess = false;
                response.Message = $"Invalid Transaction Type";
                response.Errors.Add("Transaction Type Not Found");
                return response;
            }

        }
        

    }
}
