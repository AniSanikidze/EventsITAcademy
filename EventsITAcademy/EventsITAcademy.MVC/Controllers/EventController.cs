using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
            var result = await _eventService.GetAllConfirmedAsync(cancellation).ConfigureAwait(false);
            return View(result);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var @event = await _eventService.GetAsync(cancellationToken, id).ConfigureAwait(false);
            return View(@event);
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(EventRequestModel eventRequest, CancellationToken cancellationToken)
        {
            if (eventRequest.FinishDate < eventRequest.StartDate)
            {
                ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    await _eventService.CreateAsync(cancellationToken, eventRequest, User?.FindFirst(ClaimTypes.NameIdentifier)?.Value).ConfigureAwait(false);
                    TempData["success"] = "Event created successfully";
                    return RedirectToAction("Events", "User");
                }
                catch(Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

            }
            return View();
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var @event = await _eventService.GetAsync(cancellationToken, id).ConfigureAwait(false);
            return View(@event.Adapt<UpdateEventRequestModel>());
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateEventRequestModel eventRequest, CancellationToken cancellationToken)
        {
            if (eventRequest.FinishDate < eventRequest.StartDate)
            {
                ModelState.AddModelError("", "Finish Date cannot be less than the start date");
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    await _eventService.UpdateAsync(cancellationToken, eventRequest, User?.FindFirst(ClaimTypes.NameIdentifier)?.Value).ConfigureAwait(false);
                    TempData["success"] = "Event updated successfully";
                    return RedirectToAction("Events", "User");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

            }
            return View();
        }
    }
}
