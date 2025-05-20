using ProjectTesting.TestHelpers;
using Service;
using Service.IService;
using ModelDto.TransactionDto;
using Models.Enums;
using Xunit.Abstractions;
using Xunit.Sdk;
namespace ProjectTesting
{
    public class TransactionTest
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly ITestOutputHelper _testoutput;
        public TransactionTest()
        {
            _accountService = new AccountService();
            _transactionService = new TransactionService(_accountService);
            _testoutput = new TestOutputHelper();
        }
        #region Deposit Account Test
        [Fact]
        public void AddDeposit_ShouldReturnError_WhenNull()
        {
            var result = _transactionService.WithdrawAccount(null);
            
            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }
        [Fact]
        public void Widthraw_GreaterThanAmount_ReturnResponse()
        {
            //arrange
             //Arrange
            // add first Account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());

            var addAccount = new TransactionRequest
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 100_400, //greater than 100_000
                Type = TransactionType.Deposit,
            };
            //Act
            var transaction = _transactionService.DepositAccount(addAccount);
            AsserApiHelpers.AsserApiError(transaction, "Invalid Deposit", "Cannot Deposit Greater than 100_000");
        }
        [Fact]
        public void Check_DepositAccount_IfWorking()
        {
            //Arrange
            // add first Account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());
           
            var addTransaction = new TransactionRequest()
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 1000,
                Type = TransactionType.Deposit,
            };
            var result = _transactionService.DepositAccount(addTransaction);

            AsserApiHelpers.AsserApiSuccess(result, "Successfully Deposit the desire Amount");
        }
        #endregion

        #region Withdraw Account Test
         [Fact]
        public void AddWithdraw_ShouldReturnError_WhenNull()
        {
            var result = _transactionService.WithdrawAccount(null);
            
            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }
        [Fact]
        public void WidthrawExceed_Limit()
        {    
           //Arrange
            // add first Account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());
           //deposit amount first
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
            //Act
            var transaction = _transactionService.WithdrawAccount(withraw);
            AsserApiHelpers.AsserApiError(transaction, "Invalid Withrawal", "Cannot withraw amount greater than your current Balance.");
        }
        [Fact]
        public void Check_WithdrawAccount_IfWorking()
        {
            //Arrange
            // add first Account
            var addAccountResult = _accountService.AddAccount(AddedAccount.AddAccount());
           //deposit amount first
            var deposit = new TransactionRequest()
            {
                AccountID = addAccountResult.Data.AccountNumber,
                Amount = 50_000,
                Type = TransactionType.Deposit,
            };
            var result = _transactionService.DepositAccount(deposit);
         
            //Act
            var withraw = new TransactionRequest
            {
                AccountID = result.Data.AccountID,
                Amount = 20_000,
                Type = TransactionType.Withdrawal,
            };
            var transaction = _transactionService.WithdrawAccount(withraw);

            AsserApiHelpers.AsserApiSuccess(transaction, "Successfully withdrew the desired amount" );
        }
        #endregion
    }
}
