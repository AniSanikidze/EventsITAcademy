using EventsITAcademy.API.Infrastructure.SwaggerExamples;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Roles;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/admin/")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }
        /// <summary>
        /// Returns list of unconfirmed events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>list of events</returns>
        [ProducesResponseType(typeof(IEnumerable<EventResponseModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventExamples))]
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        [HttpGet("events/unconfirmed")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetAllUnconfirmed(CancellationToken cancellationToken)
        {
            return Ok(await _eventService.GetAllUnconfirmedAsync(cancellationToken).ConfigureAwait(false));
        }
        /// <summary>
        /// Markes event as active
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the activted event</returns>
        /// <response code="200">Successfully activated event</response>
        /// <response code="404">If the event was not found</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("events/confirm/{id}")]
        public async Task<ActionResult<int>> ActivateEvent(int id, CancellationToken cancellationToken)
        {
            return Ok(await _eventService.ActivateEvent(cancellationToken, id).ConfigureAwait(false));
        }
        /// <summary>
        /// Markes event as archived
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the archived event</returns>
        /// <response code="200">Successfully archived event</response>
        /// <response code="404">If the event was not found</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut("events/archive/{id}")]
        public async Task<ActionResult<int>> ArchiveEvent(int id, CancellationToken cancellationToken)
        {
            return Ok(await _eventService.ArchiveEvent(cancellationToken, id).ConfigureAwait(false));
        }
        /// <summary>
        /// Returns list of archived events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>list of archived events</returns>
        [ProducesResponseType(typeof(IEnumerable<EventResponseModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventExamples))]
        [Produces("application/json")]
        [AllowAnonymous]
        [HttpGet("events/archived")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetAllArchivedAsync(CancellationToken cancellationToken)
        {
            return Ok(await _eventService.GetAllArchivedAsync(cancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Updated event
        /// </summary>
        /// <param name="eventRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the updated event</returns>
        /// <response code="200">Successfully updated event</response>
        /// <response code="404">If the event was not found</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpPut("event/{id}")]
        public async Task<ActionResult<int>> UpdateEvent(CancellationToken cancellationToken, AdminUpdateEventRequestModel eventRequest)
        {
            return Ok(await _eventService.UpdateEventByAdminAsync(cancellationToken, eventRequest).ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the deleted event</returns>
        /// <response code="200">Successfully deleted event</response>
        /// <response code="404">If the event was not found</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpDelete("event/{id}")]
        public async Task<ActionResult<int>> DeleteAsync(CancellationToken cancellationToken, int id)
        {
            await _eventService.DeleteAsync(cancellationToken,id).ConfigureAwait(false);    
            return NoContent();
        }

        /// <summary>
        /// returns list of users
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns>Returns newly created user data</returns>
        /// <response code="200">Returns list of users</response>
        [ProducesResponseType(typeof(IEnumerable<UserResponseModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserExamples))]
        [Produces("application/json")]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsersAsync(CancellationToken cancellation)
        {
            return Ok(await _userService.GetAllUsersAsync(cancellation).ConfigureAwait(false));
        }
        /// <summary>
        /// Assignes role to the user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the archived event</returns>
        /// <response code="200">Successfully archived event</response>
        /// <response code="404">If the event or user was not found</response>
        /// <response code="409">If the user is already assigned to the role</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> AssignRole(string id, string roleId, CancellationToken cancellationToken)
        {
            await _roleService.AssignRoleToUserAsync(cancellationToken, id, roleId).ConfigureAwait(false);
            return Ok();
        }
        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns id of the archived event</returns>
        /// <response code="200">Successfully deleted user</response>
        /// <response code="404">If the user was not found</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            await _roleService.RemoveUserRoleAsync(cancellationToken, id).ConfigureAwait(false);
            await _userService.DeleteUserAsync(cancellationToken, id).ConfigureAwait(false);
            return RedirectToAction("Users");
        }
    }
}
