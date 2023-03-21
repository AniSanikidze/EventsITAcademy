using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Tickets;
using EventsITAcademy.Application.Tickets.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsITAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string userIdClaim;
        private readonly string role;

        public TicketController(IHttpContextAccessor httpContextAccessor, ITicketService ticketService)
        {
            _contextAccessor = httpContextAccessor;
            if (httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("UserId")!.Value;
                role = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)!.Value;
            }

            bool isAdmin;

            if (httpContextAccessor.HttpContext.User != null)
                isAdmin = httpContextAccessor.HttpContext.User.IsInRole("Admin");

            _service = ticketService;
        }

        [Produces("application/json")]
        [HttpPost("v1/ticket/reserve")]
        public async Task<ActionResult<EventResponseModel>> Reserve(CancellationToken cancellationToken, int eventId)
        {
            return Ok(await _service.Reserve(cancellationToken, eventId, userIdClaim));
        }


        [Produces("application/json")]
        [HttpPost("v1/ticket/buy")]
        public async Task<ActionResult<EventResponseModel>> Buy(CancellationToken cancellationToken, int eventId)
        {
            return Ok(await _service.Buy(cancellationToken, eventId, userIdClaim));
        }
    }
}
