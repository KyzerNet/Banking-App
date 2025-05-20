using ModelDto.AccountDto;
using Models.Enums;

namespace ProjectTesting.TestHelpers
{
    public class AddedAccount
    {
        public static AccountRequest AddAccount()
        {
            return new AccountRequest
            {
                AccountName = "Shimma",
                AccountEmail = "shimma@gmail.com",
                BirthDate = DateTime.Parse("2015-11-11"),
                Gender = GenderOptions.Female,
            };
        } 
        public static AccountRequest AddAccount1()
        {
            return new AccountRequest
            {
                AccountName = "marina",
                AccountEmail = "marina@gmail.com",
                BirthDate = DateTime.Parse("2015-11-11"),
                Gender = GenderOptions.Female,
            };
        } 
        public static AccountRequest AddAccount2()
        {
            return new AccountRequest
            {
                AccountName = "Ashley",
                AccountEmail = "Ashley@gmail.com",
                BirthDate = DateTime.Parse("2015-11-11"),
                Gender = GenderOptions.Female,
            };
        }
    }
}
