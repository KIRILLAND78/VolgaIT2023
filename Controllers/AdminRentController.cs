using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Authorize("AdminOnly")]
    [Route("api/Admin")]
    [ApiController]
    public class AdminRentController : ControllerBase
    {
        RentAdminService _rentAdminService;
        public AdminRentController([FromServices] RentAdminService rentAdminService) { _rentAdminService = rentAdminService; }

        //GET: api/{AdminRentController}/Rent/{rentId}
        [HttpGet("Rent/{rentId}")]
        public ApiResponse Get([FromRoute] long rentId)
        {
            return new ApiResponse(_rentAdminService.GetRentInfo(rentId));
        }

        //GET: api/{AdminRentController}/UserHistory/{userId}
        [HttpGet("UserHistory/{userId}")]
        public ApiResponse IndexUserHistory([FromRoute] long userId)
        {
            return new ApiResponse(_rentAdminService.GetUserHistory(userId));
        }

        //GET: api/{AdminRentController}/TransportHistory/{transportId}
        [HttpGet("TransportHistory/{transportId}")]
        public ApiResponse IndexTransportHistory([FromRoute] long transportId)
        {
            return new ApiResponse(_rentAdminService.GetTransportHistory(transportId));
        }

        //POST: api/{AdminRentController}/Rent
        [HttpPost("Rent")]
        public ApiResponse Create([FromBody] RentAdminCreateRequest rent)
        {
            return new ApiResponse(_rentAdminService.CreateRent(rent));
        }

        //POST: api/{AdminRentController}/Rent/End/{rentId}
        [HttpPost("Rent/End/{rentId}")]
        public ApiResponse End([FromRoute]long rentId, [FromQuery] double Lat, [FromQuery] double Long)//todo
        {
            return new ApiResponse(_rentAdminService.EndRent(rentId, Lat, Long));
        }

        //PUT: api/{AdminRentController}/Rent/{rentId}
        [HttpPut("Rent/{rentId}")]
        public ApiResponse Edit([FromRoute] long rentId, [FromBody] RentAdminCreateRequest rent)
        {
            return new ApiResponse(_rentAdminService.UpdateRent(rentId, rent));
        }

        //Delete: api/{AdminRentController}/Rent/{rentId}
        [HttpDelete("Rent/{rentId}")]
        public ApiResponse Delete([FromRoute] long rentId)
        {
            return new ApiResponse(_rentAdminService.DeleteRent(rentId));
        }
    }
}
