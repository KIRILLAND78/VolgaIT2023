using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VolgaIT2023.Models;

namespace VolgaIT2023.Services
{
    public class RentService : BaseService
    {
        public RentService(IHttpContextAccessor httpContextAccessor, DatabaseContext databaseContext) : base(httpContextAccessor, databaseContext) { }

        public Rent GetRentInfo(long id)
        {
            var res = _databaseContext.Rents.Find(id);
            if (res == null) throw new NotFoundException("Rent");
            if (!(res.UserId==AuthorisedUser.Id || res.RentedTransport.OwnerId == AuthorisedUser.Id)) throw new UnauthorizedException();
            return res;
        }
        public IEnumerable<Rent> GetRentHistory()
        {
            return _databaseContext.Rents.Where(i => i.Id == AuthorisedUser.Id).AsEnumerable();
        }
        public IEnumerable<Rent> GetTransportHistory(long id)
        {
            var transport = _databaseContext.Transports.Find(id);
            if (transport == null) throw new NotFoundException("Transport");
            if (transport.OwnerId != AuthorisedUser.Id) throw new UnauthorizedException();
            return _databaseContext.Rents.Where(i => i.TransportId == transport.Id).AsEnumerable();
        }
        public Rent RentTransport(long id, string type)
        {
            var transport = _databaseContext.Transports.Include(i=>i.Rents).Where(i=>i.Id==id).FirstOrDefault();
            if (transport == null) throw new NotFoundException("Transport");
            if (transport.OwnerId == AuthorisedUser.Id) throw new Exception("Owner Cannot Rent Own Vehicle");
            if (!transport.IsRentable()) throw new Exception("Cannot Currently Be Rented");
            if (AuthorisedUser.Balance<=0) throw new Exception("Negative Balance");//todo придумать ошибку получше
            var price = (type == "Days") ? transport.DayPrice : transport.MinutePrice;
            if (price == null) throw new Exception("This type of rent is not supported by this transport.");

            var rent = new Rent { TransportId = transport.Id, UserId = AuthorisedUser.Id, TimeStart = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture), PriceOfUnit = (double)price, PriceType = type };
            _databaseContext.Add(rent);
            _databaseContext.SaveChanges();
            return rent;
        }
        public Rent EndRent(long id, double Lat, double Long)
        {
            var rent = _databaseContext.Rents.Where(i=>i.Id==id).Include(i=>i.User).Include(i=>i.RentedTransport).FirstOrDefault();
            if (rent == null) throw new NotFoundException("Rent");
            if (rent.UserId != AuthorisedUser.Id) throw new UnauthorizedException();
            if (rent.TimeEnd!=null) throw new Exception("Rent Has Ended");
            rent.End(Lat, Long);
            //_databaseContext.Update(rent);
            _databaseContext.SaveChanges();
            return rent;
        }
    }
}
