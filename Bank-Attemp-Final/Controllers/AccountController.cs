
using Microsoft.AspNetCore.Mvc;
using ModelDto.AccountDto;
using Service;
using Service.IService;

namespace Bank_Attemp_Final.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IAccountService _account;

        public AccountController(AccountService accountService, IAccountService account)
        {
            _account = account;
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Add Account")]
        public IActionResult AddAccount(AccountRequest account)
        {
            // Ensure the AddAccount method returns a valid ResponseApi type
            var result =  _accountService.AddAccount(account);

            if (!result.isSuccess)
            {
                return BadRequest("Failed to add account.");
            }
            return Ok("Account added successfully");
        }

        [HttpGet]
        [Route("Get All Accounts")]
        public IActionResult GetAllAccounts()
        {
            // Ensure the GetListAccounts method returns a valid ResponseApi type
            var result = _account.GetListAccounts();
            if (result == null || !result.isSuccess)
            {
                return NotFound("No accounts found.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Route("Get Account")]
        public IActionResult GetAccount(string id)
        {
            // Ensure the GetAccountByID method returns a valid ResponseApi type
            var result = _account.GetAccountByID(id);
            if (result == null || !result.isSuccess)
            {
                return NotFound("Account not found.");
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]
        [Route("Update Account")]
        public IActionResult UpdateAccount([FromBody] AccountUpdateRequest account,string id)
        {
            // Ensure the UpdateAccount method returns a valid ResponseApi type
            var result = _account.UpdateAccount( account,id);
            if (!result.isSuccess)
            {
                return BadRequest("Failed to update account.");
            }
            return Ok("Account updated successfully");
        }

        [HttpDelete("{id}")]
        [Route("Delete Account")]
        public IActionResult DeleteAccount(string id)
        {
            // Ensure the DeleteAccount method returns a valid ResponseApi type
            var result = _account.DeleteAccount(id);
            if (!result.isSuccess)
            {
                return NotFound("Failed to delete account.");
            }
            return Ok("Account deleted successfully");
        }
    }
}
