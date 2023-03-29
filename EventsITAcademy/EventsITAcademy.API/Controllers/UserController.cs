using EventsITAcademy.API.Infrastructure.Auth;
using EventsITAcademy.API.Infrastructure.SwaggerExamples;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Controllers
{
    [Route("api/v{version:apiVersion}/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfiguration> _options;
        private readonly string _userIdClaim;

        public UserController(IHttpContextAccessor httpContextAccessor,IUserService userService, IOptions<JWTConfiguration> options)
        {
            if (httpContextAccessor?.HttpContext.User.Claims.Count() > 0)
            {
                _userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("UserId")!.Value;
            }
            _userService = userService;
            _options = options;
        }

        /// <summary>
        /// registers user with provided username and password
        /// </summary>
        /// <remarks>
        /// Note id is not required
        ///
        ///     POST/User
        ///     
        ///      {
        ///         "username": "ExampleUser",
        ///         "email": "exampleuser@gmail.com",
        ///         "password": "ExamplePassword"
        ///      }   
        /// </remarks>
        /// <param name="cancellation"></param>
        /// <param name="user"></param>
        /// <returns>Returns newly created user data</returns>
        /// <response code="200">Returns new user</response>
        /// <response code="400">Invalid request coming from user</response>
        [ProducesResponseType(typeof(UserExample), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserExample))]
        [Produces("application/json")]
        [Route("register")]
        [HttpPost]
        public async Task<UserResponseModel> Register(CancellationToken cancellation, CreateUserRequestModel user)
        {
            return await _userService.CreateAsync(cancellation, user).ConfigureAwait(false);
        }
        /// <summary>
        /// logs in the user with provided username and password
        /// </summary>
        /// <remarks>
        /// Note id is not required
        ///
        ///     POST/User
        ///     
        ///      {
        ///         "email": "exampleuser@gmail.com",
        ///         "password": "ExamplePassword"
        ///      }   
        /// </remarks>
        /// <param name="cancellation"></param>
        /// <param name="request"></param>
        /// <returns>Returns user details and jwt token after the user has successfully logged into the system</returns>
        /// <response code="200">Returns user and jwt token</response>
        /// <response code="400">Invalid request coming from user</response>
        /// <response code="401">If user login failed</response>
        [ProducesResponseType(typeof(LoggedInUserExample), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoggedInUserExample))]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [Route("login")]
        [HttpPost]
        public async Task<LoggedInUserResponseModel> LogIn(CancellationToken cancellation, LoginUserRequestModel request)
        {
            var result = await _userService.AuthenticateAsync(cancellation, request).ConfigureAwait(false);
            result.Token = JWTHelper.GenerateSecurityToken(result.UserName, Guid.Parse(result.Id),result.Role, _options);
            return result;
        }
        /// <summary>
        /// Returns list of user's events
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>list of user's events</returns>
        /// <returns status="200">user's events</returns>
        /// <returns status="404">If the user was not found</returns>
        [ProducesResponseType(typeof(IEnumerable<EventResponseModel>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EventExamples))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "User")]
        [HttpGet("events")]
        public async Task<ActionResult<IEnumerable<EventResponseModel>>> GetUserEvents(CancellationToken cancellationToken)
        {
            return Ok(await _userService.GetUserEventsAsync(cancellationToken, _userIdClaim).ConfigureAwait(false));
        }
    }
}
