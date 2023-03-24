﻿using EventsITAcademy.Application.Events;
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
            var result = await _eventService.GetAllConfirmedAsync(cancellation);
            return View(result);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
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
                return RedirectToAction("Events","User");
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
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
                try
                {
                    await _eventService.UpdateAsync(cancellationToken, eventRequest, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    TempData["success"] = "Event updated successfully";
                    return RedirectToAction("Events", "User");
                }
                catch(Exception ex)
                {
                    TempData["warning"] = ex.Message;
                }

            }
            return View();
        }
    }
}
