using EventsITAcademy.Application.Tickets.Responses;
using EventsITAcademy.Domain.Tickets;
using System.Linq.Expressions;

namespace EventsITAcademy.Application.Tickets.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> GetReservedAsync(CancellationToken cancellationToken, string userId, int eventId);
        Task<List<Ticket>> GetAllReservedAsync(CancellationToken cancellationToken);
        Task<Ticket> CreateTicketAsync(CancellationToken cancellationToken, Ticket ticket);
        Task<Ticket> UpdateTicketStatusAsync(CancellationToken cancellationToken, Ticket ticket);
        Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, Ticket ticket);
        Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Ticket, bool>> predicate);

    }
}
