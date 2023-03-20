using EventsITAcademy.Application.Tickets.Repositories;
using EventsITAcademy.Domain.Tickets;
using EventsITAcademy.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Infrastructure.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        readonly ApplicationContext _applicationContext;

        #region Ctor
        public TicketRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Ticket,bool>> predicate)
        {
            return await _applicationContext.Tickets.AnyAsync(predicate, cancellationToken);
        }

        public async Task<List<Ticket>> GetAllReservedAsync(CancellationToken cancellationToken)
        {
            return await _applicationContext.Tickets.Where(x => x.TicketStatus == TicketStatuses.Reserved).ToListAsync(cancellationToken);
        }

        public async Task<Ticket> GetReservedAsync(CancellationToken cancellationToken, string userId, int eventId)
        {
            return await _applicationContext.Tickets.SingleOrDefaultAsync(x => x.UserId == userId && x.EventId == eventId && x.TicketStatus == TicketStatuses.Reserved, cancellationToken);
        }

        public async Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, Ticket ticket)
        {
            _applicationContext.Tickets.Remove(ticket);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        #endregion
        public async Task<Ticket> Reserve(CancellationToken cancellationToken, Ticket ticket)
        {
            await _applicationContext.Tickets.AddAsync(ticket, cancellationToken);
            await _applicationContext.SaveChangesAsync(cancellationToken);
            return ticket;
        }
    }
}
