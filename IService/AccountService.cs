using HelperContainer;
using ModelDto.AccountDto;
using Models;
using Response;
using Service.IService;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly List<Account> _accounts;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        public AccountService()
        {
            _accounts = new List<Account>();
        }

        /// <summary>
        /// Adds a new account based on the provided request.
        /// </summary>
        /// <param name="request">The account request containing account details.</param>
        /// <returns>A response containing the result of the add operation.</returns>
        public ResponseApi<AccountResponse> AddAccount(AccountRequest request)
        {
            ResponseApi<AccountResponse> response = new ResponseApi<AccountResponse>();

            // Validate null request
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
                response.Errors.AddRange(modelValidation);
                return response;
            }

            // Check for duplicate email
            bool accountExist = _accounts.Any(x => x.CostumerEmail == request.AccountEmail);
            if (accountExist)
            {
                response.isSuccess = false;
                response.Message = $"Duplicate Email";
                response.Errors.Add("An Account with this Email  Already Exist");
                return response;
            }

            // Map request to account model and generate account ID
            var account = request.MapAccountRequest();
            account.Id = Guid.NewGuid(); //generating random ID
            account.AccountId = HelperRandomNumber.GenerateRandomId(); //generating random  12 Characters start with CA
            account.CurrentBalance = 0; //setting the account balance as zero 
            account.CreatedAt = DateTime.UtcNow; //setting the account creation date

            // Add account to the list
            _accounts.Add(account);

            response.isSuccess = true;
            response.Message = "Successfully Added your Account";
            response.Data = account.GetAccountResponse();
            return response;
        }

        /// <summary>
        /// Deletes an account by its ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to delete.</param>
        /// <returns>A response containing the result of the delete operation.</returns>
        public ResponseApi<AccountResponse> DeleteAccount(string? accountId)
        {
            ResponseApi<AccountResponse> response = new ResponseApi<AccountResponse>();
            if (string.IsNullOrWhiteSpace(accountId))
            {
                response.isSuccess = false;
                response.Message = "Invalid request: Account ID cannot be null";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }
            // Find account by ID
            var account = _accounts.FirstOrDefault(x => x.AccountId == accountId);
            if (account == null)
            {
                response.isSuccess = false;
                response.Message = "Validation Failed";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }
            // Remove account from the list
            _accounts.Remove(account);
            response.isSuccess = true;
            response.Message = "Successfully Deleted your Account";
            response.Data = account.GetAccountResponse();
            return response;
        }

        /// <summary>
        /// Retrieves an account by its ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve.</param>
        /// <returns>A response containing the account information.</returns>
        public ResponseApi<AccountResponse> GetAccountByID(string? accountId)
        {
            ResponseApi<AccountResponse> response = new ResponseApi<AccountResponse>();

            // Validate account ID
            if (string.IsNullOrWhiteSpace(accountId))
            {
                response.isSuccess = false;
                response.Message = "Invalid request: Account ID cannot be null";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }

            // Find account by ID
            var account = _accounts.FirstOrDefault(x => x.AccountId == accountId);

            if(account == null || string.IsNullOrWhiteSpace(account.AccountId))
            {
                response.isSuccess = false;
                response.Message = "Validation Failed";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }
            response.isSuccess = true;
            response.Message = "Successfully Display Account Information";
            response.Data = account.GetAccountResponse();
            return response;
        }

        /// <summary>
        /// Retrieves a list of all accounts.
        /// </summary>
        /// <returns>A response containing a list of all account responses.</returns>
        public ResponseApi<List<AccountResponse>> GetListAccounts()
        {
            // Map all accounts to response DTOs
            var accountResponse = _accounts.Select(x => x.GetAccountResponse()).ToList();

            if(accountResponse.Count == 0)
            {
                return new ResponseApi<List<AccountResponse>>()
                {
                    isSuccess = true,
                    Message = "No Account Found",
                };
            }
            return new ResponseApi<List<AccountResponse>>
            {
                isSuccess = true,
                Message = "Successfully Display All Accounts",
                Data = accountResponse
            };
        }

        /// <summary>
        /// Updates an account based on the provided request.
        /// </summary>
        /// <param name="request">The account request containing updated account details.</param>
        /// <returns>A response containing the result of the update operation.</returns>
        public ResponseApi<AccountResponse> UpdateAccount(AccountUpdateRequest request, string id)
        {
            ResponseApi<AccountResponse> response = new ResponseApi<AccountResponse>();

            if(request == null)
            {
                response.isSuccess = false;
                response.Message = "Invalid request: Request body is missing";
                response.Errors.Add("Null request received");
                return response;
            }

             // Validate model using helper
            var modelValidation = HelperValidation.ModelValidationResponse(request);
            if (modelValidation.Any())
            {
                response.isSuccess = false;
                response.Message = "Validation Failed";
                response.Errors.AddRange(modelValidation);
                return response;
            }

            // Find account by ID
            var account = _accounts.FirstOrDefault(x => x.AccountId == id);

            if(account == null || string.IsNullOrWhiteSpace(id))
            {
                response.isSuccess = false;
                response.Message = "Validation Failed";
                response.Errors.Add("Account Id Cannot be Found");
                return response;
            }

            // Check for duplicate email
            bool accountExist = _accounts.Any(x => x.CostumerEmail == request.AccountEmail);
            if (accountExist)
            {
                response.isSuccess = false;
                response.Message = $"Duplicate Email";
                response.Errors.Add("An Account with this Email  Already Exist");
                return response;
            }

            account.CostumerName = request.AccountName;
            account.CostumerEmail = request.AccountEmail;
            account.Gender = request.Gender.ToString();
            account.BirthDay = request.BirthDate;

            return new ResponseApi<AccountResponse>
            {
                isSuccess = true,
                Message = "Successfully Updated your Account",
                Data = account.GetAccountResponse()
            };
        }

        public bool UpdateBalance(string accountId, decimal newbalance)
        {
            var account = _accounts.FirstOrDefault(temp => temp.AccountId == accountId);
            if (account == null)
                return false;

            account.CurrentBalance = newbalance;
            return true;
        }
    }
}
