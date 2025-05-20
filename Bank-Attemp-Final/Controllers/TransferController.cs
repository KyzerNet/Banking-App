using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelDto.TransferResponse;
using Service.IService;

namespace Bank_Attemp_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transfer;
        public TransferController(ITransferService transfer)
        {
            _transfer = transfer;
        }

        [HttpPost]
        public IActionResult Transfer([FromBody] TranferRequest transfer)
        {
            // Ensure the Transfer method returns a valid ResponseApi type
            var result = _transfer.TransferOut(transfer);
            if (!result.isSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
