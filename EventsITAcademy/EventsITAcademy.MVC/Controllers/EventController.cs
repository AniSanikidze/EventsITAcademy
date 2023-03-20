using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsITAcademy.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> List(CancellationToken cancellation = default)
        {
            var result = await _eventService.GetAllAsync(cancellation);
            return View(result);
        }

        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken)
        {
            var @event = await _eventService.GetAsync(cancellationToken, id);
            return View(@event);
        }

        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        public async Task<IActionResult> Create(EventRequestModel eventRequest, CancellationToken cancellationToken)
        {
            if (eventRequest.FinishDate < eventRequest.StartDate)
            {
                ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            }
            else if (ModelState.IsValid)
            {
                await _eventService.CreateAsync(cancellationToken, eventRequest, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                TempData["success"] = "Event created successfully";
                return RedirectToAction("List");
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var @event = await _eventService.GetAsync(cancellationToken, id);
            return View(@event.Adapt<UserUpdateEventRequestModel>());
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateEventRequestModel eventRequest, CancellationToken cancellationToken)
        {
            var role = User;
            if (eventRequest.FinishDate < eventRequest.StartDate)
            {
                ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            }
            else if (ModelState.IsValid)
            {
                //await _eventService.UpdateAsync(cancellationToken, eventRequest, "525c0c12-81e7-4ad7-9b21-8c3d3b728f4b");
                TempData["success"] = "Event updated successfully";
                return RedirectToAction("List");
            }
            return View();
        }
    }
}
