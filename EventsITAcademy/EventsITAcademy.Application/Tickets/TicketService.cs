using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Application.Tickets.Requests;
using EventsITAcademy.Application.Tickets.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using Mapster;

namespace EventsITAcademy.Application.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public async Task<List<TicketResponseModel>> GetAllReservedAsync(CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetAllReservedAsync(cancellationToken);
            return tickets.Adapt<List<TicketResponseModel>>();
        }

        public async Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, string userId, int eventId)
        {
            var ticket = await _ticketRepository.GetReservedAsync(cancellationToken, userId, eventId);
            if (ticket == null)
                throw new Exception("Ticket not found");

            var @event = await _eventRepository.GetAsync(cancellationToken, eventId);
            @event.NumberOfTickets += 1;
            await _eventRepository.UpdateAsync(cancellationToken, @event);

            return await _ticketRepository.RemoveReservationAsync(cancellationToken, ticket);
        }

        public async Task<TicketResponseModel> Reserve(CancellationToken cancellationToken, TicketRequestModel ticketRequest, string userId)
        {
            if (!await _userRepository.Exists(cancellationToken, userId))
            {
                throw new Exception("User not found");
            }

            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == ticketRequest.EventId && x.Status == Domain.EntityStatuses.Active))
            {
                throw new Exception("Event not found");
            }

            if (await _ticketRepository.Exists(cancellationToken, x => x.UserId == userId && x.EventId == ticketRequest.EventId
            && (x.TicketStatus == TicketStatuses.Reserved || x.TicketStatus == TicketStatuses.Bought)))
            {
                throw new Exception("Ticket is already taken");
            }
            var @event = await _eventRepository.GetAsync(cancellationToken, ticketRequest.EventId);
            if (@event.NumberOfTickets > 0)
            {
                @event.NumberOfTickets -= 1;
                await _eventRepository.UpdateAsync(cancellationToken, @event.Adapt<Event>());
            }

            var ticket = ticketRequest.Adapt<Ticket>();
            ticket.TicketStatus = TicketStatuses.Reserved;
            ticket.UserId = userId;
            ticket.ReservationDeadline = DateTime.Now.AddMinutes(@event.ReservationPeriod);
            var reservedTicket = await _ticketRepository.Reserve(cancellationToken, ticket);
            return reservedTicket.Adapt<TicketResponseModel>();
        }
    }
}
