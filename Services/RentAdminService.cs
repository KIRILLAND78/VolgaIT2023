using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VolgaIT2023.Services
{
    public class RentAdminService : BaseService
    {
        public RentAdminService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) {
            if (AuthorisedUser==null || !AuthorisedUser.IsAdmin) throw new UnauthorizedException();
        }
        public Rent GetRentInfo(long id)
        {
            var res = _databaseContext.Rents.Find(id);
            if (res == null) throw new NotFoundException("Rent");
            return res;
        }
        public IEnumerable<Rent> GetUserHistory(long id)
        {
            return _databaseContext.Rents.Where(i=>i.UserId==id).AsEnumerable();
        }
        public IEnumerable<Rent> GetTransportHistory(long id)
        {
            return _databaseContext.Rents.Where(i => i.TransportId == id).AsEnumerable();
        }
        public Rent CreateRent(RentAdminCreateRequest rent)
        {
            Rent res = _databaseContext.Rents.Add(new Rent(rent)).Entity;
            return res;
        }
        public Rent EndRent(long id, double Lat, double Long)
        {
            var res = _databaseContext.Rents.Find(id);
            if (res == null) throw new NotFoundException("Rent");
            res.End(Lat, Long);
            _databaseContext.SaveChanges();
            return res;
        }
        public Rent UpdateRent(long id, RentAdminCreateRequest rent)
        {
            var res = _databaseContext.Rents.Find(id);
            if (res == null) throw new NotFoundException("Rent");
            res.Update(rent);
            _databaseContext.SaveChanges();
            return res;
        }
        public Rent DeleteRent(long id)
        {
            var res = _databaseContext.Rents.Find(id);
            if (res == null) throw new NotFoundException("Rent");
            _databaseContext.Remove(res);
            _databaseContext.SaveChanges();
            return res;
        }
    }
}
