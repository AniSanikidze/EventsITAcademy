using EventsITAcademy.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Events.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<Event,bool>> predicate);
        Task<List<Event>> GetAllUnconfirmedAsync(CancellationToken cancellationToken);

        Task<List<Event>> GetUserEventsAsync(CancellationToken cancellationToken, string userId);
        Task<Event> GetAsync(CancellationToken cancellationToken, int id);
        Task<Event> GetUserEventAsync(CancellationToken cancellationToken, int id, string userId);
        Task CreateAsync(CancellationToken cancellationToken, Event @event);
        Task<Event> UpdateAsync(CancellationToken cancellationToken, Event @event);
        //Task<Event> UpdateToDoStatusAsync(CancellationToken cancellationToken, int id, int ownerId);
        Task DeleteAsync(CancellationToken cancellationToken, int id);
        Task<bool> Exists(CancellationToken cancellationToken, Expression<Func<Event,bool>> predicate);
    }
}
