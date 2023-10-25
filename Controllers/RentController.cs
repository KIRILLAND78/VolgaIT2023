using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentController : ControllerBase
    {
        TransportService _transportService;
        RentService _rentService;
        public RentController([FromServices] TransportService transportService, [FromServices] RentService rentService)
        { _transportService = transportService;
            _rentService = rentService;
        }

        //GET: api/{RentController}/Transport
        [AllowAnonymous]
        [HttpGet("Transport")]
        public ApiResponse Transport([FromQuery] TransportListRequest request)
        {
            return new ApiResponse(_transportService.GetTransportsByCoords(request));
        }

        //GET: api/{RentController}/{rentId}
        [HttpGet("{rentId}")]
        public ApiResponse GetId([FromRoute] long rentId)
        {
            return new ApiResponse(_rentService.GetRentInfo(rentId));
        }

        //GET: api/{RentController}/MyHistory
        [HttpGet("MyHistory")]
        public ApiResponse History()
        {
            return new ApiResponse(_rentService.GetRentHistory());
        }

        //GET: api/{RentController}/TransportHistory/{transportId}
        [HttpGet("TransportHistory/{transportId}")]
        public ApiResponse TransportHistory([FromRoute] long transportId)
        {
            return new ApiResponse(_rentService.GetTransportHistory(transportId));
        }

        //POST: api/{RentController}/New/{transportId}
        [HttpPost("New/{transportId}")]
        public ApiResponse NewRent([FromRoute] long transportId,
        [RegularExpression("Days|Minutes", ErrorMessage = "rentType must be Days or Minutes.")][FromQuery] string rentType)
        {
            return new ApiResponse(_rentService.RentTransport(transportId, rentType));
        }

        //POST: api/{RentController}/New/{transportId}
        [HttpPost("End/{rentId}")]
        public ApiResponse EndRent([FromRoute] long rentId, [FromQuery] double Lat, [FromQuery] double Long)
        {
            return new ApiResponse(_rentService.EndRent(rentId, Lat, Long));
        }
    }
}
