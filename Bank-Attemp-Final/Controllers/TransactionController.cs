using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelDto.TransactionDto;
using Service.IService;

namespace Bank_Attemp_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] TransactionRequest transaction)
        {
            var result = _transactionService.DepositAccount(transaction);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Withdraw")]
        public IActionResult Withdraw([FromBody] TransactionRequest transaction)
        {
            var result = _transactionService.WithdrawAccount(transaction);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
