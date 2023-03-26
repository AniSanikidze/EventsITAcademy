using EventsITAcademy.API.Infrastructure.SwaggerExamples;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "User")]
    [Route("api/v{version:apiVersion}/events/")]
    [ApiVersion("1.0")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly string _userIdClaim;

        public EventController(IHttpContextAccessor httpContextAccessor, IEventService eventService)
        {
            if (httpContextAccessor?.HttpContext.User.Claims.Count() > 0)
            {
                _userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("UserId")!.Value;
            }
           _service = eventService;
        }

        /// <summary>
        /// Returns event based on provided id, Authorization is not required
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns>Return specific event</returns>
        /// <response code="200">Returns the specific event</response>
        /// <response code="404">If the event was not found</response>
        [ProducesResponseType(typeof(EventExample), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventExample))]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(CancellationToken cancellationToken, int id)
        {
            return Ok(await _service.GetAsync(cancellationToken, id).ConfigureAwait(false));
        }

        /// <summary>
        /// Returns list of events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>list of events</returns>
        [ProducesResponseType(typeof(IEnumerable<EventResponseModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventExamples))]
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetAllConfirmedAsync(cancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Creates an event
        /// </summary>
        /// <remarks>
        /// Note id is not required
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns>Returns updated todo with subtasks</returns>
        /// <response code="200">Returns newly created event's Id</response>
        /// <response code="400">Invalid request</response>
        [HttpPost]
        public async Task<ActionResult<int>> Post(CancellationToken cancellationToken, EventRequestModel request)
        {
            return Ok(await _service.CreateAsync(cancellationToken, request, _userIdClaim).ConfigureAwait(false));
        }

        /// <summary>
        /// Updates event
        /// </summary>
        /// <remarks>
        /// 
        /// PUT/event
        /// 
        ///  {
        ///     "id": 1,
        ///     "title": "Event",
        ///     "description": "Updated desc",
        ///     "startDate": "2023-03-T16:39:06.224Z",
        ///     "finishDate": "2023-03-13T16:39:06.224Z",
        ///     "numberOfTickets": 30,
        ///     "modificationPeriod": 1
        ///  }
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns>Returns newly created event's id</returns>
        /// <response code="200">Returns updated event's id</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">If the event was not found</response>
        [HttpPut]
        public async Task<ActionResult<int>> Put(CancellationToken cancellationToken, UpdateEventRequestModel request,int id)
        {
            return Ok(await _service.UpdateAsync(cancellationToken, request, _userIdClaim).ConfigureAwait(false));
        }
    }
}
