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
            TicketRequestModel ticketRequestModel = new TicketRequestModel()
            {
                EventId = eventId,
            };
            //when the ticket is already taken, it should be handled 
            await _ticketService.Reserve(cancellationToken, ticketRequestModel, userId);
            return RedirectToAction("List","Event");
        }
    }
}
