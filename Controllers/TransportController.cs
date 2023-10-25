using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using VolgaIT2023.Models;
using VolgaIT2023.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VolgaIT2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransportController : ControllerBase
    {
        TransportService _transportService;
        public TransportController([FromServices] TransportService transportService) { _transportService = transportService; }

        // GET: api/<TransportController>/{id}
        [HttpGet("{id}")]
        public ApiResponse TransportGet([FromRoute] long id)
        {
            return new ApiResponse(_transportService.GetTransportInfo(id));
        }

        // POST: api/<TransportController>
        [HttpPost()]
        public ApiResponse Post([FromBody] TransportCreateRequest request)
        {
            return new ApiResponse(_transportService.CreateTransport(request));
        }

        // PUT: api/<TransportController>/{id}
        [HttpPut("{id}")]
        public ApiResponse Edit([FromRoute] long id, [FromBody] TransportCreateRequest request)
        {
            return new ApiResponse(_transportService.UpdateTransport(request, id));
        }

        // DELETE: api/<TransportController>/{id}
        [HttpDelete("{id}")]
        public ApiResponse Delete([FromRoute] long id)
        {
            return new ApiResponse(_transportService.DeleteTransport(id));
        }

    }
}
