using ModelDto.AccountDto;
using Models.Enums;
using ProjectTesting.TestHelpers;
using Service.IService;
using Xunit.Abstractions;
using Service;
namespace ProjectTesting
{
    public class AccountTest
    {
        private readonly IAccountService _account;
        private readonly ITestOutputHelper _testOutput;
        public AccountTest(ITestOutputHelper testOutput)
        {
            _account = new AccountService();
            _testOutput = testOutput;
        }
        #region
        [Fact]
        public void AddAccount_ShouldReturnError_WhenNull()
        {
            var result = _account.AddAccount(null);

            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }
        [Fact]
        public void AddAccount_EmptyAccountName_CheckWhenEmpty()
        {
            var addAccount = new AccountRequest()
            {
                AccountName = "",
                AccountEmail = "dsadas@gmail.com",
                BirthDate = DateTime.Parse("2022-11-11"),
                Gender = GenderOptions.Male
            };
            var added_account = _account.AddAccount(addAccount);

            AsserApiHelpers.AsserApiError(added_account, "Validation Failed", "Account name is required.");
        }
        [Fact]
        public void AddAccount_EmptyAccountEmail_CheckWhenEmpty()
        {
            var addAccount = new AccountRequest()
            {
                AccountName = "sadasdasd",
                AccountEmail = "",
                BirthDate = DateTime.Parse("2022-11-11"),
                Gender = GenderOptions.Male
            };
            var added_account = _account.AddAccount(addAccount);

            AsserApiHelpers.AsserApiError(added_account, "Validation Failed", "Account email is required.");
        }
        [Fact]
        public void CheckIf_ID_Generating()
        {
            // Arrange
            var addAccount = AddedAccount.AddAccount();

            //act
            var added_account = _account.AddAccount(addAccount);

            //assert
            Assert.NotEmpty(added_account.Data.AccountNumber);
            Assert.StartsWith("CA", added_account.Data.AccountNumber);
            _testOutput.WriteLine(added_account.Data.ToString());
        }
        [Fact]
        public void ValidateEmail_WhenIs_Duplicated()
        {
            var addAccount = new AccountRequest()
            {
                AccountName = "sadasdassssd",
                AccountEmail = "maria@gmail.com",
                BirthDate = DateTime.Parse("2022-11-11"),
                Gender = GenderOptions.Male
            };
            var addAccount2 = new AccountRequest()
            {
                AccountName = "sadasdasssd",
                AccountEmail = "maria@gmail.com",
                BirthDate = DateTime.Parse("2022-11-11"),
                Gender = GenderOptions.Male
            };
            var added_account = _account.AddAccount(addAccount);
            var added_account2 = _account.AddAccount(addAccount2);

            AsserApiHelpers.AsserApiError(added_account2, "Duplicate Email", "An Account with this Email  Already Exist");
}
        [Fact]
        public void Add_ValidAccount_ReturnAssertResult()
        {
            var addAccount = new AccountRequest()
            {
                AccountName = "sadasdasd",
                AccountEmail = "sadasda@gmail.com",
                BirthDate = DateTime.Parse("2022-11-11"),
                Gender = GenderOptions.Male
            };
            var added_account = _account.AddAccount(addAccount);

            AsserApiHelpers.AsserApiSuccess(added_account, "Successfully Added your Account");
        }

        #endregion

        #region List of Account Test
        [Fact]
        public void ListPersons_AccountsAdded_ReturnsAllAccounts()
        {
            var addedAccount = AddedAccount.AddAccount();

            var add = _account.AddAccount(addedAccount);

            var result = _account.GetListAccounts();

            AsserApiHelpers.AsserApiSuccess(result, "Successfully Display All Accounts");
        }
        [Fact]
        public void Check_GetAllAccount_IfWorking()
        {
            // Arrange
            var addedAccount1 = AddedAccount.AddAccount1();
            var addedAccount2 = AddedAccount.AddAccount2();
            var addedAccount3 = AddedAccount.AddAccount();

            _account.AddAccount(addedAccount1);
            _account.AddAccount(addedAccount2);
            _account.AddAccount(addedAccount3);

            // Act
            var actualResponse = _account.GetListAccounts();

            // Assert
            AsserApiHelpers.AsserApiSuccess(actualResponse, "Successfully Display All Accounts");

            Assert.NotNull(actualResponse.Data);
            Assert.Equal(3, actualResponse.Data.Count);

            _testOutput.WriteLine("Actual Values:");
            foreach (var account in actualResponse.Data)
            {
                _testOutput.WriteLine(account.ToString());
            }
        }
        #endregion

        #region Get Account ID test
        [Fact]
        public void GetAccountByID_NullId_ReturnResponseType()
        {
            var account = _account.GetAccountByID(null);

            AsserApiHelpers.AsserApiError(account,"Invalid request: Account ID cannot be null","Account Id Cannot be Found");

        }
         // Test to ensure that when a non-existent ID is passed, 
        [Fact]
        public void GetAccountByID_NonExistentId_ReturnResponseType()
        {
            // Arrange
            string nonExistentId = "CA2313515533";
            var account = _account.GetAccountByID(nonExistentId);
            AsserApiHelpers.AsserApiError(account,"Validation Failed","Account Id Cannot be Found");
        }
         // Test to verify that a valid ID returns the correct PersonResponseDto
        [Fact]
        public void GetAccountByID_ValidId_ReturnsResponseType()
        {
            var add_new_Account = AddedAccount.AddAccount();
            Assert.NotNull(add_new_Account); // Ensure the account request is not null

            var addingAccount = _account.AddAccount(add_new_Account);
            Assert.NotNull(addingAccount); // Ensure the response is not null
            Assert.True(addingAccount.isSuccess); // Ensure the account was added successfully
            Assert.NotNull(addingAccount.Data); // Ensure the response contains data
            Assert.NotEqual("", addingAccount.Data.AccountNumber); // Ensure the ID is valid

            var getID = _account.GetAccountByID(addingAccount.Data.AccountNumber);
            Assert.NotNull(getID); // Ensure the response is not null
            Assert.True(getID.isSuccess); // Ensure the account was retrieved successfully

            AsserApiHelpers.AsserApiSuccess(getID, "Successfully Display Account Information");
        }

        #endregion

        #region Update Account
        [Fact]
        public void UpdateAccount_ShouldFail_WhenAccountDoesNotExistAndNull()
        {
            var result = _account.UpdateAccount(null,"");
            AsserApiHelpers.AsserApiError(result, "Invalid request: Request body is missing", "Null request received");
        }
        [Fact]
        public void UpdateAccount_ShouldFail_WhenEmailFormatInvalid()
        {
            var addAccount = new AccountRequest
            {
                AccountEmail = "invalid-email-format",
                AccountName = "John Doe",
                BirthDate = DateTime.Parse("2011-11-11"),
                Gender = GenderOptions.Female,
            };
            var addedAccount = _account.AddAccount(addAccount);

            AsserApiHelpers.AsserApiError(addedAccount, "Validation Failed", "Invalid email address format.");
        }
        [Fact]
        public void UpdateAccount_ShouldFail_WhenGenderIsInvalid()
        {
            var addAccount = new AccountRequest
            {
                AccountEmail = "john@gmail.com",
                AccountName = "John Doe",
                BirthDate = DateTime.Parse("2011-11-11"),
                Gender = (GenderOptions)32423,
            };
            var result = _account.AddAccount(addAccount);
            AsserApiHelpers.AsserApiError(result,"Validation Failed", "Invalid Gender Options.");
        }
        [Fact]
        public void UpdateAccount_ShouldSucceed_WithValidData()
        {
            var addedAccount = _account.AddAccount(AddedAccount.AddAccount());

            var ID = addedAccount.Data.AccountNumber;
            var updateAccount = new AccountUpdateRequest
            {
                AccountName = "Updated Name",
                AccountEmail = "sadasdsa@gmail.com",
                BirthDate = DateTime.Parse("2021-11-11"),
                Gender = GenderOptions.Male
            };
            var update = _account.UpdateAccount(updateAccount,ID);
            AsserApiHelpers.AsserApiSuccess(update, "Successfully Updated your Account");
        }
        #endregion

        #region Delete Account
        [Fact]
        public void DeleteAccount_ShouldFail_WhenAccountDoesNotExist()
        {
            var result = _account.DeleteAccount("CA1234567890");
            AsserApiHelpers.AsserApiError(result, "Validation Failed", "Account Id Cannot be Found");
        }
        [Fact]
        public void DeleteAccount_ShouldSucceed_WhenAccountExists()
        {
            var addedAccount = _account.AddAccount(AddedAccount.AddAccount());

            var ID = addedAccount.Data.AccountNumber;

            var deleteAccount = _account.DeleteAccount(ID);

            AsserApiHelpers.AsserApiSuccess(deleteAccount, "Successfully Deleted your Account");

        }
        #endregion
    }
}
