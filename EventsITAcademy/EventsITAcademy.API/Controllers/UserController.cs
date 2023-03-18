using EventsITAcademy.API.Infrastructure.Auth;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Application.Users.Requests;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace EventsITAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfiguration> _options;

        public UserController(IUserService userService, IOptions<JWTConfiguration> options)
        {
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
        ///         "password": "ExamplePassword"
        ///      }   
        /// </remarks>
        /// <param name="cancellation"></param>
        /// <param name="user"></param>
        /// <returns>Returns newly created user data</returns>
        /// <response code="200">Returns new user</response>
        //[ProducesResponseType(typeof(UserExample), StatusCodes.Status200OK)]
        //[SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserExample))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [Route("/v1/users")]
        [HttpPost]
        public async Task<UserResponseModel> Register(CancellationToken cancellation, CreateUserRequestModel user)
        {
            return await _userService.CreateAsync(cancellation, user);
        }
        [Route("v1/users/access-token")]
        [HttpPost]
        public async Task<LoggedInUserResponseModel> LogIn(CancellationToken cancellation, LoginUserRequestModel request)
        {
            var result = await _userService.AuthenticateAsync(cancellation, request);
            result.Token = JWTHelper.GenerateSecurityToken(result.UserName, Guid.Parse(result.Id),result.Role, _options);
            return result;
        }
    }
}
