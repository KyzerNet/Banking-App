using ModelDto.AccountDto;
using Response;

namespace Service.IService
{
    public interface IAccountService
    {
        /// <summary>
        /// Add account for user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Accountreponse Api</returns>
        ResponseApi<AccountResponse> AddAccount(AccountRequest request);

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns>List  of Account and Response </returns>
        ResponseApi<List<AccountResponse>> GetListAccounts();

        /// <summary>
        /// Get account by ID 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>string ID</returns>
        ResponseApi<AccountResponse> GetAccountByID(string? accountId);

        /// <summary>
        /// Updates an existing account with the provided information.
        /// </summary>
        /// <param name="request">The account update request containing updated account details.</param>
        /// <returns>A response containing the updated account information.</returns>
        ResponseApi<AccountResponse> UpdateAccount(AccountUpdateRequest request, string id);

        /// <summary>
        /// Deletes an account by its unique identifier.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account to delete.</param>
        /// <returns>A response containing the deleted account information.</returns>
        ResponseApi<AccountResponse> DeleteAccount(string? accountId);

         /// <summary>
        /// Update Account Balance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool UpdateBalance(string accountId, decimal amount);
    }
}
