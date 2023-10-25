using System.Security.Claims;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class BaseService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly DatabaseContext _databaseContext;
        public BaseService(IHttpContextAccessor httpContextAccessor, DatabaseContext dbc) {
            _httpContextAccessor = httpContextAccessor;
            _databaseContext = dbc;
        }
        public Account? AuthorisedUser { get { return _databaseContext.Accounts.Where(i => i.Id == Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault(); } }
    }
}
