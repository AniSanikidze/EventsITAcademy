using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Roles;
using EventsITAcademy.Application.Users;
using EventsITAcademy.MVC.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace EventsITAcademy.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private IRoleService _roleService;
        public AdminPanelController(IUserService userService, IEventService eventService, IRoleService roleService)
        {
            _userService = userService;
            _eventService = eventService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(cancellationToken).ConfigureAwait(false);
            return View(users);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PendingEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllUnconfirmedAsync(cancellationToken).ConfigureAwait(false);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ActiveEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllConfirmedAsync(cancellationToken).ConfigureAwait(false);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            await _eventService.DeleteAsync(cancellationToken, id).ConfigureAwait(false);
            return RedirectToAction("PendingEvents");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchivedEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllArchivedAsync(cancellationToken).ConfigureAwait(false);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ActivateEvent(int id, CancellationToken cancellationToken)
        {
            await _eventService.ActivateEvent(cancellationToken, id).ConfigureAwait(false);
            return RedirectToAction("ActiveEvents");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditEvent(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var @event = await _eventService.GetAsync(cancellationToken, id).ConfigureAwait(false);
            return View(@event.Adapt<AdminUpdateEventRequestModel>());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditEvent(AdminUpdateEventRequestModel eventRequest, CancellationToken cancellationToken)
        {
            if (eventRequest.FinishDate < eventRequest.StartDate)
            {
                ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    var updatedEventId = await _eventService.UpdateEventByAdminAsync(cancellationToken, eventRequest).ConfigureAwait(false);
                    var updatedEvent = await _eventService.GetAsync(cancellationToken, updatedEventId).ConfigureAwait(false);
                    TempData["success"] = "Event updated successfully";
                    if (!updatedEvent.IsActive)
                        return RedirectToAction("PendingEvents");
                    else
                        return RedirectToAction("ActiveEvents");
                }
                catch (Exception ex)
                {
                    TempData["warning"] = ex.Message;
                }

            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string id, CancellationToken cancellationToken)
        {

            var roles = _roleService.GetRolesAsync(cancellationToken);
            var userRoleName = await _roleService.GetUserRoleAsync(cancellationToken, id).ConfigureAwait(false);
            SelectListItem userRole = null;        
            
            roles.ForEach(x =>
            {
                if (x.Name == userRoleName)
                {
                    userRole = new SelectListItem(x.Name, x.Id, true);
                }
            });

            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    role.Name == userRoleName)).ToList();

            var viewModel = new AssignRoleViewModel
            {
                UserId = id,
                Roles = new SelectList(roles, "Id", "Name", userRole.Value),
                SelectedRole = userRole.Value
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole( AssignRoleViewModel roleViewModel, CancellationToken cancellationToken)
        {
            roleViewModel.UserId = roleViewModel.UserId;
            if (ModelState.IsValid)
            {
                await _roleService.AssignRoleToUserAsync(cancellationToken, roleViewModel.UserId, roleViewModel.SelectedRole).ConfigureAwait(false);
            }
            else
            {
                return View();
            }
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            await _roleService.RemoveUserRoleAsync(cancellationToken, id).ConfigureAwait(false);
            await _userService.DeleteUserAsync(cancellationToken, id).ConfigureAwait(false);
            return RedirectToAction("Users");
        }
    }
}
