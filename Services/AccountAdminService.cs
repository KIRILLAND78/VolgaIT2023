using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class AccountAdminService : BaseService
    {
        public AccountAdminService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext)
        {
            if (AuthorisedUser == null || !AuthorisedUser.IsAdmin) throw new UnauthorizedException();
        }
        public Account CreateAccount(Account request)
        {
            _databaseContext.Add(request);
            _databaseContext.SaveChanges();
            return request;
        }
        public Account UpdateAccount(long id, Account request)
        {
            Account? acc = _databaseContext.Accounts.Find(id);
            if (acc == null) throw new NotFoundException("Account");
            acc.Update(request);
            _databaseContext.SaveChanges();
            return acc;
        }
        public Account DeleteAccount(long id)
        {
            Account? acc = _databaseContext.Accounts.Find(id);
            if (acc == null) throw new NotFoundException("Account");
            _databaseContext.Remove(acc);
            _databaseContext.SaveChanges();
            return acc;
        }
        public IEnumerable<Account> ListAccounts(PaginationRequest request)
        {
            return _databaseContext.Accounts.Skip(request.Start).Take(request.Count).AsEnumerable();
        }
        public Account GetAccount(long id) 
        {
            var res = _databaseContext.Accounts.Find(id);
            if (res == null) throw new NotFoundException("Account");
            return res;
        }
    }
}
