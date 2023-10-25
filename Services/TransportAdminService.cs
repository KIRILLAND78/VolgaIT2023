using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VolgaIT2023.Services
{
    public class TransportAdminService : BaseService
    {
        public TransportAdminService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) {
            if (AuthorisedUser==null || !AuthorisedUser.IsAdmin) throw new UnauthorizedException();
        }


        public IEnumerable<Transport> GetTransportsList(PaginationRequest pagination, string transportType)
        {
            var res = _databaseContext.Transports.AsQueryable();
            if (transportType != "All")
            {
                res = res.Where(i => i.TransportType == transportType);
            }
            return res.Skip(pagination.Start).Take(pagination.Count).AsEnumerable();
        }
        public Transport CreateTransport(TransportAdminCreateRequest request)
        {
            var res = _databaseContext.Transports.Add(new Transport(request)).Entity;
            _databaseContext.SaveChanges();
            return res;
        }
        public Transport UpdateTransport(long id, TransportAdminCreateRequest request)
        {
            var res = _databaseContext.Transports.Find(id);
            if (res == null) throw new NotFoundException("Transport");
            res.Update(request);
            _databaseContext.SaveChanges();
            return res;
        }
        public Transport DeleteTransport(long id)
        {
            var res = _databaseContext.Transports.Find(id);
            if (res == null) throw new NotFoundException("Transport");
            _databaseContext.Remove(res);
            _databaseContext.SaveChanges();
            return res;
        }
    }
}
