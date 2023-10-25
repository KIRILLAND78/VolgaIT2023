using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VolgaIT2023.Models;

namespace VolgaIT2023.Middleware
{
    public class TokenCheckerMiddleware : IMiddleware
    {
        DatabaseContext _databaseContext;
        public TokenCheckerMiddleware([FromServices] DatabaseContext databaseContext) { _databaseContext = databaseContext; }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            if (context.User.FindFirstValue(ClaimTypes.Sid) != null)
            {
                Session? session = _databaseContext.Sessions.Where(i => i.Id == Convert.ToInt64(context.User.FindFirstValue(ClaimTypes.Sid))).FirstOrDefault();
                if (session==null) throw new UnauthorizedException();
            }
            if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                Account? account = _databaseContext.Accounts.Where(i => i.Id == Convert.ToInt64(context.User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
                if (account!=null)
                {
                    context.User.Identities.FirstOrDefault()!.AddClaim(new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"));
                }
            }
            await next(context);
        }
    }
}
