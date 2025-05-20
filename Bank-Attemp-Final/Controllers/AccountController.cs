
using Microsoft.AspNetCore.Mvc;
using ModelDto.AccountDto;
using Service;
using Service.IService;

namespace Bank_Attemp_Final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;

        public AccountController(IAccountService account)
        {
            _account = account;
        }

        [HttpPost]
        public IActionResult AddAccount(AccountRequest account)
        {
            // Ensure the AddAccount method returns a valid ResponseApi type
            var result =  _account.AddAccount(account);

            if (!result.isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            // Ensure the GetListAccounts method returns a valid ResponseApi type
            var result = _account.GetListAccounts();
            if (result == null || !result.isSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(string id)
        {
            // Ensure the GetAccountByID method returns a valid ResponseApi type
            var result = _account.GetAccountByID(id);
            if (result == null || !result.isSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateAccount([FromBody] AccountUpdateRequest account,string id)
        {
            // Ensure the UpdateAccount method returns a valid ResponseApi type
            var result = _account.UpdateAccount( account,id);
            if (!result.isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(string id)
        {
            // Ensure the DeleteAccount method returns a valid ResponseApi type
            var result = _account.DeleteAccount(id);
            if (!result.isSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
