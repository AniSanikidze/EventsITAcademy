using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Tickets;
using Microsoft.Extensions.Logging;

namespace EventsITAcademy.Workers
{
    public class ServiceWrapper
    {
        private readonly ILogger<ServiceWrapper> _logger;
        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        public ServiceWrapper(ILogger<ServiceWrapper> logger, ITicketService ticketService, IEventService eventService)
        {
            _logger = logger;
            _ticketService = ticketService;
            _eventService = eventService;
        }

        public async Task RemoveTicketReservations(CancellationToken cancellationToken)
        {
            var tickets = await _ticketService.GetAllReservedAsync(cancellationToken).ConfigureAwait(false);
            foreach (var ticket in tickets)
            {
                if(ticket.ReservationDeadline <= DateTime.Now)
                {
                    await _ticketService.RemoveReservationAsync(cancellationToken, ticket.UserId, ticket.EventId).ConfigureAwait(false);
                }
            }
        }
        public async Task ArchiveEvent(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllAsync(cancellationToken).ConfigureAwait(false);
            foreach (var @event in events)
            {
                if (@event.FinishDate <= DateTime.Now)
                {
                    await _eventService.ArchiveEvent(cancellationToken, @event.Id).ConfigureAwait(false);
                }
            }
        }
    }
}
