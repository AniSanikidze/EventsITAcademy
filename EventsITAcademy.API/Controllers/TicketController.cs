using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Tickets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsITAcademy.API.Controllers
{
    [Route("api/v{version:apiVersion}/ticket/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles ="User")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly string _userIdClaim;

        public TicketController(IHttpContextAccessor httpContextAccessor, ITicketService ticketService)
        {
            if (httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                _userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("UserId")!.Value;
            }
            _service = ticketService;
        }
        /// <summary>
        /// Endpoint to reserve the ticket
        /// </summary>
        /// <remarks>
        /// Note id is not required
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns>Returnes reserved ticket</returns>
        /// <response code="200">Returns newly created ticket</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">If the event or user was not found</response>
        [Produces("application/json")]
        [HttpPost("reserve")]
        public async Task<ActionResult<EventResponseModel>> Reserve(CancellationToken cancellationToken, int eventId)
        {
            return Ok(await _service.Reserve(cancellationToken, eventId, _userIdClaim));
        }
        /// <summary>
        /// Endpoint to buy the ticket
        /// </summary>
        /// <remarks>
        /// Note id is not required
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns>Returns bought ticket</returns>
        /// <response code="200">Returns bought ticket</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">If the event or user was not found</response>
        [Produces("application/json")]
        [HttpPost("buy")]
        public async Task<ActionResult<EventResponseModel>> Buy(CancellationToken cancellationToken, int eventId)
        {
            return Ok(await _service.Buy(cancellationToken, eventId, _userIdClaim));
        }
    }
}
