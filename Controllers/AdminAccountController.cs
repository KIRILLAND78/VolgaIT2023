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
        /// <summary>
        /// Получение списка всех аккаунтов
        /// </summary>
        [HttpGet()]
        public ApiResponse Index([FromQuery] PaginationRequest pagination)
        {
            return new ApiResponse(_accountAdminService.ListAccounts(pagination));
        }

        //GET: api/{AdminAccountController}/{id}
        /// <summary>
        /// Получение информации об аккаунте по id
        /// </summary>
        [HttpGet("{id}")]
        public ApiResponse Get([FromRoute] long id)
        {
            return new ApiResponse(_accountAdminService.GetAccount(id));
        }

        //POST: api/{AdminAccountController}
        /// <summary>
        /// Создание администратором нового аккаунта
        /// </summary>
        [HttpPost]
        public ApiResponse Create([FromBody] AccountAdminCreateRequest account)
        {
            return new ApiResponse(_accountAdminService.CreateAccount(account));
        }

        //PUT: api/{AdminAccountController}/{accountId}
        /// <summary>
        /// Изменение администратором аккаунта по id
        /// </summary>
        [HttpPut("{id}")]
        public ApiResponse Edit([FromRoute] long id, [FromBody] AccountAdminCreateRequest account)
        {
            return new ApiResponse(_accountAdminService.UpdateAccount(id, account));
        }

        //DELETE: api/{AdminAccountController}/{accountId}
        /// <summary>
        /// Удаление аккаунта по id
        /// </summary>
        [HttpDelete("{id}")]
        public ApiResponse Delete([FromRoute] long id)
        {
            return new ApiResponse(_accountAdminService.DeleteAccount(id));
        }
    }
}
