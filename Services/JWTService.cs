using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class JWTService
    {
        DatabaseContext _db;
        public JWTService([FromServices] DatabaseContext db) { _db = db; }

        public List<Claim>? AddSessionGetClaims(IAccountSignData sign, DateTime expires)
        {
            Account? person = _db.Accounts.FirstOrDefault(x => x.Username == sign.Username && x.Password == sign.Password);
            if (person == null) return null;
            Session dbSession = _db.Sessions.Add(new Session { AccountId = person.Id, ExpireTime = expires.ToString("o", CultureInfo.InvariantCulture) }).Entity;
            //Здесь мы убираем токены, если их больше 5.
            List<Session> tokens = _db.Sessions.Where(i=>i.AccountId==person.Id).OrderByDescending(i=>i.Id).Skip(5).ToList();
            foreach (var token in tokens )
            {
                _db.Sessions.Remove(token);
            }
            _db.SaveChanges();
            //
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, person.Username),
                    new Claim(ClaimTypes.NameIdentifier, $"{person.Id}"),
                    new Claim(ClaimTypes.Sid, $"{dbSession.Id}"),//SID?
                };
            return claims;
        }
        public string GenerateToken(IAccountSignData sign)
        {
            DateTime now = DateTime.UtcNow;
            DateTime expireTime = now.Add(TimeSpan.FromMinutes(Convert.ToInt64(Environment.GetEnvironmentVariable("TOKEN_LIFETIME")??"60")));//Стоит добавить в руководстве, что по умолчанию asp разрешает в 5-минутном диапазоне использовать токены после истечения срока его жизни. 

            List<Claim> identity = AddSessionGetClaims(sign, expireTime);
            if (identity == null)
            {
                throw new UnauthorizedException();
            }
            // создание jwt строки
            var jwt = new JwtSecurityToken(
            notBefore: now,
            claims: identity,
            expires: expireTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        public void InvalidateToken(HttpContext context)
        {
            Session session = _db.Sessions.Where(s => s.Id == Convert.ToInt64(context.User.FindFirstValue(ClaimTypes.Sid))).FirstOrDefault()!;
            _db.Sessions.Remove(session);
            _db.SaveChanges();
            return;
        }
    }
}
