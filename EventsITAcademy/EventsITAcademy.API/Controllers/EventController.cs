using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

namespace EventsITAcademy.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]


    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string userIdClaim;
        private readonly string role;

        public EventController(IHttpContextAccessor httpContextAccessor, IEventService eventService)
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

           _service = eventService;
        }

        /// <summary>
        /// Returns event based on provided id, Authorization is not required
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns>Return specific todo</returns>
        /// <response code="200">Returns the specific todo and corresponding subtasks data</response>
        /// <response code="404">If the todo was not found or todo does not belong to the user</response>
        //[ProducesResponseType(typeof(ToDoExample), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ToDoExample))]
        [Produces("application/json")]
        [HttpGet("v1/event/{id}")]
        public async Task<ActionResult<string>> Get(CancellationToken cancellationToken, int id)
        {
            return Ok(await _service.GetAsync(cancellationToken, id));
        }

        /// <summary>
        /// Returns list of user's todos with subtasks
        /// </summary>
        /// <remarks>
        /// Note: Status is an optional query parameter to filter todos based on the following statuses:
        /// 
        ///     1 - Active
        ///     
        ///     2 - Done
        ///     
        /// By default Status is 0, which retrieves all todos regardless of the status
        /// </remarks>
        /// <param name="cancellationToken"></param>
        ///// <returns>list of todos</returns>
        //[ProducesResponseType(typeof(IEnumerable<ToDoResponseModel>), StatusCodes.Status200OK)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ToDoExamples))]
        //[Produces("application/json")]
        [HttpGet("v1/events")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetAllConfirmedAsync(cancellationToken));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("v1/events/unconfirmed")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetAllUnconfirmed(CancellationToken cancellationToken)
        {

            return Ok(await _service.GetAllUnconfirmedAsync(cancellationToken));
        }

        [Authorize(Roles = "Admin,Moderator")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("v1/events/confirm")]
        public async Task<ActionResult<EventResponseModel>> ConfirmEvent(int id,CancellationToken cancellationToken)
        {
            return Ok(await _service.ConfirmEventAsync(cancellationToken,id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("v1/user/events")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetUserEvents(CancellationToken cancellationToken)
        {
            return Ok(await _service.GetUserEventsAsync(cancellationToken, userIdClaim));
        }

        ///// <summary>
        ///// Creates an event
        ///// </summary>
        ///// <remarks>
        ///// Note id is not required
        /////
        /////     POST/Event
        /////     
        /////     {
        /////         "title": "ExampleToDo",
        /////         "targetCompletionDate": "2023-02-23T22:33:55.386Z",
        /////         "subtasks": [
        /////             {
        /////               "title": "ExampleSubtask"
        /////             }
        /////          ]    
        /////     }
        /////     POST/ToDo - Without subtasks
        /////     
        /////     {
        /////         "title": "ExampleToDo Without subtasks",
        /////         "targetCompletionDate": "2023-02-23T22:33:55.386Z"  
        /////     }
        ///// </remarks>
        ///// <param name="cancellationToken"></param>
        ///// <param name="request"></param>
        ///// <returns>Returns updated todo with subtasks</returns>
        ///// <response code="200">Returns updated todo with subtasks</response>
        //[ProducesResponseType(typeof(ToDoExample), StatusCodes.Status200OK)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ToDoExample))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [HttpPost("v1/todos/")]
        public async Task<ActionResult<EventResponseModel>> Post(CancellationToken cancellationToken, EventRequestModel request)
        {
            return Ok(await _service.CreateAsync(cancellationToken, request, userIdClaim));
        }
        /// <summary>
        /// Updates a todo and its corresponding subtasks
        /// </summary>
        /// <remarks>
        /// 
        /// PUT/event
        /// 
        ///  {
        ///     "id": 1,
        ///     "title": "Eveeent",
        ///     "description": "Updated desc",
        ///     "startDate": "2023-03-13T16:39:06.224Z",
        ///     "finishDate": "2023-03-13T16:39:06.224Z",
        ///     "numberOfTickets": 30,
        ///     "modificationPeriod": 3
        ///  }
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns>Returns newly created todo with subtasks</returns>
        /// <response code="200">Returns newly created todo with subtasks</response>
        /// <response code="404">If the todo or subtask was not found</response>
        //[ProducesResponseType(typeof(ToDoExample), StatusCodes.Status200OK)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ToDoExample))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [HttpPut("v1/event")]
        public async Task<ActionResult<EventResponseModel>> Put(CancellationToken cancellationToken, UserUpdateEventRequestModel request)
        {
            return Ok(await _service.UpdateAsync(cancellationToken, request, userIdClaim));
        }
        ///// <summary>
        ///// Markes todo status as done
        ///// </summary>
        ///// <param name="cancellationToken"></param>
        ///// <param name="id"></param>
        ///// <returns>Returns todo marked as done</returns>
        ///// <response code="200">Returns updated todo with subtasks</response>
        ///// <response code="404">If the todo was not found</response>
        //[ProducesResponseType(typeof(ToDoExample), StatusCodes.Status200OK)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ToDoExample))]
        //[Produces("application/json")]
        //[HttpPost("v1/todos/{id}/done")]
        //public async Task<ActionResult<ToDoResponseModel>> UpdateToDoStatus(CancellationToken cancellationToken, int id)
        //{
        //    return Ok(await _service.UpdateToDoStatusAsync(cancellationToken, id, userIdClaim));
        //}

        ///// <summary>
        ///// Deletes Todo
        ///// </summary>
        ///// <param name="cancellationToken"></param>
        ///// <param name="id"></param>
        ///// <returns>No content</returns>
        ///// <response code="204">If successfully deleted todo</response>
        ///// <response code="404">If todo not found</response>        
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpDelete("v1/todos/{id}")]
        //public async Task<ActionResult> Delete(CancellationToken cancellationToken, int id)
        //{
        //    await _service.DeleteAsync(cancellationToken, id, userIdClaim);
        //    return NoContent();
        //}
        ///// <summary>
        ///// Updates a single property in the specific todo
        ///// </summary>
        ///// <param name="cancellationToken"></param>
        ///// <param name="id"></param>
        ///// <param name="patchRequest"></param>
        ///// <returns>Updated todo model</returns>
        //[HttpPatch("v1/todos/{id}")]
        //public async Task<ActionResult<ToDoResponseModel>> Patch(CancellationToken cancellationToken, int id, [FromBody] JsonPatchDocument<ToDoUpdateModel> patchRequest)
        //{
        //    return Ok(await _service.PatchAsync(cancellationToken, patchRequest, id, userIdClaim));

        //}
    }
}
