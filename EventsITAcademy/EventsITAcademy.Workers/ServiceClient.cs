using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Tickets;
using Microsoft.Extensions.Logging;

namespace EventsITAcademy.Workers
{
    public class ServiceClient
    {
        private readonly ILogger<ServiceClient> _logger;
        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        public ServiceClient(ILogger<ServiceClient> logger, ITicketService ticketService, IEventService eventService)
        {
            _logger = logger;
            _ticketService = ticketService;
            _eventService = eventService;
        }

        public async Task RemoveTicketReservations(CancellationToken cancellationToken)
        {
            var tickets = await _ticketService.GetAllReservedAsync(cancellationToken);
            foreach (var ticket in tickets)
            {
                if(ticket.ReservationDeadline <= DateTime.Now)
                {
                    await _ticketService.RemoveReservationAsync(cancellationToken, ticket.UserId, ticket.EventId);
                }
            }
        }
        public async Task ArchiveAndLimitEventEdit(CancellationToken cancellationToken)
        {
            var events = await _eventService.GetAllAsync(cancellationToken);
            foreach (var @event in events)
            {
                if (@event.FinishDate <= DateTime.Now)
                {
                    await _eventService.ArchiveEvent(cancellationToken, @event.Id);
                }

                //if (@event.CreatedAt.AddMinutes(@event.ModificationPeriod) <= DateTime.Now)
                //{
                //    await _eventService.SetEventUneditableAsync(cancellationToken, @event.Id);
                //}
            }
        }
    }
}
