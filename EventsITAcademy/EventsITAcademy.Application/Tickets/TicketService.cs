using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Application.Tickets.Requests;
using EventsITAcademy.Application.Tickets.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Domain.Users;
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

        public async Task<TicketResponseModel> Buy(CancellationToken cancellationToken, int eventId, string userId)
        {
            #region Change Ticket Status From Reserved To Sold
            var reservedTicket = await _ticketRepository.GetReservedAsync(cancellationToken, userId, eventId);
            if (reservedTicket != null)
            {
                var soldTicket = reservedTicket.Adapt<Ticket>();
                await _ticketRepository.UpdateTicketStatusAsync(cancellationToken, soldTicket);
                return soldTicket.Adapt<TicketResponseModel>();
            }
            #endregion

            TicketRequestModel ticketRequest = new TicketRequestModel
            {
                EventId = eventId,
                UserId = userId,
                TicketStatus = TicketStatuses.Sold,
            };

            return await AddTicketAsync(cancellationToken, ticketRequest); ;
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

        public async Task<TicketResponseModel> Reserve(CancellationToken cancellationToken,int eventId, string userId)
        {
            TicketRequestModel ticketRequest = new TicketRequestModel
            {
                EventId = eventId,
                UserId = userId,
                TicketStatus = TicketStatuses.Reserved
            };
            return await AddTicketAsync(cancellationToken, ticketRequest);
        }

        public async Task<TicketResponseModel> AddTicketAsync(CancellationToken cancellationToken, TicketRequestModel ticketRequest)
        {
            if (!await _userRepository.Exists(cancellationToken, ticketRequest.UserId))
            {
                throw new Exception("User not found");
            }

            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == ticketRequest.EventId && x.Status == Domain.EntityStatuses.Active))
            {
                throw new Exception("Event not found");
            }

            if (ticketRequest.TicketStatus == TicketStatuses.Reserved)
            {
                if (await _ticketRepository.Exists(cancellationToken, x => x.UserId == ticketRequest.UserId && x.EventId == ticketRequest.EventId
                        && (x.TicketStatus == TicketStatuses.Reserved)))
                {
                    throw new Exception("Ticket is already taken");
                }
            }

            var @event = await _eventRepository.GetAsync(cancellationToken, ticketRequest.EventId);
            if (@event.NumberOfTickets > 0)
            {
                @event.NumberOfTickets -= 1;
                await _eventRepository.UpdateAsync(cancellationToken, @event.Adapt<Event>());
            }

            var ticket = ticketRequest.Adapt<Ticket>();
            if(ticket.TicketStatus == TicketStatuses.Reserved)
                ticket.ReservationDeadline = DateTime.Now.AddMinutes(@event.ReservationPeriod);
            else
                ticket.ReservationDeadline = null;
            var reservedTicket = await _ticketRepository.CreateTicketAsync(cancellationToken, ticket);
            return reservedTicket.Adapt<TicketResponseModel>();
        }
    }
}
