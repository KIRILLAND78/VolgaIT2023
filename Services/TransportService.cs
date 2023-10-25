using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace VolgaIT2023.Services
{
    public class TransportService : BaseService
    {
        public TransportService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) { }

        public Transport CreateTransport(TransportCreateRequest request)
        {
            Transport transport = new Transport(request);
            transport.OwnerId = AuthorisedUser.Id;
            _databaseContext.Transports.Add(transport);
            _databaseContext.SaveChanges();
            return transport;
        }
        public Transport UpdateTransport(TransportCreateRequest request, long id)
        {
            var res = _databaseContext.Transports.Find(id);
            if (res == null) throw new NotFoundException("Transport");
            if (res.OwnerId != AuthorisedUser.Id) throw new UnauthorizedException();

            res.Update(request);
            _databaseContext.Transports.Update(res);
            _databaseContext.SaveChanges();
            return res;
        }
        public Transport DeleteTransport(long id)
        {
            var res = _databaseContext.Transports.Where(i=>i.Id==id).Include(i => i.Rents).FirstOrDefault();
            if (res == null) throw new NotFoundException("Transport");
            if (res.OwnerId != AuthorisedUser.Id) throw new UnauthorizedException();
            foreach (var i in res.Rents)
            {
                if (i.TimeEnd != null) throw new Exception("Currently Rented");
            }


            _databaseContext.Transports.Remove(res);
            _databaseContext.SaveChanges();
            return res;
        }
        public IEnumerable<Transport> GetTransportsByCoords(TransportListRequest request)
        {
            var res = _databaseContext.Transports.AsQueryable();
            if (request.Type != "All")
            {
                res = res.Where(i => i.TransportType == request.Type);
            }
            return res.Include(i => i.Rents).Where(Transport.IsRentableQuery).Where(i=>
                    (Math.Sqrt((Math.Pow(i.Longitude - request.Long, 2) + Math.Pow(i.Latitude - request.Lat, 2))) <= request.Radius))
                .AsEnumerable();
        }
        public Transport GetTransportInfo(long id)
        {
            var res = _databaseContext.Transports.Find(id);
            if (res == null) throw new NotFoundException("Transport");
            return res;
        }
    }
}
