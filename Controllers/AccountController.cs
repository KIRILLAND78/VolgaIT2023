using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AccountService _accountService;
        public AccountController([FromServices]AccountService accountService) { _accountService = accountService; }
        
        // GET: api/<AccountController>/Me
        /// <summary>
        /// получение данных о текущем аккаунте
        /// </summary>
        [HttpGet("Me")]
        public ApiResponse Me()
        {
            return new ApiResponse(_accountService.GetSelfInfo());
        }

        // POST api/<AccountController>/SignIn
        /// <summary>
        /// получение нового jwt токена пользователя
        /// </summary>
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public ApiResponse SignIn([FromServices] JWTService jwt, [FromBody] AccountSignRequest request)
        {
            return new ApiResponse(jwt.GenerateToken(request));
        }

        // POST api/<AccountController>/SignUp
        /// <summary>
        /// регистрация нового аккаунта
        /// </summary>
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public ApiResponse SignUp([FromServices] JWTService jwt, [FromBody] AccountCreateRequest request)
        {
            _accountService.CreateAccount(request);
            return new ApiResponse(jwt.GenerateToken(request));
        }

        // POST api/<AccountController>/SignOut
        /// <summary>
        /// выход из аккаунта (jwt-токен перестает работать)
        /// </summary>
        [HttpPost("SignOut")]
        public ApiResponse SignOut([FromServices] JWTService jwt)
        {
            jwt.InvalidateToken(HttpContext);
            return new ApiResponse();
        }


        // PUT api/<AccountController>/Update
        /// <summary>
        /// обновление своего аккаунта
        /// </summary>
        [HttpPut("Update")]
        public ApiResponse Update([FromBody] AccountCreateRequest account)
        {
            return new ApiResponse(_accountService.UpdateAccount(account));
        }

    }
}
