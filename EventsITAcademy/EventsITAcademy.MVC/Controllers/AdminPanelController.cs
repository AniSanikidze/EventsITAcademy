using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Users;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken);
            return View(users);
        }

        public async Task<IActionResult> Events(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllAsync(cancellationToken);
            return View("~/Views/Event/List.cshtml",events);
        }

        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> ActivateEvent(int id,CancellationToken cancellationToken)
        {
            var events = await _eventService.ActivateEvent(cancellationToken,id);
            return RedirectToAction("Events");
        }

        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> EditEvent(int id, CancellationToken cancellationToken)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var @event = await _eventService.GetAsync(cancellationToken, id);
        //    return View(@event.Adapt<AdminUpdateEventRequestModel>());
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Edit(AdminEventRequestModel eventRequest, CancellationToken cancellationToken)
        //{
        //    var role = User.FindFirst(ClaimTypes.Role).Value;
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    if (eventRequest.FinishDate < eventRequest.StartDate)
        //    {
        //        ModelState.AddModelError("", "Finish Date cannot be less than the start date");
        //    }
        //    else if (ModelState.IsValid)
        //    {
        //        await _eventService.UpdateAsync(cancellationToken, eventRequest, null, role);
        //        TempData["success"] = "Event updated successfully";
        //        return RedirectToAction("List");
        //    }
        //    return View();
        //}
    }
}
