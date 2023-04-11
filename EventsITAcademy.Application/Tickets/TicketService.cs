using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Application.Tickets.Requests;
using EventsITAcademy.Application.Tickets.Responses;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Domain.Users;
using Mapster;
using Utilities.Localizations;

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
            var reservedTicket = await _ticketRepository.GetReservedAsync(cancellationToken, userId, eventId).ConfigureAwait(false);
            if (reservedTicket != null)
            {
                var soldTicket = reservedTicket;
                await _ticketRepository.UpdateTicketStatusAsync(cancellationToken, soldTicket).ConfigureAwait(false);
                return soldTicket.Adapt<TicketResponseModel>();
            }
            #endregion

            var ticketRequest = new TicketRequestModel
            {
                EventId = eventId,
                UserId = userId,
                TicketStatus = TicketStatuses.Sold,
            };

            return await AddTicketAsync(cancellationToken, ticketRequest).ConfigureAwait(false);
        }
        public async Task<TicketResponseModel> Reserve(CancellationToken cancellationToken, int eventId, string userId)
        {
            var ticketRequest = new TicketRequestModel
            {
                EventId = eventId,
                UserId = userId,
                TicketStatus = TicketStatuses.Reserved
            };
            return await AddTicketAsync(cancellationToken, ticketRequest).ConfigureAwait(false);
        }
        public async Task<List<TicketResponseModel>> GetAllReservedAsync(CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetAllReservedAsync(cancellationToken).ConfigureAwait(false);
            return tickets.Adapt<List<TicketResponseModel>>();
        }
        public async Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, string userId, int eventId)
        {
            var ticket = await _ticketRepository.GetReservedAsync(cancellationToken, userId, eventId).ConfigureAwait(false);
            if (ticket == null)
                throw new ItemNotFoundException(ClassNames.Ticket + " " + ErrorMessages.NotFound, null);

            var @event = await _eventRepository.GetAsync(cancellationToken, eventId).ConfigureAwait(false);
            @event.NumberOfTickets += 1;
            await _eventRepository.UpdateAsync(cancellationToken, @event).ConfigureAwait(false);

            return await _ticketRepository.RemoveReservationAsync(cancellationToken, ticket).ConfigureAwait(false);
        }
        public async Task<TicketResponseModel> AddTicketAsync(CancellationToken cancellationToken, TicketRequestModel ticketRequest)
        {
            if (!await _userRepository.Exists(cancellationToken, ticketRequest.UserId).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.User + " " + ErrorMessages.NotFound, nameof(User));
            }

            if (!await _eventRepository.Exists(cancellationToken, x => x.Id == ticketRequest.EventId && x.Status == Domain.EntityStatuses.Active).ConfigureAwait(false))
            {
                throw new ItemNotFoundException(ClassNames.Event + " " + ErrorMessages.NotFound, nameof(Event));
            }

            if (ticketRequest.TicketStatus == TicketStatuses.Reserved)
            {
                if (await _ticketRepository.Exists(cancellationToken, x => x.UserId == ticketRequest.UserId && x.EventId == ticketRequest.EventId
                        && (x.TicketStatus == TicketStatuses.Reserved)).ConfigureAwait(false))
                {
                    throw new ItemAlreadyExistsException(ErrorMessages.TicketReserved, "TicketAlreadyReserved");
                }
            }

            var @event = await _eventRepository.GetAsync(cancellationToken, ticketRequest.EventId).ConfigureAwait(false);
            if (@event.NumberOfTickets > 0)
            {
                @event.NumberOfTickets -= 1;
                await _eventRepository.UpdateAsync(cancellationToken, @event).ConfigureAwait(false);
            }
            else
            {
                throw new NoTicketsLeftException(ErrorMessages.NoTicketsLeft);
            }

            var ticket = ticketRequest.Adapt<Ticket>();
            if (ticket.TicketStatus == TicketStatuses.Reserved)
                ticket.ReservationDeadline = DateTime.Now.AddMinutes(@event.ReservationPeriod);
            else
                ticket.ReservationDeadline = null;
            var reservedTicket = await _ticketRepository.CreateTicketAsync(cancellationToken, ticket).ConfigureAwait(false);
            return reservedTicket.Adapt<TicketResponseModel>();
        }
    }
}
