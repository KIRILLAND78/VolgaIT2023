using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Authorize("AdminOnly")]
    [Route("api/Admin/Account")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        AccountAdminService _accountAdminService;
        public AdminAccountController([FromServices] AccountAdminService accountAdminService) { _accountAdminService = accountAdminService; }

        //GET: api/{AdminAccountController}
        [HttpGet()]
        public ApiResponse Index([FromQuery] PaginationRequest pagination)
        {
            return new ApiResponse(_accountAdminService.ListAccounts(pagination));
        }

        //GET: api/{AdminAccountController}/{id}
        [HttpGet("{id}")]
        public ApiResponse Get([FromRoute] long id)
        {
            return new ApiResponse(_accountAdminService.GetAccount(id));
        }

        //POST: api/{AdminAccountController}
        [HttpPost]
        public ApiResponse Create([FromBody] Account account)
        {
            return new ApiResponse(_accountAdminService.CreateAccount(account));
        }

        //PUT: api/{AdminAccountController}/{accountId}
        [HttpPut("{accountId}")]
        public ApiResponse Edit([FromRoute] long accountId, [FromBody] Account account)
        {
            return new ApiResponse(_accountAdminService.UpdateAccount(accountId, account));
        }

        //DELETE: api/{AdminAccountController}/{accountId}
        [HttpDelete("{accountId}")]
        public ApiResponse Delete([FromRoute] long accountId)
        {
            return new ApiResponse(_accountAdminService.DeleteAccount(accountId));
        }
    }
}
