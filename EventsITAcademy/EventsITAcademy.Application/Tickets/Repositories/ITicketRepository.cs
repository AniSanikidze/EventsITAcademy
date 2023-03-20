using EventsITAcademy.Application.Tickets.Responses;
using EventsITAcademy.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Tickets.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> GetReservedAsync(CancellationToken cancellationToken, string userId, int eventId);
        Task<List<Ticket>> GetAllReservedAsync(CancellationToken cancellationToken);
        Task<Ticket> Reserve(CancellationToken cancellationToken, Ticket ticket);
        Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, Ticket ticket);
        Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Ticket, bool>> predicate);

    }
}
