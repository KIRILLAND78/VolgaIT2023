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
        [HttpGet("Me")]
        public ApiResponse Me()
        {
            return new ApiResponse(_accountService.GetSelfInfo());
        }
        [AllowAnonymous]
        // POST api/<AccountController>/SignIn
        [HttpPost("SignIn")]
        public ApiResponse SignIn([FromServices] JWTService jwt, [FromBody] AccountSignRequest request)
        {
            return new ApiResponse(jwt.GenerateToken(request));
        }

        [AllowAnonymous]
        // POST api/<AccountController>/SignUp
        [HttpPost("SignUp")]
        public ApiResponse SignUp([FromServices] JWTService jwt, [FromBody] AccountCreateRequest request)
        {
            _accountService.CreateAccount(request);
            return new ApiResponse(jwt.GenerateToken(request));
        }

        // POST api/<AccountController>/SignOut
        [HttpPost("SignOut")]
        public ApiResponse SignOut([FromServices] JWTService jwt)
        {
            jwt.InvalidateToken(HttpContext);
            return new ApiResponse();
        }


        // PUT api/<AccountController>/Update
        [HttpPut("Update")]
        public ApiResponse Update([FromBody] AccountCreateRequest account)
        {
            return new ApiResponse(_accountService.UpdateAccount(account));
        }

        // PUT api/<AccountController>/SETADMIN
        [HttpPut("SETADMIN")]
        public ApiResponse Adminise([FromServices] DatabaseContext db, [FromBody] AccountSignRequest account)
        {
            Account? a = db.Accounts.Where(i => i.Username == account.Username).FirstOrDefault();
            a.IsAdmin = true;
            db.SaveChanges();
            return new ApiResponse();
        }
        // PUT api/<AccountController>/SETADMIN
        [AllowAnonymous]
        [HttpGet("SESSIONS")]
        public ApiResponse Sessions([FromServices] DatabaseContext db)
        {
            return new ApiResponse(db.Sessions.AsEnumerable());
        }

    }
}
