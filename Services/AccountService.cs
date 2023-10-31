using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class AccountService : BaseService
    {
        public AccountService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) { }

        public Account CreateAccount(AccountCreateRequest request)
        {
            if (AuthorisedUser != null && !AuthorisedUser.IsAdmin) throw new UnauthorizedException();
            Account account = new Account(request);
            if (_databaseContext.Accounts.Count()==0) account.IsAdmin = true;
            _databaseContext.Accounts.Add(account);
            _databaseContext.SaveChanges();
            return account;
        }
        public Account? GetSelfInfo()
        {
            return AuthorisedUser;
        }
        public Account? UpdateAccount(AccountCreateRequest request)
        {
            AuthorisedUser.Update(request);
            _databaseContext.SaveChanges();
            return AuthorisedUser;
        }
    }
}
