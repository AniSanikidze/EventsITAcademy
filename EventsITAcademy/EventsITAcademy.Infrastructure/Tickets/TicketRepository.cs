using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventsITAcademy.Infrastructure.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _applicationContext;

        #region Ctor
        public TicketRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Ticket,bool>> predicate)
        {
            return await _applicationContext.Tickets.AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Ticket>> GetAllReservedAsync(CancellationToken cancellationToken)
        {
            return await _applicationContext.Tickets.Where(x => x.TicketStatus == TicketStatuses.Reserved).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Ticket> GetReservedAsync(CancellationToken cancellationToken, string userId, int eventId)
        {
            return await _applicationContext.Tickets.SingleOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId && x.TicketStatus == TicketStatuses.Reserved, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, Ticket ticket)
        {
            _applicationContext.Tickets.Remove(ticket);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }

        #endregion
        public async Task<Ticket> CreateTicketAsync(CancellationToken cancellationToken, Ticket ticket)
        {
            await _applicationContext.Tickets.AddAsync(ticket, cancellationToken).ConfigureAwait(false);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return ticket;
        }

        public async Task<Ticket> UpdateTicketStatusAsync(CancellationToken cancellationToken, Ticket ticket)
        {
            var retrievedTicket = await GetReservedAsync(cancellationToken, ticket.UserId, ticket.EventId).ConfigureAwait(false);
            _applicationContext.Entry(retrievedTicket).State = EntityState.Detached;
            ticket.CreatedAt = retrievedTicket.CreatedAt;
            ticket.TicketStatus = TicketStatuses.Sold;
            ticket.ReservationDeadline = null;

            _applicationContext.Tickets.Update(ticket);
            await _applicationContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return ticket;
        }
    }
}
