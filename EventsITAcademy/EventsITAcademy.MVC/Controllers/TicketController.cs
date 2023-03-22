using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Tickets;
using EventsITAcademy.Application.Tickets.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EventsITAcademy.MVC.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        public async Task<IActionResult> Reserve(CancellationToken cancellationToken, string userId, int eventId)
        {
            //when the ticket is already taken, it should be handled 
            try
            {
                await _ticketService.Reserve(cancellationToken, eventId, userId);
                TempData["success"] = "Ticket successfully reserved";
            }
            catch(Exception ex)
            {
                TempData["warning"] = ex.Message;
            }
            return RedirectToAction("List", "Event");
        }

        public async Task<IActionResult> Buy(CancellationToken cancellationToken, string userId, int eventId)
        {
            await _ticketService.Buy(cancellationToken, eventId, userId);
            TempData["success"] = "Ticket successfully bought";
            return RedirectToAction("List", "Event");
        }
    }
}
