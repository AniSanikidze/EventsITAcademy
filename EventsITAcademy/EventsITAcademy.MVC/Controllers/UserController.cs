using EventsITAcademy.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsITAcademy.MVC.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Events(CancellationToken cancellationToken)
        {
            var events = await _userService.GetUserEventsAsync(cancellationToken, User?.FindFirst(ClaimTypes.NameIdentifier)?.Value).ConfigureAwait(false);
            return View("~/Views/User/Events.cshtml", events);
        }
    }
}
