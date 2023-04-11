using EventsITAcademy.Domain.Events;
using System.Linq.Expressions;

namespace EventsITAcademy.Application.Events.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<Event,bool>> predicate);
        Task<List<Event>> GetAllUnconfirmedAsync(CancellationToken cancellationToken);
        Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId);
        Task<Event> GetAsync(CancellationToken cancellationToken, int id);
        Task<Event> GetUserEventAsync(CancellationToken cancellationToken, int id, string userId);
        Task<int> CreateAsync(CancellationToken cancellationToken, Event @event);
        Task<int> UpdateAsync(CancellationToken cancellationToken, Event @event);
        Task<Event> UpdateEventModResPeriodsAsync(CancellationToken cancellationToken, Event @event);
        Task<int> DeleteAsync(CancellationToken cancellationToken, int id);
        Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Event,bool>> predicate);
    }
}
