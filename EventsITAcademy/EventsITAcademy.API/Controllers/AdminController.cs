using EventsITAcademy.Application.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EventsITAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEventService _eventService;

        public AdminController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> ActivateEvent(CancellationToken cancellationToken, int eventId)
        {
            return Ok(await _eventService.ActivateEvent(cancellationToken, eventId));
        }
    }
}
