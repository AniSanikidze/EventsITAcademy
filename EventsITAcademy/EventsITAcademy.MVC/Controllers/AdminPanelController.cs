using EventsITAcademy.Application.Admin;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Roles;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Domain.Users;
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
        private IAdminService _adminService;
        private IRoleService _roleService;
        public AdminPanelController(IUserService userService, IEventService eventService, IAdminService adminService, IRoleService roleService)
        {
            _userService = userService;
            _eventService = eventService;
            _adminService = adminService;
            _roleService = roleService;
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

        public async Task<IActionResult> PendingEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllUnconfirmedAsync(cancellationToken);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }

        public async Task<IActionResult> ActiveEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllConfirmedAsync(cancellationToken);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }

        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllConfirmedAsync(cancellationToken);
            await _adminService.DeleteEventAsync(cancellationToken, id);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }

        public async Task<IActionResult> ArchivedEvents(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllArchivedAsync(cancellationToken);
            return View("~/Views/AdminPanel/Events.cshtml", events);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ActivateEvent(int id, CancellationToken cancellationToken)
        {
            await _eventService.ActivateEvent(cancellationToken, id);
            return RedirectToAction("ActiveEvents");
        }

        [Authorize]
        public async Task<IActionResult> EditEvent(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var @event = await _eventService.GetAsync(cancellationToken, id);
            return View(@event.Adapt<AdminUpdateEventRequestModel>());
        }
        [Authorize]
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
                    var updatedEvent = await _adminService.UpdateEventAsync(cancellationToken, eventRequest);
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

        [Authorize]
        public async Task<IActionResult> AssignRole(string id, CancellationToken cancellationToken)
        {

            var roles = _roleService.GetRolesAsync(cancellationToken);
            var userRoleName = await _roleService.GetUserRoleAsync(cancellationToken, id);
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

        [HttpPost]
        public async Task<IActionResult> AssignRole( AssignRoleViewModel roleViewModel, CancellationToken cancellationToken)
        {
            roleViewModel.UserId = roleViewModel.UserId;
            await _roleService.AssignRoleToUserAsync(cancellationToken, roleViewModel.UserId, roleViewModel.SelectedRole);
            //if (eventRequest.FinishDate < eventRequest.StartDate)
            //{
            //    ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            //}
            //else if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var updatedEvent = await _adminService.UpdateEventAsync(cancellationToken, eventRequest);
            //        TempData["success"] = "Event updated successfully";
            //        if (!updatedEvent.IsActive)
            //            return RedirectToAction("PendingEvents");
            //        else
            //            return RedirectToAction("ActiveEvents");
            //    }
            //    catch (Exception ex)
            //    {
            //        TempData["warning"] = ex.Message;
            //    }

            //}
            return RedirectToAction("Users");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            await _roleService.RemoveUserRoleAsync(cancellationToken, id);
            await _adminService.DeleteUserAsync(cancellationToken, id);
            return RedirectToAction("Users");
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
