using ProjectTesting.TestHelpers;
using Service;
using Service.IService;
using ModelDto.TransactionDto;
using Models.Enums;
using Xunit.Abstractions;
using Xunit.Sdk;
namespace ProjectTesting
{
    /// <summary>
    /// Contains unit tests for deposit and withdrawal transactions.
    /// </summary>
    public class TransactionTest
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ITestOutputHelper _testoutput;

        /// <summary>
        /// Initializes services and test output helper.
        /// </summary>
        public TransactionTest()
        {
            _accountService = new AccountService();
            _transactionService = new TransactionService(_accountService);
            _testoutput = new TestOutputHelper();
        }

        #region Deposit Account Test

        /// <summary>
        /// Tests that depositing with a null request returns an error.
        /// </summary>
        [Fact]
        public void AddDeposit_ShouldReturnError_WhenNull()
        {
            var result = _transactionService.WithdrawAccount(null);
            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }

        /// <summary>
        /// Tests that depositing an amount greater than the allowed limit returns an error.
        /// </summary>
        [Fact]
        public void Widthraw_GreaterThanAmount_ReturnResponse()
        {
            // Arrange: Add a new account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());

            var addAccount = new TransactionRequest
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 100_400, // greater than 100_000
                Type = TransactionType.Deposit,
            };
            // Act: Attempt to deposit
            var transaction = _transactionService.DepositAccount(addAccount);
            AsserApiHelpers.AsserApiError(transaction, "Invalid Deposit", "Cannot Deposit Greater than 100_000");
        }

        /// <summary>
        /// Tests a successful deposit transaction.
        /// </summary>
        [Fact]
        public void Check_DepositAccount_IfWorking()
        {
            // Arrange: Add a new account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());

            var addTransaction = new TransactionRequest()
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 1000,
                Type = TransactionType.Deposit,
            };
            // Act: Deposit amount
            var result = _transactionService.DepositAccount(addTransaction);

            AsserApiHelpers.AsserApiSuccess(result, "Successfully Deposit the desire Amount");
        }
        #endregion

        #region Withdraw Account Test

        /// <summary>
        /// Tests that withdrawing with a null request returns an error.
        /// </summary>
        [Fact]
        public void AddWithdraw_ShouldReturnError_WhenNull()
        {
            var result = _transactionService.WithdrawAccount(null);
            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }

        /// <summary>
        /// Tests that withdrawing more than the current balance returns an error.
        /// </summary>
        [Fact]
        public void WidthrawExceed_Limit()
        {
            // Arrange: Add a new account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());
            // Deposit amount first
            var deposit = new TransactionRequest()
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 50_000,
                Type = TransactionType.Deposit,
            };
            var result = _transactionService.DepositAccount(deposit);

            var withraw = new TransactionRequest
            {
                AccountID = deposit.AccountID,
                Amount = 60_000,
                Type = TransactionType.Withdrawal,
            };
            // Act: Attempt to withdraw more than balance
            var transaction = _transactionService.WithdrawAccount(withraw);
            AsserApiHelpers.AsserApiError(transaction, "Invalid Withrawal", "Cannot withraw amount greater than your current Balance.");
        }

        /// <summary>
        /// Tests a successful withdrawal transaction.
        /// </summary>
        [Fact]
        public void Check_WithdrawAccount_IfWorking()
        {
            // Arrange: Add a new account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());
            // Deposit amount first
            var deposit = new TransactionRequest()
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 50_000,
                Type = TransactionType.Deposit,
            };
            var result = _transactionService.DepositAccount(deposit);

            // Act: Withdraw a valid amount
            var withraw = new TransactionRequest
            {
                AccountID = result.Data.AccountID,
                Amount = 20_000,
                Type = TransactionType.Withdrawal,
            };
            var transaction = _transactionService.WithdrawAccount(withraw);

            AsserApiHelpers.AsserApiSuccess(transaction, "Successfully Widthraw the desire Amount");
        }
        #endregion
    }
}
