﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

namespace VolgaIT2023.Controllers
{
    [Authorize("AdminOnly")]
    [Route("api/Admin/Transport")]
    [ApiController]
    public class AdminTransportController : ControllerBase
    {
        TransportAdminService _transportAdminService;
        TransportService _transportService;
        public AdminTransportController([FromServices] TransportAdminService transportAdminService, [FromServices] TransportService transportService) { _transportAdminService = transportAdminService; _transportService = transportService; }

        //GET: api/{AdminTransportController}
        /// <summary>
        /// Получение списка всех транспортных средств
        /// </summary>
        [HttpGet()]
        public ApiResponse Index([FromQuery] PaginationRequest pagination, [FromQuery] string type)
        {
            return new ApiResponse(_transportAdminService.GetTransportsList(pagination, type));
        }

        //GET: api/{AdminTransportController}/{transportId}
        /// <summary>
        /// Получение информации о транспортном средстве по id
        /// </summary>
        [HttpGet("{transportId}")]
        public ApiResponse Get([FromRoute] long transportId)
        {
            return new ApiResponse(_transportService.GetTransportInfo(transportId));
        }

        //POST: api/{AdminTransportController}
        /// <summary>
        /// Создание нового транспортного средства
        /// </summary>
        [HttpPost()]
        public ApiResponse Post([FromBody] TransportAdminCreateRequest transport)
        {
            return new ApiResponse(_transportAdminService.CreateTransport(transport));
        }

        //PUT: api/{AdminTransportController/{id}}
        /// <summary>
        /// Изменение транспортного средства по id
        /// </summary>
        [HttpPut("{id}")]
        public ApiResponse Edit([FromRoute] long id, [FromBody] TransportAdminCreateRequest transport)
        {
            return new ApiResponse(_transportAdminService.UpdateTransport(id, transport));
        }

        //DELETE: api/{AdminTransportController/{id}}
        /// <summary>
        /// Удаление транспорта по id
        /// </summary>
        [HttpDelete("{id}")]
        public ApiResponse Delete([FromRoute] long id)
        {
            return new ApiResponse(_transportAdminService.DeleteTransport(id));
        }
    }
}
