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
            bool accountExist = _transactions.Find(x => x.AccountId == getAccount.Data.AccountNumber) != null;

            if (!accountExist)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            if (request.Type == TransactionType.Deposit)
            {
                if (request.Amount > getAccount.Data.CurrentBalance)
                {
                    response.isSuccess = false;
                    response.Message = $"Request Amount is too high";
                    response.Errors.Add("Account Not Found");
                    return response;
                }
                else
                {
                    var transaction = request.MapTransactionRequest();
                    getAccount.Data.CurrentBalance += request.Amount;
                    response.isSuccess = true;
                    response.Message = $"Successfully Deposit the desire Amount";
                    transaction.Status = Status.Completed.ToString();
                    transaction.Timestamp = DateTime.UtcNow;

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
            bool accountExist = _transactions.Find(x => x.AccountId == getAccount.Data.AccountNumber) != null;

            if (!accountExist)
            {
                response.isSuccess = false;
                response.Message = $"Account not found";
                response.Errors.Add("Account Not Found");
                return response;
            }

            if (request.Type == TransactionType.Deposit)
            {
                if (request.Amount > getAccount.Data.CurrentBalance)
                {
                    response.isSuccess = false;
                    response.Message = $"Request Amount is too high";
                    response.Errors.Add("Account Not Found");
                    return response;
                }
                else
                {
                    getAccount.Data.CurrentBalance -= request.Amount;
                    response.isSuccess = true;
                    response.Message = $"Successfully Deposit the desire Amount";
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
