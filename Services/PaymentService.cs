using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class PaymentService : BaseService
    {
        public PaymentService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) { }

        public Account Hesoyam(long id)
        {
            Account subject = _databaseContext.Accounts.Find(id);
            if (subject == null) throw new NotFoundException("User");
            if (!AuthorisedUser.IsAdmin && AuthorisedUser.Id != id) throw new UnauthorizedException();

            subject.Balance += 250000;
            //_databaseContext.Accounts.Update(subject);
            _databaseContext.SaveChanges();
            return subject;
        }
    }
}
