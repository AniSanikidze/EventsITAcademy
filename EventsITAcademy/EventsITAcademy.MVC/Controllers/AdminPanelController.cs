using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsITAcademy.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        public AdminPanelController(IUserService userService, IEventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return View(users);
        }

        public async Task<IActionResult> Events(CancellationToken cancellationToken)
        {
            //await _eventService.Get
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return View(users);
        }
    }
}
