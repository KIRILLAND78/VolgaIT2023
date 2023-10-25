using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        PaymentService _paymentService;
        public PaymentController([FromServices] PaymentService paymentService) { _paymentService = paymentService; }
        //POST: api/{PaymentController}/Hesoyam/{accountId}
        [HttpPost("Hesoyam/{accountId}")]
        public ApiResponse Payment([FromRoute] long accountId)
        {
            return new ApiResponse(_paymentService.Hesoyam(accountId));

        }
    }
}
