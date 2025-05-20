using Service.IService;
using Service;
using Xunit.Abstractions;
using Xunit.Sdk;
using ProjectTesting.TestHelpers;
using ModelDto.TransferResponse;
using ModelDto.TransactionDto;
using Models.Enums;
namespace ProjectTesting
{
    public class TransferTest
    {
        private readonly ITransferService _transfer;
        private readonly IAccountService _accountService;
        private readonly ITestOutputHelper _testoutput;
        private readonly ITransactionService _transaction;
        public TransferTest()
        {
            _accountService = new AccountService();
            _transfer = new TransferService(_accountService);
            _transaction = new TransactionService(_accountService);
            _testoutput = new TestOutputHelper();
        }

        #region Test TransferOut
        // Test to check if TransferIn handles null request properly
        [Fact]
        public void CheckIf_TransferRequest_isNull()
        {
            var transfer_request = _transfer.TransferOut(null);
            AsserApiHelpers.AsserApiError(transfer_request,  "Invalid request: Request body is missing","Null request received");
        }
        // Test to ensure source and destination account numbers are not the same for transfer out
        [Fact]
        public void checkIf_AccountSource_SameAs_AccountDestinationDestinationNumber()
        {
            var addAccount = _accountService.AddAccount(AddedAccount.AddAccount2());

            // Deposit money first
            var deposit_request = new TransactionRequest
            {
                AccountID = addAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };

            var deposit = _transaction.DepositAccount(deposit_request);

            // Attempt to transfer to the same account
            var transfer_request = new TranferRequest
            {
                SourceAccountId = addAccount.Data.AccountNumber,
                DestinationAccountId = addAccount.Data.AccountNumber,
                Amount = 5000,
                Type = TransactionType.TransferOut,
            };
            var transfer = _transfer.TransferOut(transfer_request);

            Assert.Equal(transfer_request.SourceAccountId, transfer_request.DestinationAccountId);
            AsserApiHelpers.AsserApiError(transfer, "Invalid Request", "Source and Destination Account Cannot be the same");
        }
        // Test to check if incorrect transfer type is handled
        [Fact]
        public void Checkif_TrasferType_isIncorrect()
        {
            // Add source and destination accounts
            var sourceAccount = _accountService.AddAccount(AddedAccount.AddAccount2());
            var destinationAccount = _accountService.AddAccount(AddedAccount.AddAccount1());

            // Deposit money to source account
            var deposit_request = new TransactionRequest
            {
                AccountID = sourceAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };
            var desposit = _transaction.DepositAccount(deposit_request);

            // Attempt transfer with incorrect type
            var transfer_request = new TranferRequest
            {
                SourceAccountId = sourceAccount.Data.AccountNumber,
                DestinationAccountId = destinationAccount.Data.AccountNumber,
                Amount = 5000,
                Type = TransactionType.Withdrawal,
            };
            var transfer = _transfer.TransferOut(transfer_request);
            AsserApiHelpers.AsserApiError(transfer, "Invalid Request", "Invalid Transaction Type, Please Try Again");
        }
        // Test to check if transfer out amount exceeding 100,000 is rejected
        [Fact]
        public void checkTrasferOut_AmountExceedLimmit_to100k()
        {
            var addAccount = _accountService.AddAccount(AddedAccount.AddAccount2());
            var addAccount2 = _accountService.AddAccount(AddedAccount.AddAccount());

            // Deposit a large amount first
            var deposit_request = new TransactionRequest
            {
                AccountID = addAccount.Data.AccountNumber,
                Amount = 100_000,
                Type = TransactionType.Deposit,
            };

            var deposit = _transaction.DepositAccount(deposit_request);
            
            // Deposit a large amount first
            var deposit_request2 = new TransactionRequest
            {
                AccountID = addAccount.Data.AccountNumber,
                Amount = 100_000,
                Type = TransactionType.Deposit,
            };

            var deposit2 = _transaction.DepositAccount(deposit_request);

            // Attempt to transfer the maximum allowed amount
            var transfer_request = new TranferRequest
            {
                SourceAccountId = deposit.Data.AccountID,
                DestinationAccountId = addAccount2.Data.AccountNumber,
                Amount = 130_000,
                Type = TransactionType.TransferOut,
            };
            var transfer = _transfer.TransferOut(transfer_request);

            AsserApiHelpers.AsserApiError(transfer, "Invalid Request", "Transferring amount is too high, cannot exceed 100,000 per day");
        }
        [Fact]
         // Test to ensure that when a non-existent Source id is passed, 
        public void GetAccountByID_NonExistentSource_ReturnResponseType()
        {
            //ADD FIRST
            var addDestinationAccount = _accountService.AddAccount(AddedAccount.AddAccount1());

            //DEPOSIT
            var deposit_request = new TransactionRequest
            {
                AccountID = addDestinationAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };
            var transaction = _transaction.DepositAccount(deposit_request);

            var transferAccount = new TranferRequest
            {
                SourceAccountId = "CA3123123123",
                DestinationAccountId = addDestinationAccount.Data.AccountNumber,
                Amount = 5000,
                Type = TransactionType.TransferIn,
            };
            var result = _transfer.TransferOut(transferAccount);
            AsserApiHelpers.AsserApiError(result,"Validation Failed","Source Account Id Cannot be Found");
        }
        [Fact]
         // Test to ensure that when a non-existent destination id is passed, 
        public void GetAccountByID_NonExistentDestination_ReturnResponseType()
        {
             //ADD FIRST
            var addDestinationAccount = _accountService.AddAccount(AddedAccount.AddAccount1());

            //DEPOSIT
            var deposit_request = new TransactionRequest
            {
                AccountID = addDestinationAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };
            var transaction = _transaction.DepositAccount(deposit_request);

            var transferAccount = new TranferRequest
            {
                SourceAccountId = addDestinationAccount.Data.AccountNumber,
                DestinationAccountId = "CA232131233",
                Amount = 5000,
                Type = TransactionType.TransferIn,
            };
            var result = _transfer.TransferOut(transferAccount);
            AsserApiHelpers.AsserApiError(result,"Validation Failed","Source Account Id Cannot be Found");
        }

        // Test to check if a valid transfer out works and returns a success response
        [Fact]
        public void CheckIf_TranferOutWorking_ReturnResponse()
        {
            // Add source and destination accounts
            var sourceAccount = _accountService.AddAccount(AddedAccount.AddAccount2());
            var destinationAccount = _accountService.AddAccount(AddedAccount.AddAccount1());

            // Deposit money to source account
            var deposit_request = new TransactionRequest
            {
                AccountID = sourceAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };
            var desposit = _transaction.DepositAccount(deposit_request);

            // Perform a valid transfer out
            var transfer_request = new TranferRequest
            {
                SourceAccountId = desposit.Data.AccountID,
                DestinationAccountId = destinationAccount.Data.AccountNumber,
                Amount = 5000,
                Type = TransactionType.TransferOut,
            };
            var transfer = _transfer.TransferOut(transfer_request);

            AsserApiHelpers.AsserApiSuccess(transfer, "Successfully Transfering Money to other Account");
        }
        #endregion

       /* #region Test TransferIn
        [Fact]
        public void CheckIf_TransferInRequest_isNull()
        {
            var transfer_request = _transfer.TransferIn(null);
            AsserApiHelpers.AsserApiError(transfer_request, "Null request received", "Invalid request: Request body is missing");
        }
        [Fact]
        public void CheckIf_AccountSource_isNotExist()
        {
            var transfer = _transfer.TransferIn("CA231312312323");
            AsserApiHelpers.AsserApiError(transfer, "Validation Failed", "Source Account Id Cannot be Found");
        }
        [Fact]
        public void CheckIf_TransferInRequest_isWorking()
        {
            // Add source and destination accounts
            var sourceAccount = _accountService.AddAccount(AddedAccount.AddAccount2());
            var destinationAccount = _accountService.AddAccount(AddedAccount.AddAccount1());
            // Deposit money to source account
            var deposit_request = new TransactionRequest
            {
                AccountID = sourceAccount.Data.AccountNumber,
                Amount = 50_000,
                Type = Models.Enums.TransactionType.Deposit,
            };
            var desposit = _transaction.DepositAccount(deposit_request);
            // Perform a valid transfer out
            var transfer_request = new TranferRequest
            {
                SourceAccountId = sourceAccount.Data.AccountNumber,
                DestinationAccountId = destinationAccount.Data.AccountNumber,
                Amount = 5000,
                Type = TransactionType.TransferIn,
            };
            var transfer = _transfer.TransferOut(transfer_request);

            //check If TransferIn is working
            var transferIn = _transfer.TransferIn(transfer_request.SourceAccountId);

            AsserApiHelpers.AsserApiSuccess(transferIn, "Successfully Retrieve ");
        }
        #endregion*/
    }
}
