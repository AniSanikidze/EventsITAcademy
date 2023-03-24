using EventsITAcademy.Application.Admin;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //[Authorize(Roles = "Admin,Moderator")]
        //public async Task<IActionResult> ActivateEvent(CancellationToken cancellationToken, int eventId)
        //{
        //    return Ok(await _eventService.ActivateEvent(cancellationToken, eventId));
        //}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [HttpPut("v1/admin/event")]
        public async Task<IActionResult> UpdateEvent(CancellationToken cancellationToken, AdminUpdateEventRequestModel requestModel)
        {
            return Ok(await _adminService.UpdateEventAsync(cancellationToken, requestModel));
        }

    }
}
